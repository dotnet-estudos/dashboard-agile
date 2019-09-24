using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DashBoardAgil.DashBoardApi.Controllers.Api
{
    public class MenuController : BaseApiController
    {
        public object Get(int idUsuario)
        {
            var menu = new List<DashBoardAgil.Domain.Models.Menu>();

            return menu;
        }
    }
}
