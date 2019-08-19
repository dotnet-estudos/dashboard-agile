using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DashBoardAgil.Services.Interfaces.Abstracoes;

namespace DashBoardAgil.Services.Domain
{
    public class CreatedPerClosedControl
    {
        public IList<IdentificableIssue> CreatedList { get; set; }
        public IList<IdentificableIssue> ClosedList { get; set; }

        public Object GetByDay
        {
            get
            {
               // var createdList = (from rejc in CreatedList.ToList()
               //               orderby rejc.CreatedOn
               //               group rejc by ((DateTime)rejc.CreatedOn).Date
               //                into grp
               //               select new { Dia = grp.Key.ToString("d"), total = grp.Count(), type = "created" }).ToList();

               // var closedList = (from rejc in ClosedList.ToList()
               //                   orderby rejc.ClosedOn
               //                   group rejc by ((DateTime)rejc.ClosedOn).Date
               //                into grp
               //                   select new { Dia = grp.Key.ToString("d"), total = grp.Count(), type ="closed" }).ToList();
               // var union = new List<object>()
               // {
               //     closedList,
               //     createdList
               // };

               //var serie =  (from rejc in union
               //              orderby rejc["dia"]
               //  group rejc by ((DateTime)rejc.Dia).Date
               //                into grp
               //  select new { Dia = grp.Key.ToString("d"), total = grp.Count(), type = "created" }).ToList();

                return null;
            }
        }
    }
}
