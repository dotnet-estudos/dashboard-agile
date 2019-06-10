using System;
using System.Globalization;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.Services.Enums;

namespace DashBoardAgil.Services.Domain
{
    public class Chamado : Redmine.Net.Api.Types.Issue
    {

        public bool Under { get; set; }
        public  double HourTo { get; set; }
        public string Semaforo { get; set; }
        public string Mensagem { get; set; }
        public string DataCriacao => ((DateTime) (this.CreatedOn)).ToString("d");
        public string StatusRedmine => ((StatusRedmine)Status.Id).GetDisplayName();
        public Versao Versao { get; set; }
        public virtual string TimeTo
        {
            get
            {
                var date = DateTime.Now;
                return string.Format("{0} (Total Horas: {1})", date.AddHours(HourTo).Subtract(date).ToString(@"dd\:hh\:mm"), HourTo);

            }
        } 
    }

   
}