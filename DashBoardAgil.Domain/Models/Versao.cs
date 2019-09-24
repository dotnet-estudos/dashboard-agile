using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardAgil.Domain.Models
{
    public class Versao
    {
        public DateTime DataCriacao { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DataPrevista { get; set; }
    }
}
