using System;
using System.ComponentModel.DataAnnotations;

namespace DashBoardAgil.CrossCutting.Object
{
    public class Feriado
    {
        public DateTime date { get; set; }
        public string name { get; set; }
        public TipoFeriado type_code { get; set; }
        public string description { get; set; }
    }

    public enum TipoFeriado
    {
        [Display(Name = "Feriado Nacional")]
        FeriadoNacional = 1,
        [Display(Name = "Feriado Estadual")]
        FeriadoEstadual = 2,
        [Display(Name = "Feriado Municipal")]
        FeriadoMunicipal = 3,
        [Display(Name = "Facultativo")]
        Facultativo = 4,
        [Display(Name = "Dia Convencional")]
        DiaConvencional = 9,
        
    }
}