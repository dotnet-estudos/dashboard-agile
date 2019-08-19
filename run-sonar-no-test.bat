@echo off
CLS
@echo.
@echo    ***********************************************
@echo    ***                                         ***
@echo    ***    EXECUCAO DO SONAR. PROJETO SIGANT.   ***
@echo    ***                                         ***
@echo    ***********************************************
@echo.

@echo.
@echo    ***    BAIXANDO DEPENDENCIAS     ***
.nuget\NuGet.exe restore

@echo.
@echo    ***    EXECUTANDO SONAR          ***

SonarScanner.MSBuild.exe begin /k:"br.gov.ba.dashboardagil" /n:"DashBoard Agil" /v:"1.00" /s:"%~dp0SonarQube.Analysis.Base.xml" /d:sonar-verbose=true

"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild".exe /m /t:Rebuild /nodereuse:false
	
SonarScanner.MSBuild.exe end
