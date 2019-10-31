using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DashBoardAgil.Domain.Interfaces.Abstracoes;

namespace DashBoardAgil.Domain.Models
{
    public class Rejection : IdentificableIssue
    {
      
        public DateTime? RejectedDate { get; set; }        
        public bool HasIR { get; set; }
        public bool HasRejection => (RejectedDate != null && !HasIR);
    }
}
