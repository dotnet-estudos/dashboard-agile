using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoardAgil.DashBoardApi.Models
{
    public class ResultDefaultModel
    {
        public ResultDefaultModel(List<String> _ColunasDataTable, Dictionary<string, string> _renderDictionary = null,  bool _renderGrafico = true, bool _renderDados= true, bool _renderCalendario= true)
        {
            ColunasDataTable = _ColunasDataTable?.ToList() ?? new List<string>();
            RenderGrafico = _renderGrafico;
            RenderDados = _renderDados;
            RenderCalendario = _renderCalendario;
            RenderDivs = _renderDictionary?? new Dictionary<string, string>();
        }
        public bool RenderGrafico { get; set; }
        public bool RenderDados { get; set; }
        public bool RenderCalendario { get; set; }
        public List<String> ColunasDataTable { get; set; }
        public Dictionary<string, string> RenderDivs { get; set; }
    }
}