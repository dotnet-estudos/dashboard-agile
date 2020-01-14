
using System.ComponentModel.DataAnnotations;

namespace DashBoardAgil.Domain.Enums
{
	public enum StatusRedmine
	{
        [Display(Name = "Novo")]
        Novo = 1,
        [Display(Name = "Aberto")]
        Aberto = 2,
        [Display(Name = "Resolvido")]
        Resolvido = 3,
        [Display(Name = "Andamento")]
        Andamento = 4,
        [Display(Name = "Fechado")]
        Fechado = 5,
        [Display(Name = "Cancelado")]
        Cancelado = 6,
        [Display(Name = "Aguardando Informação")]
        AguardandoInformacao = 22,
        [Display(Name = "Homologação")]
        Homologacao = 28,
        [Display(Name = "Validado")]
        Validado = 20,
        [Display(Name = "Rejeitado")]
        Rejeitado = 41,
        [Display(Name = "Avaliar Produção")]
        AvaliarProducao = 56,

        [Display(Name = "Desenvolvimento: Andamento")]
        DesenvolvimentoAndamento = 114,
        [Display(Name = "Desenvolvimento: Resolvido")]
        DesenvolvimentoResolvido = 115,
        [Display(Name = "Homologação: Andamento")]
        HomologacaoAndamento = 116,
        [Display(Name = "Homologação: Validado")]
        HomologacaoValidado = 117,
        [Display(Name = "Produção: Avaliar Produção")]
        ProducaoAvaliar = 118,
        [Display(Name = "Backlog: Aberto")]
        BacklogAberto = 119,
        [Display(Name = "Backlog: Novo")]
        BacklogNovo = 120,
        [Display(Name = "Pre-Hml: A Fazer")]
        PreHmlAFazer = 131,
        [Display(Name = "Pre-Hml: Fazendo")]
        PreHmlFazendo = 132,
        [Display(Name = "Pre-Hml: Feito")]
        PreHmlFeito = 133,
        [Display(Name = "Demanda")]
        Demanda = 157,
        /*Status do Fiplan*/
        [Display(Name = "Backlog: A fazer")]
        BacklogFazer = 147,
        [Display(Name = "Backlog - Em andamento")]
        BacklogAndamento = 148,
        [Display(Name = "Desenvolvimento: A fazer")]
        DesenvolvimentoFazer = 156,
        [Display(Name = "Teste: A fazer")]
        TesteFazer = 149,
        [Display(Name = "Teste: Em andamento")]
        TesteAndamento = 150,
        [Display(Name = "Homologação: A fazer")]
        HomologacaoFazer = 142,
        [Display(Name = "Homologação: Validando")]
        HomologacaoValidando = 152,
        [Display(Name = "Pré-Produção: Pendente")]
        PreProducaoPendente = 153,
        [Display(Name = "Pré-Produção: A fazer")]
        PreProducaoFazer = 144,
        [Display(Name = "Pré-Produção: Andamento")]
        PreProducaoAndamento = 145,
        [Display(Name = "Pré-Produção: Pronto")]
        PreProducaoPronto = 146,
        [Display(Name = "Produção")]
        Producao = 50

    }
}
