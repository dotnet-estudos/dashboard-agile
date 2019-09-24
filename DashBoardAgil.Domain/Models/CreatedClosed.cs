using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DashBoardAgil.Services.Enums;
using DashBoardAgil.Services.Interfaces.Abstracoes;

namespace DashBoardAgil.Services.Domain
{
    public class CreatedClosed : IdentificableIssue
    {
        public Flag Flag { get; set; }
    }
}
