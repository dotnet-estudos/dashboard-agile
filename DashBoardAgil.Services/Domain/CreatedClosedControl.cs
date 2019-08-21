using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using DashBoardAgil.Services.Enums;
using DashBoardAgil.Services.Interfaces.Abstracoes;

namespace DashBoardAgil.Services.Domain
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

        public Object GetByDay
        {
            get
            {
                var union = new List<CreatedClosed>(CreatedList);
                union.AddRange(ClosedList);

                var listDays = union.Where(x => x.Flag == Flag.Created).Select(x => x.CreatedOn.Date).Union(union.Where(x => x.Flag == Flag.Closed).Select(y => y.ClosedOn?.Date ?? new DateTime()));

                var serie = new List<Object>();

                listDays.OrderBy(x=>x).ToList().ForEach(day=>
                {
                    serie.Add(new { Dia = day.ToString("yyyy-MM-dd"), Criados= union.Count(x=>x.Flag == Flag.Created && x.CreatedOn.Date == day), Fechados = union.Count(x => x.ClosedOn != null && (x.Flag == Flag.Closed && ((DateTime)x.ClosedOn).Date == day)) });
                });
                
                return serie;
            }
        }
    }
}
