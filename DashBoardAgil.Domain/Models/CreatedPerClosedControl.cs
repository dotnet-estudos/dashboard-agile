using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.CrossCutting.Enums;
using DashBoardAgil.Domain.Interfaces.Abstracoes;

namespace DashBoardAgil.Domain.Models
{
    public class CreatedClosedControl
    {
        public CreatedClosedControl(List<CreatedClosed> createdList, List<CreatedClosed> closedList)
        {
            this.CreatedList = createdList;
            this.ClosedList = closedList;

        }
        public List<CreatedClosed> CreatedList { get; set; }
        public List<CreatedClosed> ClosedList { get; set; }

        private List<CreatedClosed> UnionAll
        {
            get
            {
                var union = new List<CreatedClosed>(CreatedList);
                union.AddRange(ClosedList);
                return union;
            }
        }
        private List<DateTime> Dates => this.UnionAll.Where(x => x.Flag == Flag.Created).Select(x => x.CreatedOn.Date).Union(this.UnionAll.Where(x => x.Flag == Flag.Closed).Select(y => y.ClosedOn?.Date ?? new DateTime())).ToList();
        private List<int> WeekList => Dates.Count > 0 ? CrossCutting.Util.GetWeekList(this.Dates.Min(), this.Dates.Max()) : new List<int>();

        public object GetByDay
        {
            get
            {
                var seriesData = new List<Object>();
                var serieClosed = new List<int>();
                var serieCreated = new List<int>();
                var serieChart = new List<object>();
                var dateTimes = this.Dates.ToArray();

                dateTimes.OrderBy(x => x).ToList().ForEach(day =>
                {
                    var totalCrd = this.UnionAll.Count(x => x.Flag == Flag.Created && x.CreatedOn.Date == day);
                    var totalCld = this.UnionAll.Count(x => x.ClosedOn != null && (x.Flag == Flag.Closed && ((DateTime)x.ClosedOn).Date == day));

                    seriesData.Add(new { Dia = day.ToString("yyyy-MM-dd"), Criados = totalCrd, Fechados = totalCld });
                    serieClosed.Add(totalCld);
                    serieCreated.Add(totalCrd);

                });

                serieChart.Add(new { name = "Abertos", data = serieCreated });
                serieChart.Add(new { name = "Fechados", data = serieClosed });

                return new { data = seriesData, chart = new { categoria = dateTimes.Select(x => x.ToString("dd/MM/yyyy")).ToArray(), serie = serieChart } };
            }
        }

        public object GetByWeek
        {
            get
            {
                var seriesData = new List<Object>();
                var serieClosed = new List<int>();
                var serieCreated = new List<int>();
                var serieChart = new List<object>();
                var dateTimes = this.Dates.ToArray();

                WeekList.ForEach(week =>
                {
                    var totalCrd = this.UnionAll.Count(x => x.Flag == Flag.Created && x.CreatedOn.Date.GetWeekOfYear() == week);
                    var totalCld = this.UnionAll.Count(x => x.ClosedOn != null && (x.Flag == Flag.Closed && ((DateTime)x.ClosedOn).Date.GetWeekOfYear() == week));

                    seriesData.Add(new { Semana = week, Criados = totalCrd, Fechados = totalCld });
                    serieClosed.Add(totalCld);
                    serieCreated.Add(totalCrd);

                });

                serieChart.Add(new { name = "Abertos", data = serieCreated });
                serieChart.Add(new { name = "Fechados", data = serieClosed });

                return new { data = seriesData, chart = new { categoria = WeekList.ToArray(), serie = serieChart } };
            }
        }
    }
}
