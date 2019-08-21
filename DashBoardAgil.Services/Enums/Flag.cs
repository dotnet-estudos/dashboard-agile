using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardAgil.Services.Enums
{
    public enum Flag
    {
        [Display(Name = "created")]
        Created = 0,
        [Display(Name = "closed")]
        Closed = 1,
    }
}
