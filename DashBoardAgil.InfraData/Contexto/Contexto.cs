using System.Data.Entity;
using DashBoardAgil.InfraData.Models;

namespace DashBoardAgil.InfraData.Contexto
{
    public class Contexto : DbContext
    {
        public Contexto() : base("name = bd_dashboard_agil") { }
        public virtual DbSet<Processo> Processos { get; set; }
        public virtual DbSet<Fase> Fases { get; set; }
        public virtual DbSet<SubFase> SubFases { get; set; }
    }
}
