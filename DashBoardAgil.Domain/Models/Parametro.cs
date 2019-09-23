using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DashBoardAgil.Domain.Enums;

namespace DashBoardAgil.Domain.Models
{
    public class Parametro
    {
        [DisplayName("Tempo em: ")]
        public TipoContagem TipoContagemTempo { get; set; }
        [DisplayName("Intervalo: ")]
        public int ExecTime { get; set; }
        [DisplayName("Url Redmine: ")]
        public string RedmineUrl { get; set; }
        [DisplayName("Usuario: ")]
        public string User { get; set; }
        [DisplayName("Senha: ")]
        public string Pass { get; set; }
        [DisplayName("Prazo: : ")]
        public int Prazo { get; set; }

        [DisplayName("Agente Ativo?")]
        public bool FechamentoAutomaticoAtivado { get; set; }
        [DisplayName("Mensagem de Fechamento: ")]
        public string MensagemFechamento { get; set; }
    }
}
