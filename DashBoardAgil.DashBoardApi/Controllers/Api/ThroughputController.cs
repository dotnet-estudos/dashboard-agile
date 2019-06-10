using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using Redmine.Net.Api;

namespace DashBoardAgil.DashBoardApi.Controllers.Api
{
    public class ThroughputController : BaseApiController
    {
        public Object Get(string time, DateTime dataIni, DateTime dataFim)
        {
            var parm = new NameValueCollection(_defaultTipoChamados);

            parm.Add(RedmineKeys.STATUS_ID, "*");
            parm.Add(RedmineKeys.CLOSED_ON, String.Format("><{0}|{1}", dataIni.ToString("yyyy-MM-dd"), dataFim.ToString("yyyy-MM-dd")));

            if (!time.IsNullOrWhiteSpace())
            {
                parm.Add(RedmineKeys.PROJECT_ID, String.Format("{0}", time));
            }

            var ret = _RedmineService.GetThroughputRate(parm);

            return ret;

        }
    }
}
