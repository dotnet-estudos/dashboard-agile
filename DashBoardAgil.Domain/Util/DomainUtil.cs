using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DashBoardAgil.Domain.Enums;

namespace DashBoardAgil.Domain.Util
{
    public static class DomainUtil
    {
        public static string GetRgbRgbaByTipoChamado(TipoChamadoRedmine tipo, bool rgba, double transparencia = 0.5)
        {
            var formato = "{0},{1},{2}," + (rgba ? transparencia.ToString(CultureInfo.CreateSpecificCulture("en-GB")) : "1.0");
            var retorno = "";
            switch (tipo)
            {
                case TipoChamadoRedmine.Adaptativa:
                case TipoChamadoRedmine.FiplanAdaptativa:
                    {
                        retorno += string.Format(formato, 255, 102, 0);
                    }
                    break;
                case TipoChamadoRedmine.Consulta:
                /*case TipoChamadoRedmine.FiplanConsulta:
                    {
                        retorno += string.Format(formato, 140, 140, 140);
                    }
                    break;*/
                case TipoChamadoRedmine.Corretiva:
                case TipoChamadoRedmine.FiplanCorretiva:
                    {
                        retorno += string.Format(formato, 0, 153, 0);
                    }
                    break;
                case TipoChamadoRedmine.Evolutiva:
                case TipoChamadoRedmine.FiplanEvolutiva:
                    {
                        retorno += string.Format(formato, 255, 255, 0);
                    }
                    break;
            }

            return retorno;
        }
    }
}
