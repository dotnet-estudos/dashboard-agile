using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardAgil.Services.Enums
{
    public enum PrioridadeRedmine
    {
        [Display(Name = "Programada")]
        Programada = 3,
        [Display(Name = "Padrão")]
        Padrao = 4,
        [Display(Name = "Alto")]
        Alto = 5,
        [Display(Name = "Urgente")]
        Urgente = 7,
    }
}
