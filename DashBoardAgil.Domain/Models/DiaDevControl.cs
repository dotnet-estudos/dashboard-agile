using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Redmine.Net.Api.Types;

namespace DashBoardAgil.Domain.Models
{
    public class DiaDevControl
    {
        public Projeto Time { get; set; }

        public List<Alocacao> Lancamentos { get; set; }

        public List<LancamentosDiaDesenvolvedor> LancamentosDiaDesenvolvedor
        {
            get
            {
                var qtdDiaDev = from lanc in Lancamentos
                                orderby lanc.SpentOn
                                group lanc by new { lanc.SpentOn, lanc.User } into dias
                                select new LancamentosDiaDesenvolvedor { Dia = (DateTime)dias.Key.SpentOn,  User = new User() { Id = dias.Key.User.Id, FirstName = dias.Key.User.Name } , LancamentosDia = dias.Count(), TotalHoraDia = dias.Sum(x => x.Hours) };

                return qtdDiaDev.ToList();
            }
        }

        public List<LancamentosDia> LancamentosDia
        {
            get
            {
                var diaDev = from lac in this.LancamentosDiaDesenvolvedor.ToList()
                             orderby lac.Dia
                             group lac by lac.Dia
                    into d
                             select new LancamentosDia { Dia = (DateTime)d.Key, Quantidade = d.Count(), Desenvolvedores = d.Select(x => x.User).ToList() };

                return diaDev.ToList();
            }
        }
    }

    public class LancamentosDiaDesenvolvedor
    {
        public DateTime Dia { get; set; }
        public string DiaString => ((DateTime) this.Dia).ToString("dd/MM/yyyy");
        public User User { get; set; }
        public int  LancamentosDia { get; set; }
        public decimal TotalHoraDia { get; set; }


    }

    public class LancamentosDia
    {
        public DateTime Dia { get; set; }
        public string DiaString => ((DateTime)this.Dia).ToString("dd/MM/yyyy");
        public List<User> Desenvolvedores { get; set; }
        public String[] Devs => Desenvolvedores.Select(x => x.FirstName).OrderBy(x => x).ToArray();
        public int Quantidade { get; set; }
        public decimal TotalHoraDia { get; set; }
    }
}
