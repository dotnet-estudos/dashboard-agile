using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoardAgil.DashBoardApi.Models
{
    public class Menu
    {
        public int Id { get; set; }
        public int? IdPai { get; set; }
        public string Url { get; set; }
        public string Icone { get; set; }
        public string Titulo { get; set; }
        public string IdUrl { get; set; }
        public string Permissoes { get; set; }
        public bool Visivel { get; set; }
        public List<Menu> SubMenus { get; set; }
        //public List<PerfilMenu> PerfilMenu { get; set; }


    }
}