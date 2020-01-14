using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.Domain.Enums;
using DashBoardAgil.Domain.Interfaces.Abstracoes;
using DashBoardAgil.Domain.Util;
using Redmine.Net.Api.Types;
using Newtonsoft.Json;

namespace DashBoardAgil.Domain.Models
{
    public class LeadTimeControl
    {
        public LeadTimeControl(int primeiroSalto = 5, int quantidadeSalto = 10)
        {
            this.LeadTimes = new List<LeadTime>();
            this.PrimeiroSalto = primeiroSalto;
            this.QuantidadeSaltos = quantidadeSalto;

        }

        private int PrimeiroSalto { get; }
        private int QuantidadeSaltos { get; }
        public List<Issue> UniversoChamados { get; set; }
        public List<LeadTime> LeadTimes { get; set; }
        public int TotalChamados => LeadTimes.Count();
        private double TotalChamadosDouble => Double.Parse(this.TotalChamados.ToString());
        public double GeneralAverageHours => LeadTimes.Any()? Math.Round(LeadTimes.Average(x => x.LeadBruto), 2, MidpointRounding.AwayFromZero) : 0.0;
        public double GeneralMedianaHours => LeadTimes.Any() ? Math.Round(LeadTimes.Select(x => new { x.LeadBruto }).OrderBy(x => x.LeadBruto).Skip(LeadTimes.Count() / 2).First().LeadBruto, 2, MidpointRounding.AwayFromZero): 0.0;
        public double GeneralAverageDays => Math.Round((GeneralAverageHours / 24), 2, MidpointRounding.AwayFromZero);
        public double GeneralMedianaDays => Math.Round((GeneralMedianaHours / 24), 2, MidpointRounding.AwayFromZero);
        public string GeneralAverageDaysString => TimeSpan.FromHours(GeneralAverageHours).ToString(@"dd\.hh\:mm\:ss");
        public string GeneralMedianaDaysString => TimeSpan.FromHours(GeneralMedianaHours).ToString(@"dd\.hh\:mm\:ss");
        public double GeneralAverageHoursUtil => LeadTimes.Any() ? Math.Round(LeadTimes.Average(x => x.LeadUtil), 2, MidpointRounding.AwayFromZero) : 0.0;
        public double GeneralAverageDaysUtil => Math.Round(GeneralAverageHoursUtil / 7.2, 2, MidpointRounding.AwayFromZero);
        public string GeneralAverageDaysUtilString => TimeSpan.FromHours(GeneralAverageHoursUtil).ToString(@"dd\.hh\:mm\:ss");
        public double GeneralMedianaHoursUtil => LeadTimes.Any() ? Math.Round(LeadTimes.Select(x => new { x.LeadUtil }).OrderBy(x => x.LeadUtil).Skip(LeadTimes.Count() / 2).First().LeadUtil, 2, MidpointRounding.AwayFromZero) : 0.0;
        public double GeneralMedianaDaysUtil => LeadTimes.Any() ? Math.Round(GeneralMedianaHoursUtil / 7.2, 2, MidpointRounding.AwayFromZero) : 0.0;
        public string GeneralMedianaDaysUtilString => TimeSpan.FromHours(GeneralMedianaHoursUtil).ToString(@"dd\.hh\:mm\:ss");
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
                    ret.Add(string.Format("{0}", x));
                });

                ret.RemoveAt(ret.Count - 1);
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
                            select new { Tempo = temp.Key, Quantidade = temp.Count(), Percent = (temp.Count() / TotalChamadosDouble).ToString("P") };


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

                        var percent = Math.Round((Count / TotalChamadosDouble) * 100, 2, MidpointRounding.ToEven);

                        arrayCount.Add(Count);
                        arrayPercent.Add(percent);

                        ret.Add(new
                        {
                            LeadBrutoDays = r,
                            Count,
                            Percent = percent
                        });


                    });

                return new { Range = ret, ArrayCount = arrayCount.ToArray(), ArrayPercent = arrayPercent.ToArray() };
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
                            select new
                            {
                                Tipo = temp.Key,
                                Quantidade = temp.Count(),
                                Dias = TimeSpan.FromHours(Math.Round(temp.Average(x => x.LeadBruto) / 24, 2, MidpointRounding.AwayFromZero)).ToString(@"dd\.hh\:mm\:ss"),
                                LeadBruto = Math.Round(temp.Average(x => x.LeadBruto), 2, MidpointRounding.AwayFromZero),
                                Percent = (temp.Count() / TotalChamadosDouble).ToString("P")
                            };


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
                            select new
                            {
                                Prioridade = temp.Key,
                                Quantidade = temp.Count(),
                                Dias = TimeSpan.FromHours(Math.Round(temp.Average(x => x.LeadBruto) , 2, MidpointRounding.AwayFromZero)).ToString(@"dd\.hh\:mm\:ss"),
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
                            group lead by new { lead.PrioridadeString, lead.TipoString }
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
                              group lead by new { lead.TipoString, lead.Tipo }
                    into tipo
                              select new
                              {
                                  name = tipo.Key.TipoString,
                                  color = string.Format("rgba({0})", DomainUtil.GetRgbRgbaByTipoChamado(tipo.Key.Tipo, true)),
                                  data = tipo.Select(x => new { x = x.CreatedOn.DayOfYear, y = x.LeadBrutoDays, name = x.IdChamado })
                              };

                return retorno;
            }
        }
        public object ByLeadAnalyze
        {
            get
            {

                try
                {
                    var data = new List<object>
                    {
                        new {name = "Min", y = this.LeadTimes.Where(x => x.LeadBrutoDays > 0).Min(x => x.LeadBrutoDays), drilldown= ""},
                        new {name = "Média", y = this.GeneralAverageDays, drilldown= ""},
                         new {name = "Mediana", y = this.GeneralMedianaDays, drilldown= ""},
                        new {name = "Percentil 75", y =  Math.Round(LeadTimes.Select(x => new { x.LeadBrutoDays }).OrderBy(x => x.LeadBrutoDays).Skip(Convert.ToInt32((LeadTimes.Count() * 0.75))).First().LeadBrutoDays, 2, MidpointRounding.AwayFromZero), drilldown = ""},
                        new {name = "Percentil 90", y =  Math.Round(LeadTimes.Select(x => new { x.LeadBrutoDays }).OrderBy(x => x.LeadBrutoDays).Skip(Convert.ToInt32((LeadTimes.Count() * 0.9))).First().LeadBrutoDays, 2, MidpointRounding.AwayFromZero), drilldown = ""},
                        new {name = "Max", y = this.LeadTimes.Max(x => x.LeadBrutoDays), drilldown= ""},


                    };

                    var serie = new
                    {
                        name = "Lead Time",
                        colorByPoint = true,
                        data = data.ToArray()
                    };

                    return new { series = new List<object> { serie }, drilldown = new { serie = new List<object>() } };
                }
                catch (Exception ex)
                {
                    Debugar.WriteLine(ex.Message, "LeadTimeControl.ByLeadAnalyze");
                    return null;
                }


            }
        }
        public object ByProcess
        {
            get
            {
                if (UniversoChamados.Any(x => x.Journals?.Any() ?? false))
                {

                    //var processo =
                    //    JsonConvert.DeserializeObject<Processo>(
                    //        Global.Instance.LerArquivo(String.Format(@"{0}\{1}",
                    //            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Json"), "processo.json")));

                    //var dicClicleTime = new Dictionary<int, double>();
                    //// var dicClicleFase = new Dictionary<int, double>();

                    //var subfases = new List<Fase>();
                    //processo.Fluxo.Where(fluxo => fluxo.IdFase == null && fluxo.SubFases.Any(x => !x.StatusFechamento))
                    //    .ToList().ForEach((flux) =>
                    //    {
                    //        flux.SubFases.Where(x => !x.StatusFechamento).ToList().ForEach(x => subfases.Add(x));
                    //    });

                    //subfases.ToList().ForEach((flux) =>
                    //{
                    //    var datesFase = UniversoChamados.Select(uni => new
                    //    {
                    //        chamado = uni.Id,
                    //        fase = flux,
                    //        dateIn = RedmineUtil.GetDateByStatus(uni, (StatusRedmine)flux.EStatusRedmine),
                    //        dateFim = RedmineUtil.GetDateByStatus(uni, (StatusRedmine)flux.EStatusRedmine, true)
                    //    });
                    //    var resultDic = datesFase.Where(x => x.dateIn != null).Select(dic =>((DateTime)(dic.dateFim ?? DateTime.Now)).Subtract((DateTime)dic.dateIn).TotalHours);
                    //    dicClicleTime.Add(flux.Id, resultDic.Any() ? Math.Round(resultDic.Average()/24, 2, MidpointRounding.AwayFromZero) : 0);
                    //});

                    //var dicClicleFase = processo.Fluxo.Where(fluxo => fluxo.FasePai == null && fluxo.SubFases.Any(x => !x.StatusFechamento))
                    //                                  .Select((flux) =>new{
                    //                                                        name = flux.Descricao,
                    //                                                        y =  dicClicleTime.Any(x => flux.SubFases.Any(y => y.Id == x.Key))? 
                    //                                                                 dicClicleTime.Where(x => flux.SubFases.Any(y => y.Id == x.Key)).Average(x => x.Value):
                    //                                                                 0,
                    //                                                        drilldown = flux.Id.ToString()
                    //                                                       }).ToList();

                    //var drilldown = processo.Fluxo.Where(
                    //    fluxo => fluxo.FasePai == null && fluxo.SubFases.Any(x => !x.StatusFechamento)).ToList()
                    //    .Select(flux =>new {
                               
                    //                name = flux.Descricao,
                    //                id = flux.Id.ToString(),
                    //                data = dicClicleTime.Where(x => flux.SubFases.Any(y => y.Id == x.Key)).Select(x => new  { name= subfases.FirstOrDefault(s => s.Id == x.Key).Descricao, y= x.Value, value = x.Value }).ToArray()
                    //        });

                    //var retorno = new
                    //{
                    //    series = new List<object>() { new { name = "Fases", colorByPoint = true, data = dicClicleFase } }.ToArray(),
                    //    drilldown = new { series = drilldown }
                    //};

                    //return retorno;
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
