using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using DashBoardAgil.CrossCutting.Enums;
using RazorEngine;
using RazorEngine.Templating;
using System.Web.Mvc;
using System.Web;
using DashBoardAgil.CrossCutting.Object;
using Encoding = System.Text.Encoding;
using Redmine.Net.Api.Types;
using Redmine.Net.Api;

namespace DashBoardAgil.CrossCutting
{
    public static class Util
    {
        private const string HashAlgorithm = "SHA256";
        private const int PasswordIterations = 2;
        private const int KeySize = 256;
        private const string InitVector = "!1A3g2D4s9K556g7";
        private const string SaltValue = "comprasnet.BA";
        private const string PassPhrase = "comprasnet.BA";

        /// <summary>
        /// Função que verifica se a string informada “Tes123@#$” will be accepted.
        /// UMA LETRA MINUSCULA
        /// UMA LETRA MAIUSCULA
        /// UM NUMERO
        /// UM ESPECIAL
        /// NO MINIMO 8 CARACTERES
        /// </summary>
        /// <param name="password">Senha.</param>
        /// <returns></returns>
        public static bool VerificarPassword(string password)
        {
            int tamanhoMinimo = 8;
            int tamanhoMinusculo = 1;
            int tamanhoMaiusculo = 1;
            int tamanhoNumeros = 1;

            // Definição de letras minusculas
            Regex regTamanhoMinusculo = new Regex("[a-z]");

            // Definição de letras minusculas
            Regex regTamanhoMaiusculo = new Regex("[A-Z]");

            // Definição de letras minusculas
            Regex regTamanhoNumeros = new Regex("[0-9]");

            // Verificando tamanho minimo
            if (password.Length < tamanhoMinimo) return false;

            // Verificando caracteres minusculos
            if (regTamanhoMinusculo.Matches(password).Count < tamanhoMinusculo) return false;

            // Verificando caracteres maiusculos
            if (regTamanhoMaiusculo.Matches(password).Count < tamanhoMaiusculo) return false;

            // Verificando numeros
            if (regTamanhoNumeros.Matches(password).Count < tamanhoNumeros) return false;

            return true;
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                .GetName();
        }

        /// <summary>
        /// Retorna uma representação de texto como "Sim" ou "Não" do booleano apresentado.
        /// Caso o valor informado seja nulo, um texto vazio é retornado.
        /// </summary>
        public static string BooleanoParaTexto(bool? booleano)
        {
            string resultado = ConstantesCC.TEXTO_VAZIO;

            if (booleano.HasValue)
            {
                resultado = (booleano.Value ? ConstantesCC.TEXTO_SIM : ConstantesCC.TEXTO_NAO);
            }
            return resultado;
        }

        /// <summary>
        /// Retorna uma representação booleana do texto "Sim" ou "Não" apresentado.
        /// </summary>
        public static bool? TextoParaBooleano(string texto)
        {
            bool? resultado = null;

            if (string.Equals(texto, ConstantesCC.TEXTO_SIM, StringComparison.InvariantCultureIgnoreCase))
            {
                resultado = true;
            }
            else if (string.Equals(texto, ConstantesCC.TEXTO_NAO, StringComparison.InvariantCultureIgnoreCase))
            {
                resultado = false;
            }
            return resultado;
        }

        /// <summary>
        /// Retorna uma data a partir de sua representação em texto.
        /// </summary>
        public static DateTime? TextoParaData(string texto)
        {
            DateTime? resultado = null;

            if (!string.IsNullOrEmpty(texto))
            {
                resultado = DateTime.Parse(texto);
            }
            return resultado;
        }

        /// <summary>
        /// Retorna uma data e hora a partir de sua representação em texto.
        /// </summary>
        public static DateTime? TextoParaDataHora(string texto, string formato)
        {
            DateTime? resultado = null;

            if (!string.IsNullOrEmpty(texto))
            {
                resultado = DateTime.ParseExact(texto, formato, CultureInfo.InvariantCulture);
            }
            return resultado;
        }

        public static string FormatarData(DateTime? data, string formato = ConstantesCC.FORMATO_DATA_PADRAO)
        {
            string resultado = string.Empty;

            if (data.HasValue)
            {
                resultado = data.Value.ToString(formato);
            }
            return resultado;
        }

        public static string FormatarMoeda(decimal? numero)
        {
            return numero.GetValueOrDefault(0).ToString("N2");
        }

