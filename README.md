# Dashboard para Metricas Ageis

Projeto para desenvolvimento de um dashboard de metricas ageis para ser usado pelas coordenações da DIS

## Servicos Disponiveis:

1. __SLA: 			GET__ 	"http://dashboardagil.example.com/api/SlaControl?time=time-macaco-prego&IdSla=1&tempo=12&tipoContagem=2&util=true&dataIni=2018-11-01&dataFim=2018-11-30" 
  *Pegar Sla por Time*
 
2. __DIADEV: 		GET__ 	"http://dashboardagil.example.com/api/DiaDev?time=0&dataIni=2018-11-01&dataFim=2018-11-30" 
  *Pegar dia desenvolvedor por time*
 
3. __SLA:    		GET__ 	"http://dashboardagil.example.com/api/SlaControl?time=&tempo=12&tipoContagem=2&util=true&dataIni=2018-11-01&dataFim=2018-11-30&redemineDe=220&redeminePara=220" 
  *Pegar Sla todos Times*
 
4. __AGENT:  		PUT__      "http://dashboardagil.example.com/api/Agente/99" 
  *Habilitar/Desabilitar o Agente*
 
5. __Prazo Chamado: 	GET__   "http://dashboardagil.example.com/api/PrazoChamado/?time=time-traiket&dataIni=2018-12-20&dataFim=2018-12-30&efetiva=false&status=118" 
  *Verificar se os chamados estão dentro do prazo estabelecido na data da Pacote*
  
6. __CicleTime/LeadTime : 	GET__   "http://dashboardagil.example.com/api/CicleTime?time=&prioridade=&tipoChamado=&statusDe=120&statusPara=119&dataIni=2019-02-20&dataFim=2019-03-07&primeiroSalto=10&quantidadeSaltos=10" 
  *Get cicleTime e LeadTime*

7. __Taxa de Rejeição : 	GET__   "http://dashboardagil.example.com/api/Rejections?time=time-traiket&dataIni=2018-12-11&dataFim=2019-03-11" 
  *Pega Taxa de rejeicao*  

8. __Throughput : 	GET__   "http://dashboardagil.example.com/api/Throughput?time=time-traiket&dataIni=2019-01-11&dataFim=2019-03-11" 
  *Throughput por periodo*  
  

### Prerequisites

Visual Studio 2015
morris.js (chart)
jquerydatatable.js


## Deployment

URL - dashboardagil.example.com
FTP - example.com
Usuário - homdashagil
Senha - Hom5Db4

## Built With

* [morris](http://morrisjs.github.io/morris.js/index.html) - Chart
* [jquerydatatable](https://datatables.net/) - Dependency Management
* [MVC5](https://www.devmedia.com.br/guia/asp-net-mvc/38190) - asnet.MVC
* [highcharts](https://highcharts.com) - Highcharts.js

