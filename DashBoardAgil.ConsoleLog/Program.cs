using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DashBoardAgil.Aplication;
using DashBoardAgil.DashBoardApi.Util;
using DashBoardAgil.Services;
using DashBoardAgil.Domain.Interfaces;
using DashBoardAgil.Services.Service;

namespace DashBoardAgil.ConsoleLog
{
    class Program
    {
        private static void Main(string[] args)
        {

            //IRedmineService _manager = null;

            //GlobalConfig.Instance.CarregarConfig();

            //if (GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.FechamentoAutomaticoAtivado)
            //{


            //    _manager = new RedmineService(
            //        GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.RedmineUrl,
            //        GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.User,
            //        CriptografiaAES.Instance.Descriptografar(GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.Pass));

            //    Debug.WriteLine(string.Format("{0} - {1} - {2}", "Hora", DateTime.Now, "Objeto Mananger Criado"),
            //        GlobalConfig.Instance.Categoria);

            //    var projetos = GlobalConfig.Instance.ConfiguracaoRedmine.ProjetosAtivos.Select(x => x.Id).ToArray();



            //    var result =_manager.FecharChamadosAvaliarProducao(projetos,
            //        GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.Prazo);
            //}


            var data = new[] {
                            new { Id = 0, Price = 2 },
                            new { Id = 1, Price = 10 },
                            new { Id = 2, Price = 30 },
                            new { Id = 3, Price = 50 },
                            new { Id = 4, Price = 120 },
                            new { Id = 5, Price = 200 },
                            new { Id = 6, Price = 1024 },
                        };

            var ranges = new[] { 10, 50, 100, 500, 501 };
            var grouped = ranges.Select(r => new {
                Price = r,
                Count = data.Count(x =>
                            (ranges.ToList().IndexOf(r) < ranges.ToList().Count-1 ? 
                                (ranges.ToList().IndexOf(r)>0  ? 
                                (ranges[ranges.ToList().IndexOf(r)-1] < x.Price && x.Price <= r) : 
                                 x.Price <= r
                                 ) : 
                            x.Price >= r
                            )
                        )
            });

        }

       
    }
}
