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
        public String Descricao { get; set; }
        public string Url { get; set; }

        public List<Menu> SubMenus { get; set; }
        public Menu MenuPai { get; set; }
        public List<PerfilMenu> PerfilMenu { get; set; }


    }
}