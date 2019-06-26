using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DashBoardAgil.Services.Enums;
using DashBoardAgil.Services.Extensions;
using DashBoardAgil.Services.Interfaces.Abstracoes;
using Redmine.Net.Api.Types;

namespace DashBoardAgil.Services.Domain
{
    public class LeadTimeControl
    {
        public LeadTimeControl(int primeiroSalto = 5, int quantidadeSalto = 10, List<Issue> universo = null)
        {
            this.LeadTimes = new List<LeadTime>();
            this.PrimeiroSalto = primeiroSalto;
            this.QuantidadeSaltos = quantidadeSalto;

            #region teste
            if (universo != null)
            {
                UniversoChamados = new List<LeadTime>();
                universo.ForEach(x=>
                {
                    var lead = new LeadTime()
                    {
                        IdChamado = x.Id,
                        Tipo = (TipoChamadoRedmine)x.Tracker.Id,
                        Prioridade = (PrioridadeRedmine)x.Priority.Id,
                        //LeadDiaUtil = ((DateTime)dateStatusPara).Diferenca((DateTime)dateStatusDe, TipoContagem.Dia, true),
                        Projeto = x.Project.Name,
                        CreatedOn = (DateTime)x.CreatedOn,
                        // ClosedOn = (DateTime) chamado.ClosedOn,
                        Versao = CrossCutting.Util.ReflectObjec(x.FixedVersion, new IdentificableName()),
                        Status = CrossCutting.Util.ReflectObjec(x.Status, new IdentificableName())

                        //Tempo = (int)((DateTime)dateStatusDe).Diferenca((DateTime)dateStatusPara, TipoContagem.Hora, true)  
                    };
                    if (x.ClosedOn != null) lead.ClosedOn = (DateTime)x.ClosedOn;

                    UniversoChamados.Add(lead);
                });
            }
            #endregion

        }

        private int PrimeiroSalto { get; }
        private int QuantidadeSaltos { get; }
        public List<LeadTime> UniversoChamados { get; set; }
        public List<LeadTime> LeadTimes { get; set; }
        public int TotalChamados => LeadTimes.Count();
        private double TotalChamadosDouble => Double.Parse(this.TotalChamados.ToString());
        public double GeneralAverageHours => Math.Round(LeadTimes.Average(x => x.LeadBruto), 2, MidpointRounding.AwayFromZero);
        public double GeneralAverageDays => Math.Round((GeneralAverageHours/24), 2, MidpointRounding.AwayFromZero);
        public string GeneralAverageDaysString => TimeSpan.FromHours(GeneralAverageHours).ToString(@"dd\.hh\:mm\:ss");

        public double GeneralAverageHoursUtil => Math.Round(LeadTimes.Average(x => x.LeadUtil), 2, MidpointRounding.AwayFromZero);
        public double GeneralAverageDaysUtil => Math.Round(GeneralAverageHoursUtil/7.2, 2, MidpointRounding.AwayFromZero);
        public string GeneralAverageDaysUtilString => TimeSpan.FromHours(GeneralAverageHoursUtil).ToString(@"dd\.hh\:mm\:ss");
        public int[] Range => CrossCutting.Util.GerarArraySalto(this.PrimeiroSalto, this.QuantidadeSaltos);

        public string[] RangeString
        {
            get
            {
                var rang = CrossCutting.Util.GerarArraySalto(this.PrimeiroSalto, this.QuantidadeSaltos);
                var last = rang.LastOrDefault();
                var ret = new List<string>();
                rang.ToList().ForEach(x =>
                {
                    ret.Add(string.Format("{0}",x));
                });

                ret.RemoveAt(ret.Count-1);
                ret.Add(string.Format("{0}+", last));

                return ret.ToArray();

            }
           
        }

