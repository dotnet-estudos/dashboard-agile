using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.Domain.Models;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using DashBoardAgil.CrossCutting.Enums;

namespace DashBoardAgil.Services.Util
{
    public static class RedmineUtil
    {
        public static TimeSpan TempoMedio(this List<Periodo> listaPeriodo)
        {
            double tempoMedio = 0d;
            List<Periodo> tmpList = listaPeriodo.Where(x => x.DataFinal.HasValue).ToList();

            foreach (Periodo item in tmpList)
            {
                TimeSpan ts = item.DataFinal.Value.Subtract(item.DataInicial);
                tempoMedio += ts.Ticks / (double)tmpList.Count;
            }

            return new TimeSpan((long)tempoMedio);
        }

        public static string FormatoRedmine(this Periodo periodo)
        {
            const string formato = "yyyy-MM-dd";

            string dataFinal = periodo.DataFinal?.ToString(formato) ?? "";

            return string.Format("><{0}|{1}", periodo.DataInicial.ToString(formato), dataFinal);
        }

        public static string ChamadosAvaliarProducao()
        {
            return string.Format("{0}|{1}", StatusRedmine.AvaliarProducao.GetHashCode(), StatusRedmine.ProducaoAvaliar.GetHashCode());
        }

        public static DateTime AddDiasUteis(this DateTime data, int dias)
        {
            var sign = Math.Sign(dias);
            var unsignedDays = Math.Abs(dias);

            for (var i = 0; i < unsignedDays; i++)
            {
                do { data = data.AddDays(sign); }
                while (data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday);
            }

            return data;
        }

        public static bool DiferencaDentroDoPrazo(this DateTime data, DateTime datePara, TipoContagem tipoRetorno, double prazo = 0, bool ultil = false)
        {
            if (!ultil)
            {
                return datePara.SubtractByTipoConagem(data, tipoRetorno) <= prazo;
            }

            double valorCalc = 0;
            DateTime? dataRef = null;

            switch (tipoRetorno)
            {
                case TipoContagem.Ano:
                    {

                        valorCalc += prazo * 12 * 30;
                        dataRef = data.AddDiasUteis((int)valorCalc);
                    }
                    break;
                case TipoContagem.Mes:
                    {
                        valorCalc += prazo * 30;
                        dataRef = data.AddDiasUteis((int)valorCalc);
                    }
                    break;
                case TipoContagem.Dia:
                    {
                        dataRef = data.AddDiasUteis((int)valorCalc);
                    }
                    break;
                case TipoContagem.Hora:
                    {
                        return data.AddHoraUtil(prazo) >= datePara;
                    }
                    break;
                default: return false;
            }


            if (dataRef != null)
                return ((DateTime)dataRef).SubtractByTipoConagem(datePara, tipoRetorno) <= prazo;

            return false;
        }

        public static double Diferenca(this DateTime data, DateTime datePara, TipoContagem tipoRetorno, bool ultil = false)
        {
            if (!ultil) return datePara.SubtractByTipoConagem(data, tipoRetorno);

            var valorReturn = 0.0;
            var dataRef = data;
            while (datePara > dataRef)
            {
                valorReturn++;
                switch (tipoRetorno)
                {
                    case TipoContagem.Ano:
                        {
                            dataRef = dataRef.AddDiasUteis(1 * 30 * 12);
                        }
                        break;
                    case TipoContagem.Mes:
                        {
                            dataRef = dataRef.AddDiasUteis(1 * 30);
                        }
                        break;
                    case TipoContagem.Dia:
                        {
                            dataRef = dataRef.AddDiasUteis(1);
                        }
                        break;
                    case TipoContagem.Hora:
                        {
                            dataRef = dataRef.AddHoraUtil(1);
                        }
                        break;
                    //case TipoContagem.Minuto:
                    //    {
                    //        dataRef.AddHoraUtil(valorReturn );
                    //    }
                    //    break;
                    //case TipoContagem.Segundo:
                    //    {
                    //        dataRef.AddHoraUtil(valorReturn);
                    //    }
                    //    break;
                    default:
                        {
                            dataRef = dataRef.AddMilliseconds(1);
                        }
                        break;
                }
            }

            if (datePara < dataRef && tipoRetorno == TipoContagem.Hora)
            {
                dataRef = dataRef.AddHours(-1);
                valorReturn += datePara.Subtract(dataRef).TotalHours;
            }

            return valorReturn;
        }

