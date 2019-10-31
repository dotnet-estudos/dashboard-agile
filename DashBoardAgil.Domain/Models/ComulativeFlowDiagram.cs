using System;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.Domain.Interfaces.Abstracoes;

namespace DashBoardAgil.Domain.Models
{
    public class ComulativeFlowDiagram : IdentificableIssue
    {

        public Fase Fase { get; set; }
        public SubFase SubFase { get; set; }
        public DateTime DataReferencia { get; set; }
        public int Semana { get; set; }
        public int Ano => DataReferencia.Year;
        public int Mes => DataReferencia.Month;
        public int Dia => DataReferencia.Day;

    }
}