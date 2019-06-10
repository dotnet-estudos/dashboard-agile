using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardAgil.Services.Enums
{
    public enum TipoChamadoRedmine
    {
        [Display(Name = "Evolutiva")]
        Evolutiva = 83,
        [Display(Name = "Adaptativa")]
        Adaptativa = 86,
        [Display(Name = "Consulta")]
        Consulta = 87,
        [Display(Name = "Corretiva")]
        Corretiva = 85,
    }
}
