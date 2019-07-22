using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoardAgil.DashBoardApi.Models
{
    public class ResultDefaultModel
    {
        public ResultDefaultModel(List<String> _ColunasDataTable, bool _renderGrafico = true, bool _renderDados= true, bool _renderCalendario= true)
        {
            ColunasDataTable = _ColunasDataTable?.ToList() ?? new List<string>();
            RenderGrafico = _renderGrafico;
            RenderDados = _renderDados;
            RenderCalendario = _renderCalendario;
        }
        public bool RenderGrafico { get; set; }
        public bool RenderDados { get; set; }
        public bool RenderCalendario { get; set; }
        public List<String> ColunasDataTable { get; set; }
    }
}