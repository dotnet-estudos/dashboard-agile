using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardAgil.Services.Domain
{
    public class Projeto
    {
        public Projeto()
        {
            Slas = new List<SlaProjeto>();
        }
        [Required]
        public string Id { get; set; }
        [Required]
        public string Identifier { get; set; }
        public bool Ativo { get; set; }
        public int HorasContratadas { get; set; }
        public List<SlaProjeto> Slas { get; set; }
        public Projeto Parent { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Id, Identifier);
        }
    }
}
