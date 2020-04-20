using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardAgil.Domain.Models
{
    public class CustonFieldProjeto
    {
        [Key]
        public int CFId { get; set; }
        public string Campo { get; set; }
        public string Valor { get; set; }
        public int ProjetoId { get; set; }
        public virtual Projeto Projeto { get; set; }

    }
}
