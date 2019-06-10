using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardAgil.Services.Domain
{
    public class Menu
    {
       
        public int Id { get; set; }
        public int? IdPai { get; set; }
        public string Nome { get; set; }
        public string Url { get; set; }
        public virtual ICollection<Menu> SubMenus { get; set; }
        public virtual Menu MenuPai { get; set; }

    }
}