        public static DateTime AddByTipoContagem(this DateTime data, double valor, TipoContagem tipo)
        {
            switch (tipo)
            {

                case TipoContagem.Ano:
                    {
                        return data.AddYears((int)valor);
                    }
                    break;
                case TipoContagem.Mes:
                    {
                        return data.AddMonths((int)valor);
                    }
                    break;
                case TipoContagem.Dia:
                    {
                        return data.AddDays(valor);
                    }
                    break;
                case TipoContagem.Hora:
                    {
                        return data.AddHours(valor);
                    }
                    break;
                case TipoContagem.Minuto:
                    {
                        return data.AddMinutes(valor);
                    }
                    break;
                case TipoContagem.Segundo:
                    {
                        return data.AddSeconds(valor);
                    }
                default:
                    {
                        return data.AddMilliseconds(valor);
                    }
            }
        }

        public static double SubtractByTipoConagem(this DateTime dataPara, DateTime data, TipoContagem tipoRetorno = TipoContagem.Dia)
        {
            switch (tipoRetorno)
            {
                case TipoContagem.Hora:
                    {
                        return dataPara.Subtract(data).TotalHours;

                    }
                    break;
                case TipoContagem.Minuto:
                    {
                        return dataPara.Subtract(data).TotalMinutes;
                    }
                    break;
                case TipoContagem.Segundo:
                    {
                        return dataPara.Subtract(data).TotalMinutes;
                    }
                    break;
                case TipoContagem.Dia:
                    {
                        return dataPara.Subtract(data).TotalDays;
                    }
                    break;
                default:
                    {
                        return dataPara.Subtract(data).TotalMilliseconds;
                    }

            }
        }

        public static double FatorHoraUtil(this DateTime dateIni)
        {
            return GetFatorHoraUtil(dateIni, dateIni);
        }

        public static DateTime AddHoraUtil(this DateTime date, double prazo)
        {
            var sign = Math.Sign(prazo);
            var unsignedDays = Math.Abs(prazo);
            var dateRef = date;

            for (var i = 0; i < unsignedDays; i++)
            {
                do { dateRef = dateRef.AddHours(sign); }
                while (!DateUnderExpPeriod(dateRef));
            }

            return dateRef;
        }

        public static bool DateUnderExpPeriod(DateTime date)
        {
            var horaIni = date.Date.AddHours(ConstantesCC.HORA_INI_EXP);
            var horaFim = date.Date.AddHours(ConstantesCC.HORA_FIM_EXP);


            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                    {
                        return date >= horaIni && date <= horaFim;
                    }
                    break;
                case DayOfWeek.Friday:
                    {
                        return date >= horaIni && date <= horaFim.AddHours(-2);
                    }
                    break;
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                default:
                    {
                        return false;
                    }
            }
        }

