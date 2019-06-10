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
    public class ComparativeController : BaseApiController
    {
        public object Get(string time )
        {
            var parm = new NameValueCollection(_defaultTipoChamados)
            {
                {RedmineKeys.STATUS_ID, "open"}
            };


            if (!time.IsNullOrWhiteSpace())
            {
                parm.Add(RedmineKeys.PROJECT_ID, String.Format("{0}", time));
            }

            var ret = _RedmineService.GetComparativeObject(parm);

            return ret;

        }


    }
}
