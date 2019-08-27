using DashBoardAgil.InfraData.Enum;

namespace DashBoardAgil.InfraData.Models
{
    public class SubFase
    {
        public int Id { get; set; }
        public int IdFase { get; set; }
        public int Ordem { get; set; }
        public string Descricao { get; set; }
        public StatusRedmine StatusRedmine { get; set; }
        public SituacaoSubFase Situacao { get; set; }
        public virtual Fase Fase { get; set; }
    }
}