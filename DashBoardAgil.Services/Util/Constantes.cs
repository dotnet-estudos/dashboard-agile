using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DashBoardAgil.Services.Enums;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using DashBoardAgil.CrossCutting;

namespace DashBoardAgil.Services.Util
{
    public class Constantes
    {

        public static class Details
        {
            public const string Name_Attr = "attr";
            public const string Name_Tag = "tag_list";
            public const string Value_AguardandoInformacao = "Aguardando Informação";
            public const string Value_Rejeitado = "Rejeitado";
        }

        public static class Mensagens
        {
            public const string NOT_FOUND = "Não foi Possível encontrar {0}";
        }

        public static List<string> StatusBackLogNovo = new List<string>
        {
            StatusRedmine.Novo.GetHashCode().ToString(),
            StatusRedmine.BacklogNovo.GetHashCode().ToString()
        };

        public static List<string> StatusBackLogAberto = new List<string>
        {
            StatusRedmine.Aberto.GetHashCode().ToString(),
            StatusRedmine.BacklogAberto.GetHashCode().ToString()
        };

        public static List<string> StatusDesenvolvimentoAnddamento = new List<string>
        {
            StatusRedmine.DesenvolvimentoAndamento.GetHashCode().ToString(),
            StatusRedmine.Andamento.GetHashCode().ToString(),
        };

        public static List<string> StatusDesenvolvimentoResolvido = new List<string>
        {
            StatusRedmine.DesenvolvimentoResolvido.GetHashCode().ToString(),
            StatusRedmine.Resolvido.GetHashCode().ToString()
        };

        public static List<string> StatusHomologacaoAndamento = new List<string>
        {
            StatusRedmine.HomologacaoAndamento.GetHashCode().ToString(),
            StatusRedmine.Homologacao.GetHashCode().ToString()
        };

        public static List<string> StatusHomologacaoValidado = new List<string>
        {
            StatusRedmine.HomologacaoValidado.GetHashCode().ToString(),
            StatusRedmine.Validado.GetHashCode().ToString()
        };

        public static List<string> StatusPreHomologacao = new List<string>
        {
            StatusRedmine.PreHmlAFazer.GetHashCode().ToString(),
            StatusRedmine.PreHmlFazendo.GetHashCode().ToString(),
            StatusRedmine.PreHmlFeito.GetHashCode().ToString()
        };

        public static List<string> StatusProducao = new List<string>
        {
            StatusRedmine.ProducaoAvaliar.GetHashCode().ToString(),
            StatusRedmine.AvaliarProducao.GetHashCode().ToString()
        };




        public static List<string> FaseBackLog = new List<string>()
                                                           .Concat(Constantes.StatusBackLogNovo)
                                                           .Concat(Constantes.StatusBackLogAberto).ToList();

        public static List<string> FaseDesnvolvimento = new List<string>()
                                                            .Concat(Constantes.StatusDesenvolvimentoResolvido)
                                                            .Concat(Constantes.StatusDesenvolvimentoAnddamento).ToList();

        public static List<string> FaseHomologacao = new List<string>()
                                                            .Concat(Constantes.StatusHomologacaoAndamento)
                                                            .Concat(Constantes.StatusHomologacaoValidado).ToList();

        public static List<string> FaseProducao = new List<string>().Concat(Constantes.StatusProducao).ToList();

        public static List<String> StatusChamadosAbertos = new List<string>()
                                                            .Concat(Constantes.FaseBackLog)
                                                            .Concat(Constantes.FaseDesnvolvimento)
                                                            .Concat(Constantes.FaseHomologacao)
                                                            .Concat(Constantes.FaseProducao)
                                                            .ToList();

        public static List<String> FasePreHomologacao = new List<string>()
                                                            .Concat(Constantes.StatusPreHomologacao)
                                                            .ToList();

        public static List<String> StatusChamadosPendenteInterno = new List<string>() { StatusRedmine.PreHmlAFazer.GetHashCode().ToString(), StatusRedmine.PreHmlFazendo.GetHashCode().ToString() }
                                                            .Concat(Constantes.FaseBackLog)
                                                            .Concat(Constantes.FaseDesnvolvimento)
                                                            //.Concat(Constantes.FaseHomologacao)
                                                            //.Concat(Constantes.FaseProducao)
                                                            .ToList();

        public static List<String> StatusChamadosPendenteCliente = new List<string>() {  StatusRedmine.PreHmlFeito.GetHashCode().ToString(),
                                                                                         StatusRedmine.HomologacaoAndamento.GetHashCode().ToString(),
                                                                                      }
                                                           .ToList();


