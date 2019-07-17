using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using DashBoardAgil.Services.Domain;
using DashBoardAgil.Services.Enums;
using Redmine.Net.Api.Types;

namespace DashBoardAgil.Services.Interfaces
{
	public interface IRedmineService
	{
		List<Issue> GetObjectsById(string[] ids);
		List<Issue> GetObjectsByProjects(string[] projectIds, NameValueCollection parameters);
		string GetChamado(string id);
	    IEnumerable<string> FecharChamadosAvaliarProducao(string[] projetos, int prazo);
        bool GerarCfdTeams();
        bool GerarThroughputReject();
        TimeSpan? LeadTime();
	    SlaControl GetSla(Projeto time, SlaProjeto slaProjeto, NameValueCollection chamados);
	    PrazoControl GetPrazo(NameValueCollection parms, bool efetiva, StatusRedmine statusRedmine);
	    LeadTimeControl GetCicleTimeTeam(NameValueCollection parms, StatusRedmine statusDe, StatusRedmine statusPara, int primeiroSalto, int quantidadeSaltos, bool leadtime=false);
        RejectionControl GetRejectionRate(NameValueCollection parms);
        ThroughputControl GetThroughputRate(NameValueCollection parms);
        ConsumoHorasControl ConsumptionHoursByProject(NameValueCollection parms, string project, string time);
        object GetComparativeObject(NameValueCollection parms);
        object GetCumulativeFlowDiagram(NameValueCollection parms, List<int> weeks);
        




	}
}
