using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DashBoardAgil.CrossCutting
{
    public class Global
    {
        #region Singleton

        //Variaveis Privadas
        private static volatile Global i_instance = null;

        /// <summary>
        /// Singleton
        /// </summary>
        public static Global Instance
        {
            get
            {
                if (i_instance == null)
                {
                    lock (typeof(Global))
                    {
                        if (i_instance == null)
                        {
                            i_instance = new Global();
                        }
                    }
                }
                return i_instance;
            }
        }

        //Inicialização Privada
        private Global()
        {

        }

        #endregion

        #region Proriedades
        private readonly string PathLog = String.Format(@"{0}\\{1}\\", System.AppDomain.CurrentDomain.BaseDirectory, "Temp");

        public int IntervaloExecucao { get; set; }

        public string CpfConsultante { get; set; }

        public string CertificadoCliente { get; set; }

        public string CertificadoClienteSenha { get; set; }

        public string CertificadoService { get; set; }

        public string TipoPedido { get; set; }//tipoPedido = "1"; 1-consulta 2- Emissao, 3-consulta Emissao

        public string CodSistemaInfoconv { get; set; } //codSistemaInfoconv = "5";

        public string CodPerfilInfoconv { get; set; } //codPerfilInfoconv = "1";

        public string ChavePedido { get; set; } //chavePedido = "";

        public bool ServicoOn { get; set; } //chavePedido = "";

        public bool GravaFatura { get; set; } //chavePedido = "";

        public string SmtpclientHost { get; set; }

        public int SmtpPort { get; set; }

        public string[] DestinatariosEmail { get; set; }

        public string AssuntoEmail { get; set; }

        public string AutorEmail { get; set; }

        public DateTime LastEmailSended { get; set; }

        public int TempoDeEnvioDeEmail { get; set; }

        #endregion

        #region Log

        /// <summary>
        /// Registra Mensagem TXT caso nao consiga registrar no banco
        /// </summary>
        /// <param name="a_Metodo"></param>
        /// <param name="a_Log"></param>
        public void RegistraMensagem(String a_Metodo, String a_Log)
        {
            try
            {
                if (!Directory.Exists(PathLog))
                {
                    Directory.CreateDirectory(PathLog);
                }
                StreamWriter l_SrLog = new StreamWriter(PathLog + "Log_" + DateTime.Now.ToString("ddMMyyyy") + ".txt", true, Encoding.UTF8);
                l_SrLog.WriteLine(DateTime.Now.ToString("dd/MM/yy HH:mm") + " - " + a_Metodo + " " + a_Log + "\n");
                l_SrLog.Flush();
                l_SrLog.Close();
            }
            catch (Exception e)
            {
            }
        }

        public void GravarArquivo(String path, String conteudo, String nomeArquivo, Boolean append = false)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                StreamWriter l_SrLog = new StreamWriter(Path.Combine(path, nomeArquivo) , append, Encoding.UTF8);
                l_SrLog.WriteLine(conteudo);
                l_SrLog.Flush();
                l_SrLog.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string LerArquivo(string path)
        {
            string arquivo="";

            if (File.Exists(path))
            {

                using (StreamReader sr = new StreamReader(path))
                {
                    arquivo = sr.ReadToEnd();

                }
            }

            return arquivo;
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="a_Metodo"></param>
        /// <param name="a_Log"></param>
        public void GravaLog(String a_Metodo, String a_Log, bool email = false)
        {
            try
            {
                Global.Instance.RegistraMensagem(a_Metodo, a_Log);
                if (email)
                {
                    EnviarEmailLog(a_Metodo, a_Log);
                }

            }
            catch (Exception ex)
            {
                //i_EventLog.WriteEntry(a_Metodo + " - Erro: Falha ao gravar notificação no arquivo de LOG: " + ex.Message, EventLogEntryType.Error);

                // this.RegistraMensagem(a_Metodo, a_Log);
            }
        }

        private void EnviarEmailLog(string a_Metodo, string a_Log)
        {

            if (this.LastEmailSended == null || (DateTime.Now - LastEmailSended).Hours >= this.TempoDeEnvioDeEmail)
            {
                this.LastEmailSended = DateTime.Now;

                SmtpClient smtpClient = new SmtpClient();
                MailMessage message = new MailMessage();

                MailAddress autorEmail = new MailAddress(this.AutorEmail);

                smtpClient.Host = this.SmtpclientHost;

                smtpClient.Port = this.SmtpPort;

                message.From = autorEmail;

                foreach (var Destino in this.DestinatariosEmail)
                {
                    message.To.Add(Destino);
                }

                message.Subject = this.AssuntoEmail;

                message.IsBodyHtml = true;

                message.Body = String.Format("Mensagem de Log Do InfoConv: Metodo: {0}; Log: {1}", a_Metodo, a_Log);

                smtpClient.Send(message);
            }
        }

        #endregion

        public string ConcatenarLista(string contac, List<String> list)
        {
            var ret = new StringBuilder();
            list.ForEach(x=>
            {
                ret.AppendFormat("{0}{1}", x, list.IndexOf(x) == list.Count - 1 ? string.Empty : contac);
            });

           return ret.ToString();
        }

          
        public dynamic JsonToObject(string pathArquivo)
        {
           var rawjson  = Global.Instance.LerArquivo(pathArquivo);
            dynamic objeto = JObject.Parse(rawjson);

            return objeto;
        }

        public static T ReflectObjec<T>(object origem, T destino)
        {
            if (origem != null)
            {
                foreach (var propertyIbj in origem.GetType()
                    .GetProperties().Where(x =>
                        destino?.GetType().GetProperty(x.Name)  != null &&
                        destino?.GetType().GetProperty(x.Name).PropertyType.FullName == x.PropertyType.FullName))
                {


                    destino.GetType().GetProperty(propertyIbj.Name).SetValue(destino, propertyIbj.GetValue(origem));
                }
            }

            return destino;
        }
    }
}
