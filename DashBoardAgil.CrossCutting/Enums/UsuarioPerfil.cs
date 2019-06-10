using System.ComponentModel.DataAnnotations;

namespace DashBoardAgil.CrossCutting.Enums
{
    public enum PerfilUsuario
    {

        /// <summary>
        /// Administrador
        /// </summary>
        [Display(Name = "Administrador")]
        Administrador = 1,

        /// <summary>
        /// Auditor
        /// </summary>
        [Display(Name = "Auditor")]
        Auditor = 2,

        /// <summary>
        /// Comissão
        /// </summary>
        [Display(Name = "Comissão")]
        Comissao = 3,

        /// <summary>
        /// Gestor
        /// </summary>
        [Display(Name = "Gestor")]
        Gestor = 4,

        /// <summary>
        /// Habilitador
        /// </summary>
        [Display(Name = "Habilitador")]
        Habilitador = 5
    }
}