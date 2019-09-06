namespace DashBoardAgil.InfraData.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Services.Domain;

    internal sealed class Configuration : DbMigrationsConfiguration<Contexto.Contexto>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Contexto.Contexto context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            SeedProcessos(context);
        }

        private void SeedProcessos(Contexto.Contexto context)
        {
            context.Processos.AddOrUpdate(p => p.ProcessoId,
                new Processo
                {
                    Nome = "Sustentação"
                });

            context.SaveChangesSeed();
        }

        private void SeedFases(Contexto.Contexto context)
        {
        }
    }
}