        /// <summary>
        /// Detail BacklogAberto
        /// </summary>
        public static readonly Func<Detail, bool> DetailBaclogAberto = det => det.Property.Equals("attr") && det.Name.Equals(RedmineKeys.STATUS_ID) && Constantes.StatusBackLogAberto.Contains(det.NewValue);
        /// <summary>
        /// Status BackLogABerto = 119
        /// </summary>
        public static readonly Func<Journal, bool> JornalBaclogAberto = jornal => jornal.Details.Any(Constantes.DetailBaclogAberto);
        /// <summary>
        /// Expressão Lambda para chamados em Desenvolvimento
        /// </summary>
        public static readonly Func<Issue, bool> ChamadoFaseBackLog = fase => Constantes.FaseBackLog.Contains(fase.Status.Id.ToString());
        /// <summary>
        /// Expressão Lambda para chamados em Homologação
        /// </summary>
        public static readonly Func<Issue, bool> ChamadoFaseDesenvolvimento = fase => Constantes.FaseDesnvolvimento.Contains(fase.Status.Id.ToString());
        /// <summary>
        /// Expressão Lambda para chamados em Hõmologação
        /// </summary>
        public static readonly Func<Issue, bool> ChamadoFaseHomologacao = fase => Constantes.FaseHomologacao.Contains(fase.Status.Id.ToString());

        /// <summary>
        /// Expressão Lambda para chamados em Produção
        /// </summary>
        public static readonly Func<Issue, bool> ChamadoFaseProducao = fase => Constantes.FaseProducao.Contains(fase.Status.Id.ToString());

        /// <summary>
        /// Expressão Lambda para chamados em Produção
        /// </summary>
        public static readonly Func<Issue, bool> ChamadoPendenteInterno = fase => Constantes.StatusChamadosPendenteInterno.Contains(fase.Status.Id.ToString());

        public static readonly Func<Issue, bool> ChamadoPendenteCliente = fase => (Constantes.StatusChamadosPendenteCliente.Contains(fase.Status.Id.ToString()) && !(fase.Journals.Any(jorn=>jorn.Details.Any(det=>det.Name.Equals(Constantes.Details.Name_Tag) && det.NewValue.Contains(Details.Value_AguardandoInformacao)) && !jorn.Details.Any(det => det.Name.Equals(Constantes.Details.Name_Tag) && det.OldValue.Contains(Details.Value_AguardandoInformacao)))));

        public static readonly Func<Issue, bool> ChamadosEmAguardandoInformacao = chamad =>
                                                                     (chamad.Journals.Count(jorn => jorn.Details.Any(det => det.Name.Equals(Constantes.Details.Name_Tag) && det.NewValue.Contains(Details.Value_AguardandoInformacao) && !det.OldValue.Contains(Details.Value_AguardandoInformacao))) !=
                                                                      chamad.Journals.Count(jorn => jorn.Details.Any(det => det.Name.Equals(Constantes.Details.Name_Tag) && det.OldValue.Contains(Details.Value_AguardandoInformacao) && !det.NewValue.Contains(Details.Value_AguardandoInformacao))));

        public static readonly Func<Issue, bool> ChamadosRejeitados = chamad => 
                                                                     (chamad.Journals.Count(jorn => jorn.Details.Any(det => det.Name.Equals(Constantes.Details.Name_Tag) && det.NewValue.Contains(Details.Value_Rejeitado) && !det.OldValue.Contains(Details.Value_Rejeitado))) !=
                                                                      chamad.Journals.Count(jorn => jorn.Details.Any(det => det.Name.Equals(Constantes.Details.Name_Tag) && det.OldValue.Contains(Details.Value_Rejeitado) && !det.NewValue.Contains(Details.Value_Rejeitado))));
        public static Func<Detail, bool> GetDetail(string newvalue, string Name, string prop)
        {
            Func<Detail, bool> retorno = det => det.Property.Equals(prop) && det.Name.Equals(Name) && det.NewValue.Equals(newvalue);
            return retorno;
        }

        public static Func<Issue, bool> GetChamadoOntag(string tag_name)
        {
           Func<Issue, bool> chan = chamad =>  (chamad.Journals.Count(jorn => jorn.Details.Any(det => det.Name.Equals(Constantes.Details.Name_Tag) && det.NewValue.Contains(tag_name) && !det.OldValue.Contains(tag_name))) !=
                                                chamad.Journals.Count(jorn => jorn.Details.Any(det => det.Name.Equals(Constantes.Details.Name_Tag) && det.OldValue.Contains(tag_name) && !det.NewValue.Contains(tag_name))));
            return chan;
        }

    }
}
