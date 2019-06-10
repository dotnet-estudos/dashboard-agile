using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoardAgil.DashBoardApi.Models
{
    public class SumaryChamdo
    {
        public string FaIcon { get; set; }
        public string PaneColor { get; set; }
        public int Ordem { get; set; }
        public string Mensagem { get; set; }
        public int Quantidade { get; set; }
        public string  Fase { get; set; }
        public string  UrlRedmine { get; set; }

    }
}