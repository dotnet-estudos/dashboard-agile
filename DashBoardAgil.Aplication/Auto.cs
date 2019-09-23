using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.DashBoardApi.Util;
using DashBoardAgil.Services;
using DashBoardAgil.Domain.Models;
using DashBoardAgil.Domain.Enums;
using DashBoardAgil.Domain.Interfaces;
using DashBoardAgil.Services.Service;

namespace DashBoardAgil.Aplication
{
    public class Auto
    {

        #region Singleton

        //Variaveis Privadas
        private static volatile Auto i_instance = null;

        /// <summary>
        /// Singleton
        /// </summary>
        public static Auto Instance
        {
            get
            {
                if (i_instance == null)
                {
                    lock (typeof(Auto))
                    {
                        if (i_instance == null)
                        {
                            i_instance = new Auto();
                        }
                    }
                }
                return i_instance;
            }
        }

        //Inicialização Privada
        private Auto()
        {
            LastStatusExecucao = true;
            CFDGerado = false;
            CFDExecucao = false;
            ThrougRejecExecucao = false;
            HourRun = new List<int> { 12, 0 };
            
            ResetControleAgendador();

            GlobalConfig.Instance.CarregarConfig();

            _manager = new RedmineService(
                           GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.RedmineUrl,
                           GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.User,
                           CriptografiaAES.Instance.Descriptografar(GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.Pass));

            //ResetControleCFDTimer();
            //ResetControleThroughputAgendador();
        }

        private void ResetControleDiagramnsTimer()
        {
            ControleCfdTimer = new System.Timers.Timer { Enabled = true, Interval = 1000 * 60 * 30 };
            ControleCfdTimer.Elapsed += new System.Timers.ElapsedEventHandler(RunGenerateDiagrams);
            ControleCfdTimer.Start();
            Debugar.WriteLine(String.Format("ResetControleCFDTimer: ControleAgendador Reiniciado"));
        }

        private void ResetControleAgendador()
        {
            
            ControleAgendador = new System.Timers.Timer {Enabled = true, Interval = 1000*30};
            ControleAgendador.Elapsed += new System.Timers.ElapsedEventHandler(ControleExecucaoTimer);
            ControleAgendador.Start();
            Debugar.WriteLine(String.Format("ResetControleAgendador: ControleAgendador Reiniciado"));
        }

        //private void ResetControleThroughputAgendador()
        //{
        //    ControleThroughtPutTimer = new System.Timers.Timer { Enabled = true, Interval = 30000 };
        //    ControleThroughtPutTimer.Elapsed += new System.Timers.ElapsedEventHandler(RunGenerateThroughputRejec);
        //    ControleThroughtPutTimer.Start();
        //}

        #endregion

        #region Properties

        private IRedmineService _manager = null;

        public System.Timers.Timer DashControl;

        public System.Timers.Timer ControleAgendador;

        public System.Timers.Timer ControleCfdTimer;

        public System.Timers.Timer ControleThroughtPutTimer;


        public bool LastStatusExecucao { get; set; }

        public bool EmExecucao { get; set; }

        public bool Ativo { get; set; }

        public bool CFDGerado { get; set; }
        public bool CFDExecucao { get; set; }
        public bool ThrougRejecExecucao { get; set; }
        public bool ThrougRejecGerado { get; set; }

        public List<int> HourRun { get; set; }

        #endregion

        #region Automatizador


        public void StartDashBoardControl()
        {
            GlobalConfig.Instance.CarregarConfig();

            #region Configura e Inicia o Timer

#if DEBUG
            DashControl = new System.Timers.Timer { Enabled = true, Interval = 60000 };
#else
            DashControl = new System.Timers.Timer { Enabled = true, Interval = GetInteval(GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.TipoContagemTempo, GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.ExecTime)};
#endif
            //Global.Instance.IntervaloExecucao;//Global.Instance.IntervaloExecucao;//60000; //300000;//teste de 1 min 60.000 // Padrao 300.000 sem '.'

            DashControl.Elapsed += new System.Timers.ElapsedEventHandler(RunAutoClossing);
            Start();


            Debugar.WriteLine(String.Format("Primeira Execução"));

            Auto.Instance.RunAuto();

            Debugar.WriteLine(String.Format("Serviço Configurado para Rodar em: {0} {1}(s)", GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.ExecTime, GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.TipoContagemTempo.ToString()));



            #endregion


        }

        private void ControleExecucaoTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            GlobalConfig.Instance.CarregarConfig();
           
        }

