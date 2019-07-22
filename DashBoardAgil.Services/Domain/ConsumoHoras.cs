using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DashBoardAgil.Services.Interfaces.Abstracoes;

namespace DashBoardAgil.Services.Domain
{
    public class ConsumoHoras
    {
        public int Id { get; set; }
        public double  Horas { get; set; }
        public IdentificableName Usuario { get; set; }
        public IdentificableName Projeto { get; set; }
        public IdentificableName Team { get; set; }
        public IdentificableName Chamado { get; set; }

        public DateTime DataAlocacao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public string Comentario { get; set; }
    }
}
