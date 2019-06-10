using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DashBoardAgil.Services.Domain;

namespace DashBoardAgil.DashBoardApi.Models
{
    public class Feriado
    {
        public DateTime Data { get; set; }
        public string Nome { get; set; }
        public TipoFeriado Tipo { get; set; }
        public string Descricao { get; set; }
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