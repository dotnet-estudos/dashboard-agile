using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DashBoardAgil.Domain.Models
{
    public class CustonFieldProjeto
    {
        [Key]
        public int CFId { get; set; }
        public string Campo { get; set; }
        public string Valor { get; set; }
        public string ProjetoId { get; set; }
        public virtual Projeto Projeto { get; set; }
    }
}
