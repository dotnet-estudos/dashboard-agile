using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DashBoardAgil.Domain.Interfaces.Abstracoes;
using DashBoardAgil.CrossCutting.Enums;

namespace DashBoardAgil.Services.Domain
{
    public class CreatedClosed : IdentificableIssue
    {
        public Flag Flag { get; set; }
    }
}
