using System.Collections.Generic;

namespace DashBoardAgil.Services.Domain
{
    public class SlaControl : ChamadoControl
    {
        public SlaControl()
        {
            ChamadoSemafaro = new List<Chamado>();
        }

        public Sla Sla { get; set; }

        public Projeto Projeto { get; set; }
        
    }

  
}