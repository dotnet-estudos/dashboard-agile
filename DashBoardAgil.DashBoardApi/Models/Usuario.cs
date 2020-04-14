using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DashBoardAgil.CrossCutting.Enums;
using DashBoardAgil.Services;
using DashBoardAgil.Domain.Models;
using Redmine.Net.Api.Types;

namespace DashBoardAgil.DashBoardApi.Models
{
    public class Usuario : User
    {

        private PerfilDash _PerfilDash { get; set; }
        public PerfilDash Perfil
        {
            get
            {

                if (this.Memberships != null)
                {
                    GlobalConfig.Instance.CarregarConfig();
                    if (GlobalConfig.Instance.ConfiguracaoRedmine.UsuariosMaster.Contains(this.Login))
                    {
                        return PerfilDash.Administrador;
                    }
                    else
                    {
                        var list = new List<string>();
                        this.Memberships.ForEach(s =>
                        {
                            var rols = s.Roles.Select(x => x.Name);
                            list.AddRange(rols);
                        });
                        list = list.Distinct().ToList();
                        if (GlobalConfig.Instance.ConfiguracaoRedmine.PerfilCliente.Any(x => list.Any(y => y.ToLowerInvariant().Contains(x.ToLowerInvariant()))))
                        {
                            return PerfilDash.Cliente;
                        }
                    }
                    return PerfilDash.Usuario;
                }
                else
                {
                    return PerfilDash.Cliente;
                }
            }
            set { _PerfilDash = value; }
        }

        private List<Projeto> _projetos { get; set; }
        public List<Projeto> Projetos
        {
            get
            {
                var projs = this.Memberships.Select(x => new Projeto() { IdExterno = x.Project.Id.ToString(), Identifier = x.Project.Id.ToString() }).ToList();
                return projs;
            }
            set { _projetos = value; }
        }

        public List<Projeto> projetosRedmine { get; set; }
    }
}