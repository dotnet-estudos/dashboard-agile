using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DashBoardAgil.Domain.Enums;

namespace DashBoardAgil.DashBoardApi.Models
{
    public class ParameterDefault
    {
        //string time, DateTime? dataIni, DateTime? dataFim,  StatusRedmine statusDe, StatusRedmine statusPara, TipoChamadoRedmine? tipoChamado=null, PrioridadeRedmine? prioridade=null, int primeiroSalto=5, int QuantidadeSaltos = 10, bool leadTime =false, bool canceled = true

        public ParameterDefault()
        {
            tipoChamado = null;
            prioridade = null;
            primeiroSalto = 5;
            QuantidadeSaltos = 10;
            leadTime = false;
            issueCanceled = true;
        }
        public string time { get; set; }
        public DateTime? dataIni { get; set; }
        public DateTime? dataFim { get; set; }

        public StatusRedmine? statusDe { get; set; }
        public StatusRedmine? statusPara { get; set; }
        public TipoChamadoRedmine? tipoChamado { get; set; }
        public PrioridadeRedmine? prioridade { get; set; }

        public int primeiroSalto { get; set; }
        public int QuantidadeSaltos { get; set; }
        public bool leadTime { get; set; }
        public bool throughput { get; set; }
        public bool issueCanceled { get; set; }


    }
}