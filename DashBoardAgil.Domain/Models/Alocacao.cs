using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redmine.Net.Api.Types;

namespace DashBoardAgil.Domain.Models
{
    public class Alocacao :  TimeEntry
    {
        public Alocacao(Projeto time)
        {
            this.Time = time;
        }
        public Projeto Time { get; set; }

    }
}
