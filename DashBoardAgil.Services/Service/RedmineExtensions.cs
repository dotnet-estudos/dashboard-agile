using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using DashBoardAgil.CrossCutting;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using DashBoardAgil.Services.Util;
using DashBoardAgil.CrossCutting.Enums;

namespace DashBoardAgil.Services.Service
{

    public class RedmineExtensions : RedmineManager
    {
        public RedmineExtensions(string host, string usuario, string senha) : base(host, usuario, senha)
        {
        }

        /// <summary>
        /// Método para pegar Issues com os Journals incluídos. (método GetObjects não está trazendo Journals)
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<Issue> GetObjectsWithJournals(NameValueCollection parameters)
        {
            List<Issue> lista = new List<Issue>();
            try
            {
                lista = GetObjects<Issue>(parameters);

            }
            catch (Exception ex) { throw ex; }

            var retorno = lista.Select(x => x.Id).Select(id => GetObject<Issue>(id.ToString(), new NameValueCollection { { RedmineKeys.INCLUDE, RedmineKeys.JOURNALS } })).ToList();

            return retorno;

        }

        #region Journal
        private bool JournalPossuiTag(Journal journal)
        {
            return journal.Details.Any(x => x.Name == "tag_list");
        }

        private bool JournalTagsRemovidas(Journal journal)
        {
            return journal.Details.Any(x => x.Name == "tag_list" && x.NewValue == string.Empty);
        }
        #endregion

        private void FecharChamado(Issue chamado)
        {
            chamado.Status = new IdentifiableName { Id = StatusRedmine.Fechado.GetHashCode(), Name = StatusRedmine.Fechado.ToString() };
            chamado.Notes = GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.MensagemFechamento;

            UpdateObject(chamado.Id.ToString(), chamado);
        }

        public List<string> FecharChamados(List<Issue> listaChamados, int prazo, string[] situacoes)
        {


            List<string> result = new List<string>();
            try
            {
                foreach (var chamado in listaChamados.Where(x => x.Journals.Any()))
                {
                    try
                    {
                        Journal ultimoJournal = chamado.Journals.OrderBy(x => x.CreatedOn).LastOrDefault();
                        Journal ultimoJournalComTag =
                            chamado.Journals.OrderBy(x => x.CreatedOn).LastOrDefault(JournalPossuiTag);

                        if (ultimoJournalComTag == null || JournalTagsRemovidas(ultimoJournalComTag))
                        {
                            if (DateTime.Now > ultimoJournal.CreatedOn.Value.AddDiasUteis(prazo))
                            {
                                FecharChamado(chamado);
                                result.Add(string.Format("[FECHADO] - #{0}: {1}", chamado.Id, chamado.Subject));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debugar.WriteLine(String.Format("#{2} - {3} - FecharChamados Error: {0} inner: {1}", ex.Message, ex.InnerException?.Message ?? "", chamado.Id, chamado.Subject));
                    }

                }
            }
            catch (Exception ex)
            {
                Debugar.WriteLine(String.Format("FecharChamados Error: {0} inner: {1}", ex.Message,
                    ex.InnerException?.Message ?? ""));
            }
            return result;
        }
    }
}

