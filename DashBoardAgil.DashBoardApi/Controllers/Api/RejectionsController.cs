using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.DashBoardApi.Models;
using DashBoardAgil.Services.Domain;
using DashBoardAgil.Services.Enums;
using DashBoardAgil.Services.Util;
using Redmine.Net.Api;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using WebApi.OutputCache.V2;

namespace DashBoardAgil.DashBoardApi.Controllers.Api
{
    public class RejectionsController : BaseApiController
    {

        [CacheOutput(ServerTimeSpan = 1 * (60))]
        public Object Get(string time, DateTime dataIni, DateTime dataFim, bool throughput=false)
        {
            var parm = new NameValueCollection(_defaultTipoChamados)
            {
                {RedmineKeys.STATUS_ID, throughput? "closed": "*"},
                {
                    RedmineKeys.CREATED_ON,
                    String.Format("><{0}|{1}", ((DateTime)dataIni).ToString("yyyy-MM-dd"), ((DateTime)dataFim).ToString("yyyy-MM-dd"))
                }
            };


            if (!time.IsNullOrWhiteSpace())
            {
                parm.Add(RedmineKeys.PROJECT_ID, String.Format("{0}", time));
            }

            var ret = throughput? JsonConvert.DeserializeObject(Global.Instance.LerArquivo(Path.Combine(ConstantesCC.PATH_THROUGHPUT_REJECT_DIAGRAM, String.Format("{0}.json", time)))) :
                        _RedmineService.GetRejectionRate(parm);
            
            return ret;
        }
    }
}
