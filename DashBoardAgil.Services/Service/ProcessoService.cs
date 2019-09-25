using DashBoardAgil.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DashBoardAgil.InfraData.Contexto;

namespace DashBoardAgil.Services.Service
{
    public static class ProcessoService
    {
        public static Contexto Contexto;

        static ProcessoService()
        {
            Contexto = new Contexto();
        }
        public static IEnumerable<Processo> listarProcessos()
        {
            return new Contexto().Processos.ToList();
        }

        public static int salvarProcesso(Processo Processo)
        {
            Contexto.Processos.Add(Processo);
            return Contexto.SaveChanges();
        }

        public static Processo carregarPorId(int id)
        {
            return Contexto.Processos.Where(P => P.ProcessoId == id).FirstOrDefault();
        }
    }
}
