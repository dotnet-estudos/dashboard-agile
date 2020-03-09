using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using DashBoardAgil.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web;

namespace DashBoardAgil.InfraData.Contexto
{
    public class Contexto : DbContext
    {
        static Contexto()
        {
            //Database.SetInitializer<AgileGameContexto>(new BdInitializer<AgileGameContexto>());
            Database.SetInitializer<Contexto>(null);

        }

        public Contexto() : base("name = bd_dashboard_agil")
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 180;
#if DEBUG
            Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
#endif
        }

        public Contexto(bool disabledProxyLazyLoad)
            : base("name = bd_dashboard_agil")
        {
#if DEBUG
            Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
#endif
            Configuration.LazyLoadingEnabled = disabledProxyLazyLoad;
            Configuration.ProxyCreationEnabled = disabledProxyLazyLoad;
        }

        public virtual DbSet<Processo> Processos { get; set; }
        public virtual DbSet<Fase> Fases { get; set; }
        public virtual DbSet<SubFase> SubFases { get; set; }
        public virtual DbSet<PFS> PFS { get; set; }
        public virtual DbSet<TipoChamado> TipoChamado { get; set; }
        public virtual DbSet<ProcessoTipo> ProcessoTipo  { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<Projeto> Projetos { get; set; }
        public virtual DbSet<CustonFieldProjeto> CustonFieldProjetos { get; set; }


        private string GetPrimaryKeyValue(object entry)
        {
            var keyNames = GetKeyNames(entry);
            var sb = new StringBuilder();
            sb.Append("{");
            for (var i = 0; i < keyNames.Length; i++)
            {
                sb.Append(keyNames[i] + " : " + GetKeyValue(entry, keyNames, i));
                sb.Append(i != keyNames.Length - 1 ? "," : "");
            }
            sb.Append("}");
            return sb.ToString();
        }

        private static object GetKeyValue(object entry, string[] keyNames, int i)
        {
            if (!(entry is DbEntityEntry))
                return entry.GetType().GetProperty(keyNames[i]).GetValue(entry);

            var x = (DbEntityEntry)entry;

            if (x.State == EntityState.Added)
                return x.CurrentValues[keyNames[i]].ToString();

            return x.OriginalValues[keyNames[i]].ToString();
        }

        private string[] GetKeyNames(object entity)
        {
            EntitySetBase setBase;
            var manager = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager;

            if (entity is DbEntityEntry)
                setBase = manager.GetObjectStateEntry(((DbEntityEntry)entity).Entity).EntitySet;
            else
                setBase = manager.GetObjectStateEntry(entity).EntitySet;

            var keyNames = setBase.ElementType.KeyMembers.Select(k => k.Name).ToArray();
            return keyNames;
        }
        public int SaveChangesSeed()
        {
            return base.SaveChanges();
        }

        public override int SaveChanges()
        {
            ProcessaDataCadastro();

            var added = new List<KeyValuePair<object, Log>>();

            //GerarLog(added);

            var result = base.SaveChanges();
            UpdateInsertedKey(added);
            return result;
        }

        private void ProcessaDataCadastro()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
            {

                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataCadastro").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataCadastro").IsModified = false;
                }

            }
        }

        private void GerarLog(List<KeyValuePair<object, Log>> added)
        {
            var entries = ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted);

            foreach (var dbEntityEntry in entries)
            {
                var entityName = dbEntityEntry.Entity.GetType().Name;
                var primaryKey = GetPrimaryKeyValue(dbEntityEntry);


                switch (dbEntityEntry.State)
                {
                    case EntityState.Added:
                    case EntityState.Deleted:
                        var log = new Log
                        {
                            Tabela = entityName,
                            Acao = dbEntityEntry.State.ToString(),
                            NomeUsuario = HttpContext.Current.User.Identity.Name,
                            Chaves = primaryKey,
                            NomeColuna = "*",
                            Propriedade = "*",
                            ValorOriginal = String.Empty,
                            ValorAtual = String.Empty,
                            DataOcorrencia = DateTime.Now
                        };

                        Set<Log>().Add(log);

                        if (dbEntityEntry.State != EntityState.Added)
                            continue;

                        var kv = new KeyValuePair<object, Log>(dbEntityEntry.Entity, log);
                        added.Add(kv);
                        break;
                    case EntityState.Modified:
                        GeraLogUpdate(dbEntityEntry, entityName, primaryKey);
                        break;
                }
            }
        }

        private void UpdateInsertedKey(IEnumerable<KeyValuePair<object, Log>> added)
        {
            foreach (var keyvalues in added)
            {
                var log = Set<Log>().Find(keyvalues.Value.LogId);

                if (log == null)
                    continue;

                log.Chaves = GetPrimaryKeyValue(keyvalues.Key);
                base.SaveChanges();
            }
        }

        private void GeraLogUpdate(DbEntityEntry dbEntityEntry, string entityName, string primaryKey)
        {
            foreach (var prop in dbEntityEntry.OriginalValues.PropertyNames)
            {
                var originalValue = (dbEntityEntry.OriginalValues[prop] != null
                    ? dbEntityEntry.OriginalValues[prop].ToString()
                    : "");
                var currentValue = (dbEntityEntry.CurrentValues[prop] != null
                    ? dbEntityEntry.CurrentValues[prop].ToString()
                    : "");

                if (originalValue == currentValue)
                    continue;

                var log = new Log
                {
                    Tabela = entityName,
                    Acao = dbEntityEntry.State.ToString(),
                    NomeUsuario = HttpContext.Current.User.Identity.Name,
                    Chaves = primaryKey,
                    NomeColuna = prop,
                    Propriedade = prop,
                    ValorOriginal = originalValue,
                    ValorAtual = currentValue,
                    DataOcorrencia = DateTime.Now
                };
                Set<Log>().Add(log);
            }
        }
        
        public bool AmbienteProducao()
        {
            //return Database.Connection.ConnectionString.Contains("Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DashBoardAgil;Integrated Security=True");
            return false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Properties()
               .Where(p => p.Name == p.ReflectedType.Name + "Id")
               .Configure(p => p.IsKey());

            modelBuilder.Properties<string>()
                .Configure(p => p.HasColumnType("varchar"));

            modelBuilder.Properties<string>()
                .Configure(p => p.HasMaxLength(100));

        }
        
    }
}
