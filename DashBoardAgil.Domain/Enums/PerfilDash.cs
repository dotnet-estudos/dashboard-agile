using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardAgil.Domain.Enums
{
    public enum PerfilDash
    {
        [Display(Name = "Administrador")]
        Administrador = 0,
        [Display(Name = "Usuário")]
        Usuario = 1,
        [Display(Name = "Cliente")]
        Cliente = 2,


    }
}
