using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardAgil.Services.Domain
{
    public class CalendarioChamado
    {
        public DateTime Data { get; set; }

        public string DataString
        {
            get { return Data.ToString("yyyy-MM-dd"); }
        }

        public int Total { get; set; }

        public int TotalOut { get; set; }

        public String[] ChamadosArray { get; set; }
    }
}
