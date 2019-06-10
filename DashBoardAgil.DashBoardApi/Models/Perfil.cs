using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoardAgil.DashBoardApi.Models
{
    public class Perfil
    {
        public Perfil()
        {
            this.PerfilMenus = new List<PerfilMenu>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public virtual ICollection<PerfilMenu> PerfilMenus { get; set; }
    }
}