        public static double CountDifHorasUteis(this DateTime dateIni, DateTime dataFim)
        {
            var retorno = 0.0;
            var dataHoraExp = dateIni.Date.AddHours(ConstantesCC.HORA_INI_EXP);
            var dataHoraFim = dateIni.Date.AddHours(ConstantesCC.HORA_FIM_EXP);
            var dateRef = dateIni;
            var dataFimRef = dataFim;

            #region Aguste Data

            #region Aguste DataIni RefUlti
            if (dateRef.DayOfWeek == DayOfWeek.Saturday)
            {
                dateRef = dateRef.Date.AddDays(2).AddHours(ConstantesCC.HORA_INI_EXP);
            }
            else if (dateRef.DayOfWeek == DayOfWeek.Sunday)
            {
                dateRef = dateRef.Date.AddDays(1).AddHours(ConstantesCC.HORA_INI_EXP);
            }
            #endregion

            #region Aguste DataFIm RefUlti

            if (dataFimRef.DayOfWeek == DayOfWeek.Saturday)
            {
                dataFimRef = dataFimRef.Date.AddDays(2).AddHours(ConstantesCC.HORA_INI_EXP + 1);
            }
            else if (dataFimRef.DayOfWeek == DayOfWeek.Sunday)
            {
                dataFimRef = dataFimRef.Date.AddDays(1).AddHours(ConstantesCC.HORA_INI_EXP + 1);
            }
            #endregion


            dataHoraExp = dateRef.Date.AddHours(ConstantesCC.HORA_INI_EXP);
            dataHoraFim = dateRef.Date.AddHours(ConstantesCC.HORA_FIM_EXP);


            if (dateRef < dataHoraExp)
            {
                dateRef = dateRef.Date.AddHours(ConstantesCC.HORA_INI_EXP);
            }
            else if (dateRef > dataHoraFim)
            {
                dateRef = dateRef.Date.AddDays(1).AddHours(ConstantesCC.HORA_INI_EXP);
            }

            if (dataFimRef.TimeOfDay < dataHoraExp.TimeOfDay)
            {
                dataFimRef = dataFimRef.Date.AddHours(ConstantesCC.HORA_INI_EXP + 1);
            }
            else if (dataFimRef.TimeOfDay > dataHoraFim.TimeOfDay)
            {
                dataFimRef = dateRef.Date.AddDays(1).AddHours(ConstantesCC.HORA_INI_EXP + 1);
            }

            #endregion


            while (dateRef < dataFimRef)
            {
                retorno += 1.0;
                do { dateRef = dateRef.AddHours(1); }
                while (!DateUnderExpPeriod(dateRef));
            }


            if (dataFimRef < dateRef)
            {
                dateRef = dateRef.AddHours(-1);
                retorno += dateRef.Subtract(dataFimRef).TotalHours;
            }


            return retorno;
        }

        public static double GetFatorHoraUtil(DateTime? _dateIni = null, DateTime? _dateFim = null)
        {
            var dateIni = (_dateIni ?? DateTime.Now.Date).Date.AddHours(ConstantesCC.HORA_INI_EXP);
            var dateFim = (_dateFim ?? dateIni).Date.AddHours(ConstantesCC.HORA_FIM_EXP);


            switch (dateIni.DayOfWeek)
            {

                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                    {
                        dateIni = dateIni.AddDays(1);
                        dateFim = dateFim;
                    }
                    break;
                case DayOfWeek.Friday:
                    {
                        dateIni = dateIni.AddDays(3);
                        dateFim = dateFim.AddHours(-2);
                    }
                    break;
                case DayOfWeek.Saturday:
                    {
                        dateIni = dateIni.AddDays(3);
                        dateFim = dateFim;
                    }
                    break;
                case DayOfWeek.Sunday:
                    {
                        dateIni = dateIni.AddDays(2);
                        dateFim = dateFim;
                    }
                    break;
            }

            var ret = dateIni.Subtract(dateFim);

            return ret.TotalHours;
        }

        public static DateTime? GetDateByStatus(Issue chamado, StatusRedmine status, bool oldValue = false)
        {
            return CrossCutting.Util.GetDateByStatus(chamado, status, oldValue);
        }

       

        public static List<Rejection> GetRejectionsList(List<Issue> chamados)
        {
            return chamados.Select(
                chamado =>
                    new Rejection()
                    {
                        IdChamado = chamado.Id,
                        Tipo = (TipoChamadoRedmine)chamado.Tracker.Id,
                        Prioridade = (PrioridadeRedmine)chamado.Priority.Id,
                        //HasRejection = ((!(chamado.CustomFields?.Any(cf => cf.Name.Equals("IR") && cf.Values.Any(x => x.Info?.Equals("Sim") ?? false)) ?? false) && chamado.Journals.Any(x => x.Details.Any(y => y.Name == Constantes.Details.Name_Tag && (y.NewValue.Contains(Constantes.Details.Value_Rejeitado)))))),
                        RejectedDate = chamado.Journals?.FirstOrDefault(x => x.Details.Any(y => y.Name == ConstantesCC.Details.Name_Tag && y.NewValue.Contains(ConstantesCC.Details.Value_Rejeitado)))?.CreatedOn ?? null,
                        HasIR = chamado.CustomFields?.Any(cf => cf.Name.Equals("IR") && cf.Values.Any(val => val.Info?.Equals("Sim") ?? false)) ?? false,
                        ClosedOn = chamado.ClosedOn ?? null,
                        CreatedOn = chamado.CreatedOn ?? new DateTime(1900, 1, 1),
                        Projeto = chamado.Project.Name

                    }).ToList();
        }

    }
}
