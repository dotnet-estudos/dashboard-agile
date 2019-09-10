using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DashBoardAgil.Services.Enums;

namespace DashBoardAgil.Services.Domain
{
    public class ConfigRedmine
    {
        public ConfigRedmine()
        {
            Parametro = new Parametro();
            AllowList = new List<Projeto>();
            DenyList = new List<Projeto>();
            UsuariosMaster = new List<string>();
            PerfilCliente = new List<string>();
        }

        public Parametro Parametro { get; set; }

        public List<Projeto> AllowList { get; set; }

        public List<Projeto> DenyList { get; set; }

        public List<string> UsuariosMaster { get; set; }
        public List<string> PerfilCliente { get; set; }

        [field: NonSerialized()]
        public virtual List<Projeto> ProjetosAtivos()
        {
             return AllowList.Where(x => x.Ativo && DenyList.All(y => y.Id != x.Id)).ToList(); 
        }

        public Projeto GetProjetoById(string id)
        {

            return ProjetosAtivos().FirstOrDefault(x => x.Id.Equals(id));

        }
    }
    
}