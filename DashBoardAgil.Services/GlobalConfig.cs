using System;
using System.IO;
using DashBoardAgil.Domain.Models;
using DashBoardAgil.CrossCutting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DashBoardAgil.InfraData.Contexto;
using System.Linq;
using DashBoardAgil.Domain.Interfaces;
using DashBoardAgil.Services.Service;
using System.Collections.Generic;

namespace DashBoardAgil.Services
{
	public class GlobalConfig
	{
		#region Singleton
		private static volatile GlobalConfig i_instance = null;
		public static GlobalConfig Instance
		{
			get
			{
				if (i_instance == null)
				{
					lock (typeof(GlobalConfig))
					{
						if (i_instance == null)
						{
							i_instance = new GlobalConfig();
						}
					}
				}
				return i_instance;
			}
		}
        public GlobalConfig()
        {
            _redmineService = new RedmineService();
            LocalProjeto = new List<Projeto>();
            LocalProjeto = _redmineService.GetTime();
        }
        #endregion

        private readonly string PathLog =  Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Temp");
        string pathConfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"config.json");
        private IRedmineService _redmineService;

        #region Propriedades do json de config

	    public string Categoria => "GlobalConfig";

	    public ConfigRedmine ConfiguracaoRedmine { get; set; }
        private List<Projeto> LocalProjeto { get; set; }

		#endregion

		public void CarregarConfig()
		{
			
			using (StreamReader sr = new StreamReader(pathConfig))
			{
				string json = sr.ReadToEnd();
				ConfiguracaoRedmine = JsonConvert.DeserializeObject<ConfigRedmine>(json);
                //var db = new DashBoardAgil.InfraData.Contexto.Contexto(false);
                ConfiguracaoRedmine.AllowList = LocalProjeto;

            }
		}

	    public void GravarConfig()
	    {
            LocalProjeto = _redmineService.GetTime();
            var ConfigRedmine = JsonConvert.SerializeObject(ConfiguracaoRedmine);

            Global.Instance.GravarArquivo(System.AppDomain.CurrentDomain.BaseDirectory, ConfigRedmine, "config.json");
            GlobalConfig.Instance.CarregarConfig();
	    }

    }
}