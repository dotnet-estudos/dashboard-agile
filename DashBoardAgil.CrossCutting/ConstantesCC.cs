using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DashBoardAgil.CrossCutting
{
    public class ConstantesCC
    {
        public static class Details
        {
            public const string Name_Attr = "attr";
            public const string Name_Tag = "tag_list";
            public const string Value_AguardandoInformacao = "Aguardando Informação";
            public const string Value_Rejeitado = "Rejeitado";
        }


        public const string TEXTO_VAZIO = "-";
        public const string TEXTO_SIM = "Sim";
        public const string TEXTO_NAO = "Não";
        public const string TEXTO_POSSUI = "Possui";
        public const string TEXTO_NAO_POSSUI = "Não possui";
        public const string TEXTO_RETICENCIAS = "...";
        public const string CAMPO_OBRIGATORIO = "Campo Obrigatório.";

        public const string FORMATO_DATA_PADRAO = "dd/MM/yyyy";
        public const string FORMATO_MES_ANO = "MM/yyyy";
        public const string FORMATO_ANO_MES_DIA = "yyyy-MM-dd";
        public const string FORMATO_MES = "MM";
        public const string FORMATO_DATA_RODAPE = @"d \de MMMM \de yyyy";
        public const string FORMATO_PERCENTUAL = @"{0:N2} %";
        public const string FORMATO_CNPJ = @"{0:00\.000\.000\/0000\-00}";
        public const string FORMATO_CPF = @"{0:000\.000\.000\-00}";

        public const string REGEX_REMOVE_CARACTERES_ESPECIAIS = "[^0-9a-zA-Z]+";

        public const string REGEX_SOMENTE_NUMEROS = "[^0-9]+";

        public const int TAMANHO_PAGINA_PADRAO = 20;

        public const int HORA_INI_EXP = 8;
        public const int HORA_FIM_EXP = 18;
        
        public const string COOKIE_REQ_BAIXAR_ARQUIVO = "req-token-arquivo";
        public const string COOKIE_RESP_BAIXAR_ARQUIVO = "resp-token-arquivo";

        public const string CONFIG_VERSAO_CUSTOMIZADA = "VersaoCustomizada";

        public const string ALFABETO = "#ABCDEFGHIJKLMNOPQRSTUWVXYZ";

        public const string SESSAO_APL_APLICACAO = "Aplicacao";

        #region SMPT EMAIL

        public static string CONTA_ENVIO = "comprasnet@example.com";

        public static string SMTPCLIENT_HOST = "example.com";

        public static string SMTPCLIENT_PORT = "25";

        public static Regex REGEX_VALIDA_EMAIL = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");

        public static string PATH_THROUGHPUT_REJECT_DIAGRAM => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp", "DashBoard", "ThrptReject", DateTime.Now.Date.ToString("yyyyMMdd"));

        public static string PATH_CFD_DIAGRAM => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp", "DashBoard", "CFD", DateTime.Now.Date.ToString("yyyyMMdd"));

        #endregion
    }
}