        private void RunAutoClossing(object sender, System.Timers.ElapsedEventArgs e)
        {
            Auto.Instance.RunAuto();
        }

        private void RunGenerateDiagrams(object sender, System.Timers.ElapsedEventArgs e)
        {
            Auto.Instance.GenerateThrougRejec();
            Auto.Instance.GenerateCfd();

        }

        //private void RunGenerateThroughputRejec(object sender, System.Timers.ElapsedEventArgs e)
        //{

        //}



        private void RunAuto()
        {
            Debugar.WriteLine(string.Format("{0} - {1} - {2}", "Hora", DateTime.Now, "RunAutoClossing"), GlobalConfig.Instance.Categoria);

            GlobalConfig.Instance.CarregarConfig();

            if (GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.FechamentoAutomaticoAtivado)
            {
                try
                {
                    DashControl.Stop();
                    EmExecucao = true;
                    _manager = new RedmineService(
                            GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.RedmineUrl,
                            GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.User,
                            CriptografiaAES.Instance.Descriptografar(GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.Pass));

                    Debugar.WriteLine(string.Format("{0} - {1} - {2}", "Hora", DateTime.Now, "Objeto Mananger Criado"), GlobalConfig.Instance.Categoria);

                    var projetos = GlobalConfig.Instance.ConfiguracaoRedmine.ProjetosAtivos().Select(x => x.Id).ToArray();
                    
                    var result = _manager.FecharChamadosAvaliarProducao(projetos, GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.Prazo);


                    Debugar.WriteLine(String.Format("Chamados Fechados: {0}", result.Count()), GlobalConfig.Instance.Categoria);
                    
                    result.ToList().ForEach((item) =>
                    {
                        Debugar.WriteLine(String.Format("{0}", item), GlobalConfig.Instance.Categoria);
                        Global.Instance.GravarArquivo(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Temp", "Chamados", "Fechados"),item, String.Format("{0}.txt", DateTime.Now.Date.ToString("ddMMyyyy")), true);

                    });


                }
                catch (Exception exception)
                {
                    Debugar.WriteLine(string.Format("{0} - {1} - DashControlAtive-{2} [Error]: {3}", "Finalizado carga", DateTime.Now, GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.FechamentoAutomaticoAtivado, exception.Message), GlobalConfig.Instance.Categoria);
                }
                finally
                {
#if DEBUG
                    DashControl.Interval = 60000;
#else
                    DashControl.Interval = GetInteval(GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.TipoContagemTempo, GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.ExecTime);
#endif
                    Debugar.WriteLine(String.Format("Serviço Configurado para Rodar em: {0} {1}(s)", GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.ExecTime, GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.TipoContagemTempo.ToString()));

                    if (Ativo)
                    {
                        DashControl.Start();
                    }

                    EmExecucao = false;


                }


                Debugar.WriteLine(string.Format("{0} - {1} - DashControlAtive-{2}", "Finalizado carga", DateTime.Now, GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.FechamentoAutomaticoAtivado), GlobalConfig.Instance.Categoria);
            }

        }

        private void GenerateCfd()
        {
            Debugar.WriteLine(String.Format("CFD: GenerateCfd Inicio; CFDExecucao {0}, CFDGerado {1}, HourRun {2}", CFDExecucao, CFDGerado, string.Join(",", HourRun.ToArray())));
            if (!CFDExecucao)
            {
                if (!CFDGerado && HourRun.Contains(DateTime.Now.Hour))
                {
                    CFDGerado = true;
                    Debugar.WriteLine(String.Format("CFDGerado: Alterado para: {0}", CFDGerado));
                    //ControleCfdTimer.Stop();
                    try
                    {
                        CFDExecucao = true;
                        Debugar.WriteLine(String.Format("CFDExecucao: Alterado para: {0}", CFDExecucao));
                        if (_manager.GerarCfdTeams())
                        {
                            CFDExecucao = false;
                            Debugar.WriteLine(String.Format("CFDExecucao: Alterado para: {0}", CFDExecucao));
                        }
                        // ControleCfdTimer.Start();
                    }
                    catch (Exception ex)
                    {
                        CFDExecucao = false;
                        CFDGerado = false;
                        Debugar.WriteLine(String.Format("CFDExecucao: Alterado para: {0}", CFDExecucao));
                        Debugar.WriteLine(String.Format("CFDGerado: Alterado para: {0}", CFDGerado));

                        Debugar.WriteLine(String.Format("CFD: Exception: {0}; Inner: {1}", ex.Message, ex.InnerException != null ? ex.InnerException.Message : ""));

                    }
                    // ControleCfdTimer.Start();
                }
                else if (CFDGerado && !HourRun.Contains(DateTime.Now.Hour))
                {
                    CFDGerado = false;
                    Debugar.WriteLine(String.Format("CFDGerado: Alterado para: {0}", CFDGerado));
                }
            }
            else
            {
                Debugar.WriteLine(String.Format("CFD: Em execucao"));
            }
            Debugar.WriteLine(String.Format("CFD: GenerateCfd Fim;"));
        }

        private void GenerateThrougRejec()
        {
            Debugar.WriteLine(String.Format("ThrougRejec: GenerateThrougRejec Inicio; ThrougRejecExecucao: {0} , ThrougRejecGerado:{1}, HourRun: {2}", ThrougRejecExecucao, ThrougRejecGerado, string.Join(",", HourRun.ToArray())));
            
            if (!ThrougRejecExecucao)
            {
                if (!ThrougRejecGerado && HourRun.Contains(DateTime.Now.Hour))
                {
                    ThrougRejecGerado = true;
                    Debugar.WriteLine(String.Format("ThrougRejecGerado: Alterado para: {0}", ThrougRejecGerado));
                    //ControleCfdTimer.Stop();
                    try
                    {
                        ThrougRejecExecucao = true;
                        Debugar.WriteLine(String.Format("ThrougRejecExecucao: Alterado para: {0}", ThrougRejecExecucao));
                        if (_manager.GerarThroughputReject())
                        {
                            ThrougRejecExecucao = false;
                            Debugar.WriteLine(String.Format("ThrougRejecExecucao: Alterado para: {0}", ThrougRejecExecucao));
                        }
                        // ControleCfdTimer.Start();
                    }
                    catch (Exception ex)
                    {
                        ThrougRejecExecucao = false;
                        ThrougRejecGerado = false;
                        Debugar.WriteLine(String.Format("ThrougRejecGerado: Alterado para: {0}", ThrougRejecGerado));
                        Debugar.WriteLine(String.Format("ThrougRejecExecucao: Alterado para: {0}", ThrougRejecExecucao));
                        Debugar.WriteLine(String.Format("ThrougRejec: Exception: {0}; Inner: {1}", ex.Message, ex.InnerException != null ? ex.InnerException.Message : ""));

                    }
                    // ControleCfdTimer.Start();
                }
                else if (ThrougRejecGerado && !HourRun.Contains(DateTime.Now.Hour))
                {
                    ThrougRejecGerado = false;
                    Debugar.WriteLine(String.Format("ThrougRejecGerado: Alterado para: {0}", ThrougRejecGerado));
                }
            }
            else
            {
                Debugar.WriteLine(String.Format("ThrougRejec: Em execucao"));
            }
            Debugar.WriteLine(String.Format("ThrougRejec: GenerateThrougRejec Fim;"));
           
        }

        #endregion

        #region StatusExecução

        public void Start()
        {
            DashControl?.Start();
            // ControleCfdTimer?.Start();
            ResetControleAgendador();
            ResetControleDiagramnsTimer();
            Debugar.WriteLine("ResetControlers Reiniciado");
            Ativo = true;
            Debugar.WriteLine("Servido Iniciado");
        }

        public void Stop()
        {
            DashControl?.Stop();
            //ControleCfdTimer?.Stop();

            ResetControleAgendador();
            ResetControleDiagramnsTimer();
            Debugar.WriteLine("ResetControlers Reiniciado");

            Ativo = false;
            Debugar.WriteLine("Servido Parado");
        }

        public void ToggleServico()
        {
            if (Ativo)
            {
                Stop();
            }
            else
            {
                Start();
            }
        }

        public bool StatusAgent()
        {
            return Ativo;
        }

        #endregion

        private double GetInteval(TipoContagem tipo, int ExecTime)
        {
            double retorno = 0;

            switch (tipo)
            {
                case TipoContagem.Hora:
                    {
                        retorno = ExecTime * (60000 * 60);
                    }
                    break;
                case TipoContagem.Minuto:
                    {
                        retorno = ExecTime * (60000);
                    }
                    break;
                case TipoContagem.Segundo:
                    {
                        retorno = ExecTime * (1000);
                    }
                    break;

            }
            return retorno;
        }
    }
}
