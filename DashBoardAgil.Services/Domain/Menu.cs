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
        public string Url { get; set; }
        public string Icone { get; set; }
        public string Titulo { get; set; }
        public string IdUrl { get; set; }
        public string Permissoes { get; set; }
        public bool Visivel { get; set; }
        public List<Menu> SubMenus { get; set; }
        //public virtual ICollection<Menu> SubMenus { get; set; }
        //public virtual Menu MenuPai { get; set; }

        public List<Menu> MontaMenuPorPerfil(List<Menu> ItensMenu, string _perfil)
        {
            List<Menu> ItensMenuAux = ItensMenu;
            //Adicionar os menus principais
            int contador = ItensMenuAux.Count;
            int contRemocao = 0;
            for (int y = 0; y < contador; y++)
            {
                Menu item = ItensMenuAux[contRemocao];
                if (!item.Permissoes.Contains(_perfil))
                {
                    ItensMenuAux.Remove(item);
                } else
                {
                    contRemocao++;
                    if (item.SubMenus.Count > 0){
                        this.MontaMenuPorPerfil(item.SubMenus, _perfil);
                    }
                }
            }
            return ItensMenuAux;
        }

    }

    
}
