using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardAgil.Domain.Models
{
    public class Projeto
    {
        /*public Projeto()
        {
            CustonFields = new List<CustonFieldProjeto>();
            Slas = new List<SlaProjeto>();
        }*/
        [Required]
        [Key]
        public string Id { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        public string IdProjeto { get; set; }
        [Required]
        public string Identifier { get; set; }
        public bool Ativo { get; set; }
        public int HorasContratadas { get; set; }
        public virtual List<CustonFieldProjeto> CustonFields { get; set; }
        /*public List<SlaProjeto> Slas { get; set; }
        public Projeto Parent { get; set; }*/
        public int QuantidadeDesenvolvedores { get; set; }

        public int ProcessoId { get; set; }
        public virtual Processo Processo { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1} ({2}-{3})", IdProjeto, Descricao, Id, Identifier);
        }
    }
}
