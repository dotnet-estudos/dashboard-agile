using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoardAgil.DashBoardApi.Models
{
    public class PerfilMenu
    {
        public int Id { get; set; }
        public int IdPerfil { get; set; }
        public int? IdMenu { get; set; }
        public virtual Menu Menu { get; set; }
        public virtual Perfil Perfil { get; set; }
        public bool Ativo { get; set; }
    }
}