using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardAgil.Domain.Enums
{
    public enum TipoChamadoRedmine
    {
        #region CSOL-1
        [Display(Name = "Evolutiva")]
        Evolutiva = 83,
        [Display(Name = "Adaptativa")]
        Adaptativa = 86,
        [Display(Name = "Consulta")]
        Consulta = 87,
        [Display(Name = "Corretiva")]
        Corretiva = 85,
        #endregion

        #region FIPLAN
        [Display(Name = "Fiplan: Evolutiva")]
        FiplanEvolutiva = 96,
        [Display(Name = "Fiplan: Adaptativa")]
        FiplanAdaptativa = 93,
        [Display(Name = "Fiplan:  Consulta")]
        FiplanConsulta = 100,
        [Display(Name = "Fiplan:  Corretiva")]
        FiplanCorretiva = 99,
        #endregion

        #region SAC Digital
        [Display(Name = "História de Usuário")]
        SacHistoriaUsuario = 71,
        [Display(Name = "Item de Melhoria")]
        SacItemMelhoria = 73,
        [Display(Name = "Defeito")]
        SacDefeito = 74,
        [Display(Name = "SD:Tarefa")]
        SacTarefa = 80,
        #endregion

    }
}
