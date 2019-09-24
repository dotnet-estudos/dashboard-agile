using System;
using System.IO;
using DashBoardAgil.Domain.Models;
using DashBoardAgil.CrossCutting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        #endregion

        private readonly string PathLog =  Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Temp");
        string pathConfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"config.json");

        #region Propriedades do json de config

	    public string Categoria => "GlobalConfig";

	    public ConfigRedmine ConfiguracaoRedmine { get; set; }

		#endregion

		public void CarregarConfig()
		{
			
			using (StreamReader sr = new StreamReader(pathConfig))
			{
				string json = sr.ReadToEnd();
				ConfiguracaoRedmine = JsonConvert.DeserializeObject<ConfigRedmine>(json);
			}
		}

	    public void GravarConfig()
	    {
	        var ConfigRedmine = JsonConvert.SerializeObject(ConfiguracaoRedmine);

            Global.Instance.GravarArquivo(System.AppDomain.CurrentDomain.BaseDirectory, ConfigRedmine, "config.json");
            GlobalConfig.Instance.CarregarConfig();
	    }

    }
}