        public object ByLead
        {
            get
            {
                var leeds = from lead in LeadTimes
                    orderby lead.Tempo
                    group lead by Math.Round(lead.LeadBrutoDays, 2, MidpointRounding.AwayFromZero)
                    into temp
                    select new {Tempo = temp.Key, Quantidade = temp.Count(), Percent = (temp.Count() / TotalChamadosDouble).ToString("P") };


                return leeds;

            }
        }
        public object ByRange
        {
            get
            {
                var ranges = CrossCutting.Util.GerarArraySalto(this.PrimeiroSalto, this.QuantidadeSaltos);

                var ret = new List<object>();
                var arrayCount = new List<int>();
                var arrayPercent = new List<double>();

                ranges.ToList().ForEach((r) =>
                    {

                        int Count = LeadTimes.Count(x =>
                            (ranges.ToList().IndexOf(r) < ranges.ToList().Count - 1
                                ? (ranges.ToList().IndexOf(r) > 0
                                    ? (ranges[ranges.ToList().IndexOf(r) - 1] < x.LeadBrutoDays && x.LeadBrutoDays <= r)
                                    : x.LeadBrutoDays <= r
                                    )
                                : x.LeadBrutoDays >= r
                                )
                            );

                        var percent = Math.Round((Count/TotalChamadosDouble)*100, 2, MidpointRounding.ToEven);

                        arrayCount.Add(Count);
                        arrayPercent.Add(percent);

                        ret.Add(new
                        {
                            LeadBrutoDays = r,
                            Count ,
                            Percent = percent
                        });


                    });

                return new {Range = ret, ArrayCount = arrayCount.ToArray(), ArrayPercent = arrayPercent.ToArray()};
            }
        }
        public object ByType
        {
            get
            {
                var leeds = from lead in LeadTimes
                            orderby lead.IdChamado
                            group lead by lead.TipoString
                            into temp
                            select new { Tipo = temp.Key,
                                         Quantidade = temp.Count(),
                                         Dias = TimeSpan.FromHours(Math.Round(temp.Average(x => x.LeadBruto) / 24, 2, MidpointRounding.AwayFromZero)).ToString(@"dd\.hh\:mm\:ss"),
                                         LeadBruto = Math.Round(temp.Average(x => x.LeadBruto), 2, MidpointRounding.AwayFromZero),
                                         Percent = (temp.Count()/TotalChamadosDouble).ToString("P")  };


                return leeds;
            }
        }
        public object ByPriority
        {
            get
            {
                var leeds = from lead in LeadTimes
                            orderby lead.IdChamado
                            group lead by lead.PrioridadeString
                    into temp
                            select new { Prioridade = temp.Key,
                                         Quantidade = temp.Count(),
                                         Dias = TimeSpan.FromHours(Math.Round(temp.Average(x => x.LeadBruto) / 24, 2, MidpointRounding.AwayFromZero)).ToString(@"dd\.hh\:mm\:ss"),
                                         LeadBruto = Math.Round(temp.Average(x => x.LeadBruto), 2, MidpointRounding.AwayFromZero),
                                         Percent = (temp.Count() / TotalChamadosDouble).ToString("P")
                                        };


                return leeds;
            }
        }
        public object ByPriorityType
        {
            get
            {
                var leeds = from lead in LeadTimes
                            orderby lead.IdChamado
                            group lead by new { lead.PrioridadeString , lead.TipoString}
                            into temp
                            select new
                            {
                                PrioridadeTipo = temp.Key,
                                Quantidade = temp.Count(),
                                Dias = TimeSpan.FromHours(Math.Round(temp.Average(x => x.LeadBruto) / 24, 2, MidpointRounding.AwayFromZero)).ToString(@"dd\.hh\:mm\:ss"),
                                LeadBruto = Math.Round(temp.Average(x => x.LeadBruto), 2, MidpointRounding.AwayFromZero),
                                Percent = (temp.Count() / TotalChamadosDouble).ToString("P")
                            };


                return leeds;
            }
        }
        public object ByProjectWeek
        {
            get
            {
                var retorno = from lead in LeadTimes
                    orderby lead.IdChamado
                    group lead by new { lead.TipoString, lead.Tipo}
                    into tipo
                    select new
                    {
                        name = tipo.Key.TipoString,
                        color = string.Format("rgba({0})", RedmineUtil.GetRgbRgbaByTipoChamado(tipo.Key.Tipo, true)),
                        data = tipo.Select(x => new {x= x.CreatedOn.DayOfYear, y= x.LeadBrutoDays, name= x.IdChamado })
                    };
                              
                return retorno;
            }
        }
    }
}
