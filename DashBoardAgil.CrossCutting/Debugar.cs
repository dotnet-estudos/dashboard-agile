using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardAgil.CrossCutting
{
    public sealed class Debugar
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mensage"></param>
        /// <param name="categoria"></param>
        public static void WriteLine(string mensage, string categoria=null)
        {
            Global.Instance.GravaLog(categoria??"Debug ", mensage);
            Debug.WriteLine(mensage,categoria);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="categoria"></param>
        public static void WriteLine(object objeto, string categoria = null)
        {
            var mensage = string.Format("type: {0} ToString: {1}", objeto?.GetType(), objeto?.ToString());

            Debugar.WriteLine(mensage, categoria);

        }
    }
}
