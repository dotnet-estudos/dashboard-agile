using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoardAgil.DashBoardApi.Models
{
    public class DivLoading
    {
        public DivLoading()
        {
            FaIcon = "fa-cog";
            TamanhoIcon = 3;
        }
        public string Id { get; set; }
        public string  FaIcon { get; set; }
        public int TamanhoIcon { get; set; }
    }
}

/*
 * 
 * 
 * <div class="row">
    <div class="text-center" id="_LoadingSumary"> <i class="fa fa-cog fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span> </div>
</div>
*/