        public static string FormataCnpjCpf(string cnpjCpf)
        {
            var formatacao = cnpjCpf.Trim().Length == 11 ? ConstantesCC.FORMATO_CPF : ConstantesCC.FORMATO_CNPJ;
            var cnpjCpfFormatado = String.Format(formatacao, Convert.ToInt64(cnpjCpf.Trim()));

            return cnpjCpfFormatado;
        }

        public static string FormataTelefone(string tel)
        {
            if (string.IsNullOrEmpty(tel)) return null;
            string somenteNumeros = Regex.Replace(tel, ConstantesCC.REGEX_SOMENTE_NUMEROS, String.Empty);
            return Regex.Replace(somenteNumeros, @"(\d{2})(\d{4,5})(\d{4})", "(0XX$1) $2-$3");
        }

        public static string ToHtmlString(Dictionary<string, object> dict)
        {
            StringBuilder str = new StringBuilder();
            string valor;

            foreach (string chave in dict.Keys)
            {
                valor = ToHtmlStringValue(dict[chave]);
                str.Append(string.Format("{0}={1} ", chave, valor));
            }
            return str.ToString();
        }

        public static string ToHtmlStringValue(object value)
        {
            string strValue;

            if (value == null)
            {
                strValue = null;
            }
            else if (value is bool)
            {
                strValue = value.ToString().ToLowerInvariant();
            }
            else
            {
                strValue = value.ToString();
            }
            return strValue;
        }

        public static T ReflectObjec<T>(object origem, T destino)
        {
            if (origem != null)
            {
                foreach (var propertyIbj in origem.GetType()
                    .GetProperties()
                    .Where(
                        x =>
                            destino.GetType().GetProperty(x.Name) != null &&
                            destino.GetType().GetProperty(x.Name).PropertyType.FullName == x.PropertyType.FullName &&
                            x.CanWrite)
                    )
                {


                    destino.GetType().GetProperty(propertyIbj.Name).SetValue(destino, propertyIbj.GetValue(origem));
                }
            }
            return destino;
        }

        public static string ReadModelFromTemplate(object model, string bodyTemplate)
        {
            if (model != null && bodyTemplate != null)
            {
                return Engine.Razor.RunCompile(bodyTemplate, bodyTemplate.GetHashCode().ToString(), model.GetType(),
                    model);
            }
            return null;
        }

        public static T RetirarCaracteresEspeciaisObjeto<T>(T ObjetoRetorno, string[] excludesFields = null)
        {

            foreach (var propertyIbj in ObjetoRetorno.GetType()
                .GetProperties()
                .Where(
                    x =>
                        ObjetoRetorno.GetType().GetProperty(x.Name) != null &&
                        x.GetValue(ObjetoRetorno) != null &&
                        ObjetoRetorno.GetType().GetProperty(x.Name).PropertyType.Name == "String" &&
                        (excludesFields == null || !excludesFields.Contains(x.Name))))
            {


                ObjetoRetorno.GetType()
                    .GetProperty(propertyIbj.Name)
                    .SetValue(ObjetoRetorno,
                        Regex.Replace((string)propertyIbj.GetValue(ObjetoRetorno),
                            ConstantesCC.REGEX_REMOVE_CARACTERES_ESPECIAIS, String.Empty));
            }

            return ObjetoRetorno;
        }

        public static string GetColumnName(SelectListItem firstOrDefault)
        {

            string coluna;
            switch (firstOrDefault != null ? firstOrDefault.Text : " ")
            {
                case "Física":
                    {
                        coluna = "CPF";
                    }
                    break;

                case "Jurídica":
                    {
                        coluna = "CNPJ";
                    }
                    break;

                default:
                    {
                        coluna = "CPF/CNPJ";
                    }
                    break;
            }

            return coluna;
        }

        public static string Alfabeto
        {
            get { return "#ABCDEFGHIJKLMNOPQRSTUWVXYZ"; }

        }

        public static string PrimeiroCaracterMinusculo(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;

            StringBuilder sb = new StringBuilder();

            string[] strV = value.Split('.');
            foreach (var str in strV)
            {
                sb.Append(Char.ToLowerInvariant(str[0]) + str.Substring(1) + ".");
            }

            string retorno = sb.ToString();
            return retorno.Substring(0, retorno.Length - 1);
        }

