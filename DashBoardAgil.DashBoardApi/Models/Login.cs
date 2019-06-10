using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Redmine.Net.Api.Types;

namespace DashBoardAgil.DashBoardApi.Models
{
    public class Login 
    {
        public string Email { get; set; }
        public string Senha { get; set; }
        public bool  Ativo { get; set; }
    }
}