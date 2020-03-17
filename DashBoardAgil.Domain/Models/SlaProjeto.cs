using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DashBoardAgil.CrossCutting.Enums;

namespace DashBoardAgil.Domain.Models
{
    public class SlaProjeto
    {
        public int IdSla { get; set; }
        public int Tempo { get; set; }
        public TipoContagem TipoContagemTempo { get; set; } // Padrão para fazer Contagem, estudar usar o Slq sempre em Minutos para utilizar de forma dinamica convertendo os valores

        public bool TempoUltil { get; set; }
        public Sla Sla { get; set; }

        public int GetTempoSla()
        {
            switch (TipoContagemTempo)
            {
                case TipoContagem.Hora:
                    {

                    }
                    break;
                case TipoContagem.Minuto:
                    {

                    }
                    break;
                case TipoContagem.Segundo:
                    {

                    }
                    break;
            }

            return 0;
        }

    }
    
}