        public static string GetMd5Hash(string input)
        {
            if (input != null)
            {
                using (MD5 md5Hash = MD5.Create())
                {
                    // Convert the input string to a byte array and compute the hash.
                    byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                    // Create a new Stringbuilder to collect the bytes
                    // and create a string.
                    StringBuilder sBuilder = new StringBuilder();

                    // Loop through each byte of the hashed data 
                    // and format each one as a hexadecimal string.
                    for (int i = 0; i < data.Length; i++)
                    {
                        sBuilder.Append(data[i].ToString("x2"));
                    }

                    // Return the hexadecimal string.
                    return sBuilder.ToString();
                }
            }
            return "";
        }

        public static string Encrypt(string plainText)
        {
            // Convert strings into byte arrays.
            // Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8 
            // encoding.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(InitVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(SaltValue);

            // Convert our plaintext into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // First, we must create a password, from which the key will be derived.
            // This password will be generated from the specified passphrase and 
            // salt value. The password will be created using the specified hash 
            // algorithm. Password creation can be done in several iterations.
            PasswordDeriveBytes password = new PasswordDeriveBytes
                (
                PassPhrase,
                saltValueBytes,
                HashAlgorithm,
                PasswordIterations
                );

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(KeySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;

            // Generate encryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key bytes.
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor
                (
                    keyBytes,
                    initVectorBytes
                );

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream();

            // Define cryptographic stream (always use Write mode for encryption).
            CryptoStream cryptoStream = new CryptoStream
                (
                memoryStream,
                encryptor,
                CryptoStreamMode.Write
                );

            // Start encrypting.
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            // Finish encrypting.
            cryptoStream.FlushFinalBlock();

            // Convert our encrypted data from a memory stream into a byte array.
            byte[] cipherTextBytes = memoryStream.ToArray();

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert encrypted data into a base64-encoded string.
            string cipherText = Convert.ToBase64String(cipherTextBytes);

            // Return encrypted string.
            return cipherText;
        }

        public static string Decrypt(string cipherText)
        {
            // Convert strings defining encryption key characteristics into byte arrays. 
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(InitVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(SaltValue);

            // Convert our ciphertext into a byte array.
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            // First, we must create a password, from which the key will be 
            // derived. This password will be generated from the specified passphrase and salt value. 
            // The password will be created using the specified hash algorithm. Password creation can be done in several iterations.
            PasswordDeriveBytes password = new PasswordDeriveBytes
                (
                PassPhrase,
                saltValueBytes,
                HashAlgorithm,
                PasswordIterations
                );

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(KeySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();

            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate decryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor
                (
                    keyBytes,
                    initVectorBytes
                );

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

            // Define cryptographic stream (always use Read mode for encryption).
            CryptoStream cryptoStream = new CryptoStream
                (
                memoryStream,
                decryptor,
                CryptoStreamMode.Read
                );
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            // Start decrypting.
            int decryptedByteCount = cryptoStream.Read
                (
                    plainTextBytes,
                    0,
                    plainTextBytes.Length
                );

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert decrypted data into a string. 
            // Let us assume that the original plaintext string was UTF8-encoded.
            string plainText = Encoding.UTF8.GetString
                (
                    plainTextBytes,
                    0,
                    decryptedByteCount
                );

            // Return decrypted string.   
            return plainText;
        }

        public static string ObterNomeCompletoCategoria(string categoria)
        {
            switch (categoria)
            {
                case "NI":
                    return "NI - Não Identificado";
                case "ME":
                    return "ME - Microempresa";
                case "EPP":
                    return "EPP - Empresa de Pequeno Porte";
                case "NO":
                    return "NO - Normal";
                case "ES":
                    return "ES - Especial";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Adiciona ao Log a mensagem desejada.
        /// Arquivo logMensagem.txt (diretório raiz).
        /// </summary>
        /// <param name="mensagem">Mensagem Desejada.</param>
        public static void AddLog(string mensagem)
        {
            System.IO.File.AppendAllText(HttpContext.Current.Request.PhysicalApplicationPath + "//logMensagem.txt", mensagem);
        }

        public static IEnumerable<string> ObtemListaMeses()
        {
            var meses = new List<string>();
            for (var i = 1; i <= 12; i++)
            {
                var mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i);
                mes = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(mes);
                meses.Add(mes);
            }

            return meses;
        }

        /// <summary>
        /// Últimos anos
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<int> ObtemUltimosAnos(int count)
        {
            var anos = new List<int>();
            var anoAtual = DateTime.Now.Year;
            for (var i = anoAtual; i >= anoAtual - (count - 1); i--)
            {
                anos.Add(i);
            }

            return anos;
        }

        /// <summary>
        /// Método privado para Conversão das Datas.
        /// </summary>
        /// <param name="data">Data a ser convertida.</param>
        /// <param name="addHora">Adição de Tempo.</param>
        /// <returns></returns>
        public static DateTime? ParseDate(string data, TimeSpan? addHora = null)
        {
            DateTime? dtReturn = null;
            if (!string.IsNullOrEmpty(data))
            {
                dtReturn = DateTime.Parse(data);

                if (addHora != null)
                {
                    dtReturn = ((DateTime)dtReturn).Add((TimeSpan)addHora);
                }
            }

            return dtReturn;
        }

        /// <summary>
        /// Obtém Letra que representa o perfil de usuário no banco de dados.
        /// </summary>
        /// <param name="perfilUsuario"></param>
        /// <returns></returns>
        public static string ObterLetraBdPerfilUsuario(PerfilUsuario perfilUsuario)
        {
            switch (perfilUsuario)
            {
                case PerfilUsuario.Administrador:
                    return "A";
                case PerfilUsuario.Comissao:
                    return "L";
                case PerfilUsuario.Gestor:
                    return "G";
                case PerfilUsuario.Habilitador:
                    return "H";
                default:
                    return null;
            }
        }

        /// <summary>
        /// Remove caracteres não numéricos
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveNaoNumericos(string text)
        {
            Regex reg = new Regex(@"[^0-9]");
            string ret = reg.Replace(text, string.Empty);
            return ret;
        }
        /// <summary>
        /// Valida se um cpf é válido
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public static bool ValidaCpf(string cpf)
        {
            //Remove formatação do número, ex: "123.456.789-01" vira: "12345678901"
            cpf = RemoveNaoNumericos(cpf);
            if (cpf.Length > 11)
                return false;
            while (cpf.Length != 11)
                cpf = '0' + cpf;
            bool igual = true;
            for (int i = 1; i < 11 && igual; i++)
                if (cpf[i] != cpf[0])
                    igual = false;
            if (igual || cpf == "12345678909")
                return false;
            int[] numeros = new int[11];
            for (int i = 0; i < 11; i++)
                numeros[i] = int.Parse(cpf[i].ToString());
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];
            int resultado = soma % 11;

            return ValidaCpfFinal(resultado, numeros);
        }

        private static bool ValidaResultadoCpfFinal(int resultado, int[] numeros)
        {
            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else if (numeros[9] != 11 - resultado)
                return false;
            return true;
        }

        private static bool ValidaCpfFinal(int resultado, int[] numeros)
        {
            if (!ValidaResultadoCpfFinal(resultado, numeros))
                return false;

            var soma = 0;
            for (int i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];
            resultado = soma % 11;
            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;
                return true;
            }
            if (numeros[10] != 11 - resultado)
                return false;
            return true;
        }

        private static FileVersionInfo GetVersionInfo()
        {
            Assembly assembly = Assembly.GetEntryAssembly();

            if (IsNullOrEmpty(assembly))
            {
                assembly = GetAssemblyByStack();
            }
            return FileVersionInfo.GetVersionInfo(assembly.Location);
        }

        private static Assembly GetAssemblyByStack()
        {
            Assembly assembly = null;
            Module biblioteca = typeof(Util).Module;
            Module modulo;
            int skipFrames = 2;

            do
            {
                modulo = new StackFrame(skipFrames, false)
                    .GetMethod().Module;
                skipFrames++;
            }
            while (modulo == biblioteca);

            if (!IsNullOrEmpty(modulo))
            {
                assembly = modulo.Assembly;
            }
            return assembly;
        }

        public static bool IsNullOrEmpty(dynamic obj)
        {
            bool result;

            if (obj == null)
            {
                result = true;
            }
            else if (obj is string)
            {
                result = string.IsNullOrEmpty(obj as string);
            }
            else if (obj is Array)
            {
                result = (obj as Array).Length == 0;
            }
            else if (obj is IEnumerable)
            {
                result = !(obj as IEnumerable).Cast<object>().Any();
            }
            else
            {
                result = string.IsNullOrEmpty(obj.ToString());
            }
            return result;
        }

        public static string LerConfiguracao(string nome)
        {
            return ConfigurationManager.AppSettings[nome];
        }

        public static int[] GerarArraySalto(int primeiroSalto, int quantidadeSaltos)
        {
            var array = new List<int>();
            var valorAtual = 0;
            for (int i = 0; i < quantidadeSaltos - 1; i++)
            {
                valorAtual += primeiroSalto;
                array.Add(valorAtual);
            }
            valorAtual++;
            array.Add(valorAtual);
            return array.ToArray();
        }

        public static List<int> GetWeekList(DateTime dataIni, DateTime dataFim)
        {
            // Gets the Calendar instance associated with a CultureInfo.
            //CultureInfo myCI = new CultureInfo("en-US");
            var myCI = new CultureInfo("pt-BR");
            var myCal = myCI.Calendar;
            var retorno = new List<int>();
            // Gets the DTFI properties required by GetWeekOfYear.
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

            var ini = myCal.GetWeekOfYear(dataIni, myCWR, myFirstDOW);
            var fim = myCal.GetWeekOfYear(dataFim, myCWR, myFirstDOW);

            for (int i = ini; i < fim; i++)
            {
                retorno.Add(i);
            }

            return retorno;
        }

        public static int GetWeekOfYear(this DateTime data)
        {
            // Gets the Calendar instance associated with a CultureInfo.
            var myCI = new CultureInfo("pt-BR");
            var myCal = myCI.Calendar;

            // Gets the DTFI properties required by GetWeekOfYear.
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

            var ini = myCal.GetWeekOfYear(data, myCWR, myFirstDOW);

            return ini;
        }

        public static int GetRamdonNumber(int min, int max)
        {
            Random random = new Random();
            var ret = random.Next(min, max);
            return ret;
        }

        public static double GetPercentFrom(double percent, double From)
        {
            return (percent * From / 100);
        }

        public static List<Feriado> Feriados(int ano = 0, string uf = "BA", string cidade = "SALVADOR")
        {
            ano = ano == 0 ? DateTime.Now.Year : ano;
            var arquivo = Global.Instance.LerArquivo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Feriados", ano.ToString()));
            if (!(arquivo ?? "fasle").Equals("false"))
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("https://api.calendario.com.br");
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                System.Net.Http.HttpResponseMessage response = client.GetAsync("json=true&ano=" + ano.ToString() + "&estado=" + uf + "&cidade=" + cidade + "&token=dWhpdGxlaUBnbWFpbC5jb20maGFzaD0xNDIxNDk1MDc").Result;
                var result = response.Content;



            }

            return new List<Feriado>();
        }

        public static Predicate<T> Or<T>(params Predicate<T>[] predicates)
        {
            return delegate (T item)
            {
                foreach (Predicate<T> predicate in predicates)
                {
                    if (predicate(item))
                    {
                        return true;
                    }
                }
                return false;
            };
        }

        public static Predicate<T> And<T>(params Predicate<T>[] predicates)
        {
            return delegate (T item)
            {
                foreach (Predicate<T> predicate in predicates)
                {
                    if (!predicate(item))
                    {
                        return false;
                    }
                }
                return true;
            };
        }

        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
            
            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
            
            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }
        
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }

        public static DateTime? GetDateByStatus(Issue chamado, StatusRedmine status, bool oldValue = false)
        {
            switch (status)
            {
                case StatusRedmine.BacklogNovo:
                case StatusRedmine.Novo:
                    {
                        return oldValue ? GetFristJournalByStatus(status, chamado, true)?.CreatedOn ?? null : chamado.CreatedOn;
                    }
                case StatusRedmine.Cancelado:
                case StatusRedmine.Fechado:
                    {
                        return chamado.ClosedOn;
                    }
                default:
                    {
                        var retorno = GetFristJournalByStatus(status, chamado, oldValue);
                        return retorno?.CreatedOn ?? null;
                    }
            }
        }

        public static Journal GetFristJournalByStatus(StatusRedmine status, Issue chamado, bool oldValue = false)
        {
            var retorno = chamado?.Journals?.FirstOrDefault(x =>
            x.Details.Any(Constantes.GetDetail(status.GetHashCode().ToString(), //new Value
                                                 RedmineKeys.STATUS_ID, //Name Pardao status
                                                 ConstantesCC.Details.Name_Attr, //property 
                                                 oldValue //Verificar a Saida do Status
                                               ))) ?? null;
            return retorno;
        }



    }
}