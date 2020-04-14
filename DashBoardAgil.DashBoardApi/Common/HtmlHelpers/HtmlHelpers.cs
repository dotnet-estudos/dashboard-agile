using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;
using DashBoardAgil.CrossCutting;

namespace DashBoardAgil.DashBoardApi.Common.HtmlHelpers
{
    public static class HtmlHelpers
    {
        /// <summary>
        /// Botão para exibição de modal.
        /// </summary>
        /// <param name="ajaxHelper">Extensão.</param>
        /// <param name="modalButtonParameters">Parâmetro do ModalButton.</param>
        /// <returns></returns>
        public static MvcHtmlString ModalButton(this AjaxHelper ajaxHelper, ModalButtonParameters modalButtonParameters)
        {
            AjaxOptions ajaxOptions = new AjaxOptions
            {
                HttpMethod = "get",
                UpdateTargetId = "ModalContent",
                LoadingElementId = "divCarregando",
                OnBegin = modalButtonParameters.OnBegin,
                OnSuccess = "comprasnet.showModal('" + modalButtonParameters.ModalTitle + "'," + modalButtonParameters.Responsive.ToString().ToLower(CultureInfo.InvariantCulture) + ", null, '" + modalButtonParameters.ModalWidth + "', '" + modalButtonParameters.Modalheight + "')",
                InsertionMode = InsertionMode.Replace
            };

            return ajaxHelper.ActionLink(modalButtonParameters.LinkText ?? " ", modalButtonParameters.ActionName, modalButtonParameters.ControllerName, modalButtonParameters.RouteValues, ajaxOptions, modalButtonParameters.HtmlAttributes);
        }

        /// <summary>
        /// Gera um elemento a (link) com a formatação padrão do botão de imprimir e abre em uma nova aba (target _blank)
        /// </summary>
        /// <param name="printTitle">Título da página de impressão</param>
        /// <param name="action">Action</param>
        /// <param name="routeValues">Parãmetros a serem passados para o controller</param>
        /// <returns></returns>
        public static MvcHtmlString PrintButton(this HtmlHelper htmlHelper, string printTitle, string action, object routeValues)
        {
            TagBuilder tagBuilder;
            UrlHelper urlHelper;
            var routeValuesLocal = routeValues as IDictionary<string, object> ?? new RouteValueDictionary(routeValues);
            urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            tagBuilder = new TagBuilder("a");

            tagBuilder.InnerHtml = "Imprimir";
            tagBuilder.AddCssClass("btn btn-default yellow medium");
            tagBuilder.Attributes.Add("target", "_blank");
            routeValuesLocal.Add("PrintTitle", printTitle);

            tagBuilder.Attributes["href"] = urlHelper.Action(action, new RouteValueDictionary(routeValuesLocal));
            return MvcHtmlString.Create(tagBuilder.ToString());
        }

        public static IHtmlString RenderScripts(this HtmlHelper htmlHelper)
        {
            var scripts = htmlHelper.ViewContext.HttpContext.Items["Scripts"] as IList<string>;
            string scriptsAtual = String.Empty;
            if (scripts != null)
            {
                var builder = new StringBuilder();
                foreach (var script in scripts)
                {
                    if (!script.Equals(scriptsAtual))
                    {
                        scriptsAtual = script;
                        builder.AppendLine(script);
                    }

                }
                return new MvcHtmlString(builder.ToString());
            }
            return null;
        }

        /// <summary>
        /// Método para gerar grid Jquery DataTable com paginação lado cliente ou servidor.
        /// https://datatables.net/
        /// </summary>
        /// <param name="htmlHelper">Extensão.</param>
        /// <param name="gridParameters">Parâmetros da Grid.</param>
        /// <returns></returns>
        public static IHtmlString RenderDataTable(this HtmlHelper htmlHelper, GridParameters gridParameters)
        {
            ValidarDataTable(gridParameters);

            bool runServerSide = ((gridParameters.Columns != null && gridParameters.Columns.Count > 0));

            string filtros = string.Empty;
            if (gridParameters.Filters != null)
            {
                filtros = gridParameters.Filters.Aggregate(filtros, (current, filter) => current + ("&" + filter.Name + "=" + filter.Value));
            }
            string urlAjaxCall = " url: '/" + gridParameters.Controller + "/PageDataDataTable?idTable=" + gridParameters.IdTable + filtros + "',";

            StringBuilder sb = new StringBuilder();

            sb.Append(" <script>");
            sb.Append("            var " + gridParameters.IdTable + ";");
            sb.Append("   $(function () {");
            sb.Append("       " + gridParameters.IdTable + " = {");
            sb.Append("          load: function () {");
            sb.Append("             $('#" + gridParameters.IdTable + "').closest('div').css('visibility','visible');     ");
            sb.Append("             if (!$.fn.DataTable.isDataTable('#" + gridParameters.IdTable + "')) {");
            sb.Append("                var table = $('#" + gridParameters.IdTable + "').dataTable({");
            sb.Append("                    paging: " + gridParameters.Paging.ToString().ToLower(CultureInfo.InvariantCulture) + ",");
            sb.Append("                    pageLength: " + gridParameters.PageLength + ",");
            sb.Append("                    ordering: " + gridParameters.Ordering.ToString().ToLower(CultureInfo.InvariantCulture) + ",");
            sb.Append("                    info: true,");
            sb.Append("                    displayStart: 0,");
            sb.Append("                    lengthChange: false,");
            sb.Append("                    searching: false,");
            sb.Append("                    processing: " + runServerSide.ToString().ToLower(CultureInfo.InvariantCulture) + ",");
            sb.Append("                    serverSide: " + runServerSide.ToString().ToLower(CultureInfo.InvariantCulture) + ",");

            if (gridParameters.OrderDefault != null)
            {
                sb.Append("                    order: [[" + gridParameters.OrderDefault.IndexColumn + ", '" + gridParameters.OrderDefault.Direction.GetDisplayName() + "']],");
            }

            StringBuilder compImprimirTodos = new StringBuilder();

            if (runServerSide)
            {
                sb.Append("                    ajax: $.fn.dataTable.pipeline({");
                sb.Append(urlAjaxCall);
                sb.Append("                          pages: " + gridParameters.CachePageLength + ",");
                sb.Append("                          sendFormData: " + (gridParameters.SendFormData ? "true" : "false"));
                sb.Append("                    }),");

                CarregarColunas(gridParameters, sb, compImprimirTodos);
            }

            sb.Append("                    initComplete: function (oSettings, json) {");
            sb.Append("                        $('#" + gridParameters.IdTable + "').css('width', '100%');");
            sb.Append("                        $('#" + gridParameters.IdTable + "').css('font-size', 'small');");
            sb.Append("                    },");


            if (gridParameters.IsVisibleExcelButton || gridParameters.IsVisiblePrintButton)
            {
                CarregarBotao(gridParameters, sb, runServerSide);
            }
           

            sb.Append("                    language: {");
            sb.Append("                        'sEmptyTable': '" + gridParameters.EmptyTextTable + "',");
            sb.Append("                        'sInfo': 'Mostrando de _START_ até _END_ de _TOTAL_ registros',");
            sb.Append("                        'sInfoEmpty': 'Mostrando 0 até 0 de 0 registros',");
            sb.Append("                        'sInfoFiltered': '',");
            sb.Append("                        'sInfoPostFix': '',");
            sb.Append("                        'sInfoThousands': '.',");
            sb.Append("                        'sLengthMenu': '_MENU_ resultados por página',");
            sb.Append("                        'sLoadingRecords': 'Carregando...',");
            sb.Append("                        'sProcessing': '<div class=\\'progresso\\' style=\\'display: block;\\'><div id=\\'overlay\\'></div><img src=\\'/Content/img/loading.gif\\'><br>Carregando...</div>',");
            sb.Append("                        'sZeroRecords': '" + gridParameters.EmptyTextTable + "',");
            sb.Append("                        'sSearch': 'Pesquisar',");
            sb.Append("                        'oPaginate': {");
            sb.Append("                            'sNext': 'Próximo',");
            sb.Append("                            'sPrevious': 'Anterior',");
            sb.Append("                            'sFirst': 'Primeiro',");
            sb.Append("                            'sLast': 'Último'");
            sb.Append("                        },");
            sb.Append("                        'oAria': {");
            sb.Append("                            'sSortAscending': ': Ordenar colunas de forma ascendente',");
            sb.Append("                            'sSortDescending': ': Ordenar colunas de forma descendente'");
            sb.Append("                        }");
            sb.Append("                    }");
            sb.Append("                });");
            sb.Append(gridParameters.IsVisiblePrintButton ? " $('#" + gridParameters.IdTable + "').DataTable().buttons().container().appendTo($('#" + gridParameters.WrapperIdBtnImprimir + "').first());" : string.Empty);
            sb.Append("        } ");

            if (runServerSide)
            {
                sb.Append("              else { var tblSublist = $('#" + gridParameters.IdTable + "').DataTable();");
                sb.Append("                   tblSublist.clearPipeline(); tblSublist.page(0);");
                sb.Append("                   tblSublist.ajax.url(");
                sb.Append("                            $.fn.dataTable.pipeline ({");
                sb.Append(urlAjaxCall);
                sb.Append("                               pages: 5,");
                sb.Append("                               sendFormData: " + (gridParameters.SendFormData ? "true" : "false"));
                sb.Append("                            })");
                sb.Append("                   ).load(null, false);");
                sb.Append("              }");
            }
            sb.Append(" return false; }");
            sb.Append("     }");
            sb.Append(" });");

            if (runServerSide && gridParameters.IsVisiblePrintButton)
            {
                sb.Append("        function ImprimirTodos" + gridParameters.IdTable + "($table) {");
                sb.Append("            var retorno = false;");
                sb.Append("            var dataTable = $('#" + gridParameters.IdTable + "').DataTable(); ");             
                sb.Append("            var colIndexOrder = dataTable.order()[0][0];");
                sb.Append("            var sortName = dataTable.settings().init().columns[colIndexOrder].name;");
                sb.Append("            var sortDirection = dataTable.order()[0][1];");
                sb.Append("            $.ajax({");
                sb.Append("                url: '/" + gridParameters.Controller + "/Imprimir?idTable=" + gridParameters.IdTable + "&sortName=' + sortName + '&sortDirection=' + sortDirection + '&' + $('form').serialize(),");
                sb.Append("                type: 'GET',");
                sb.Append("                contentType: 'application/json; charset=utf-8',");
                sb.Append("                dataType: 'json',");
                sb.Append("                async: false,");
                sb.Append("                success: function (data) {");
                sb.Append("                    var rows = '';");
                sb.Append("                    $.each(data, function (index, item) {");
                sb.Append("                        rows += '<tr>" + compImprimirTodos + "</tr>';");
                sb.Append("                    });");
                sb.Append("                    $table.find('tbody').html(rows);");           
                sb.Append("                    retorno = true;");
                sb.Append("                },");
                sb.Append("                error: function (result) {");
                sb.Append("                    if (result.responseText.indexOf('maxJsonLength') > 0) {");
                sb.Append("                        comprasnet.alertErro('Erro ao Imprimir. Consulta muito Extensa.');");
                sb.Append("                    } else {");
                sb.Append("                        comprasnet.alertErro('Erro ao Imprimir.');");
                sb.Append("                    }");
                sb.Append("                    retorno = false;");
                sb.Append("                }");
                sb.Append("            });");
                sb.Append("            return retorno;");
                sb.Append("        }");
            }

            if (runServerSide && gridParameters.IsVisibleExcelButton)
            {
                string propItemArray = "[";
                int loop = 1;
                foreach (Columns column in gridParameters.Columns)
                {
                    propItemArray += "item." + column.DataFieldAlias+(gridParameters.Columns.Count > loop ? "," : "];");                    
                    loop++;
                }
                sb.Append("        function ExportarTodos" + gridParameters.IdTable + "($table) {");
                sb.Append("            var retorno = null;");
                sb.Append("            var dataTable = $('#" + gridParameters.IdTable + "').DataTable(); ");
                sb.Append("            var colIndexOrder = dataTable.order()[0][0];");
                sb.Append("            var sortName = dataTable.settings().init().columns[colIndexOrder].name;");
                sb.Append("            var sortDirection = dataTable.order()[0][1];");
                sb.Append("            $.ajax({");
                sb.Append("                url: '/" + gridParameters.Controller + "/Imprimir?idTable=" + gridParameters.IdTable + "&sortName=' + sortName + '&sortDirection=' + sortDirection + '&' + $('form').serialize(),");
                sb.Append("                type: 'GET',");
                sb.Append("                contentType: 'application/json; charset=utf-8',");
                sb.Append("                dataType: 'json',");
                sb.Append("                async: false,");
                sb.Append("                success: function (data) {");          
                sb.Append("                    var dados = new Array();");
                sb.Append("                    $.each(data, function (index, item) {");
                sb.Append("                        dados[index] = "+ propItemArray);
                sb.Append("                    });");
                sb.Append("                    retorno = dados;");
                sb.Append("                },");
                sb.Append("                error: function (result) {");
                sb.Append("                    if (result.responseText.indexOf('maxJsonLength') > 0) {");
                sb.Append("                        comprasnet.alertErro('Erro ao Exportar. Consulta muito Extensa.');");
                sb.Append("                    } else {");
                sb.Append("                        comprasnet.alertErro('Erro ao Exportar.');");
                sb.Append("                    }");
                sb.Append("                    retorno = null;");
                sb.Append("                }");
                sb.Append("            });");
                sb.Append("            return retorno;");
                sb.Append("        }");
            }
            if (gridParameters.AutoLoad)
            {
                sb.Append("  $(document).ready(function () {");
                sb.Append("       " + gridParameters.IdTable + ".load();");
                sb.Append("  });");
            }

            sb.Append("</script>");

            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        /// Método privado que Valida os Parâmetros do DataTable.
        /// </summary>
        /// <param name="gridParameters">Parâmetros do DataTable.</param>
        private static void ValidarDataTable(GridParameters gridParameters)
        {
            if ((gridParameters == null) || string.IsNullOrEmpty(gridParameters.IdTable))
            {
                throw new ArgumentNullException("gridParameters", "Favor informar o gridParameters e gridParameters gridParameters.IdTable.");
            }
        }

        /// <summary>
        /// Método privado para Carregar lógica do botão Imprimir.
        /// </summary>
        /// <param name="gridParameters">Parâmetros da Grid.</param>
        /// <param name="sb">StringBuilder Principal.</param>
        /// <param name="runServerSide">Roda no Lado Servidor (true/false).</param>
        private static void CarregarBotao(GridParameters gridParameters, StringBuilder sb, bool runServerSide)
        {
            sb.Append("                    buttons: [");
            if (gridParameters.IsVisibleExcelButton)
            {
                sb.Append("                      { extend: 'excelHtml5',  text: 'Exportar Excel', className: 'btn btn-default yellow medium', ");
                sb.Append("                        customizeData: function (data) { ");
                sb.Append(runServerSide ? "            var retorno = ExportarTodos" + gridParameters.IdTable + "(table);  data.body = retorno;" : string.Empty);
                sb.Append("                             return data;");
                sb.Append("                        }");
                sb.Append("                      }" + (gridParameters.IsVisiblePrintButton ? "," : ""));
            }

            if (gridParameters.IsVisiblePrintButton)
            {
                sb.Append("                      {   extend: 'print',");
                sb.Append("                            text: 'Imprimir',");
                sb.Append("                            className: 'btn btn-default yellow medium',");
                sb.Append("                            autoPrint: true,");
                sb.Append("                        customize: function (win) {");
                sb.Append("                            var table = $(win.document.body).find('table'); console.log(table);");
                sb.Append("                            var footer = $('.logomarca').clone();");
                sb.Append(runServerSide ? " var retorno = ImprimirTodos" + gridParameters.IdTable + "(table);  if (retorno === true) { " : string.Empty);
                sb.Append("                                var filtro = $('#" + gridParameters.WrapperIdFiltroImprimir + "').clone();");
                //sb.Append("                                filtro.addClass('display').css('font-size', '10px');");
                sb.Append("                                filtro.removeClass('visible-print');");
                sb.Append("                                filtro.insertBefore(table);");
                //sb.Append("                                table.addClass('display').css('font-size', '10px');");
                sb.Append("                                $(win.document.body).find('h1').css({'text-align': 'center', 'font-size':'" + gridParameters.PrintPageTitleSize + "'});");
                sb.Append(!String.IsNullOrWhiteSpace(gridParameters.PrintPageCustomTitle) ? "$(win.document.body).find('h1').text('" + gridParameters.PrintPageCustomTitle + "');" : string.Empty);
                sb.Append("                                footer.css('margin', '20px');");
                sb.Append("                                footer.find('img').first().attr('src', document.location.protocol + '//' + document.location.host + '/Content/img/brasao.png');");

                sb.Append("                                var tableTotalReg = $('<div>Total de Registros: '+ table.find('tbody tr').length + '</div>').css('font-size', '9px').insertAfter(table);");

                sb.Append("                                var customFooter = $('#" + gridParameters.WrapperIdRodapeImprimir + "').clone();");
                sb.Append("                                customFooter.addClass('display').css('font-size', '10px');");
                sb.Append("                                customFooter.removeClass('visible-print');");
                sb.Append("                                customFooter.insertAfter(tableTotalReg); ");

                sb.Append("                                if (customFooter.length > 0) { footer.insertAfter(customFooter); }else{ footer.insertAfter(tableTotalReg); }");
                sb.Append(runServerSide ? "  } else { win.close(); } " : string.Empty);
                sb.Append("                      }");
                sb.Append("                     }");
            }
            sb.Append("                    ],");           
            sb.Append("                    dom: 'Bfrtip',");
        }

        /// <summary>
        /// Método privado para Carregar as Colunas na Grid.
        /// </summary>
        /// <param name="gridParameters">Parâmetros da Grid.</param>
        /// <param name="sb">StringBuilder Principal.</param>
        /// <param name="compImprimirTodos">StringBuilder Botão Imprimir.</param>
        private static void CarregarColunas(GridParameters gridParameters, StringBuilder sb, StringBuilder compImprimirTodos)
        {
            StringBuilder compColumns = new StringBuilder();
            StringBuilder compCreatedRow = new StringBuilder();

            int indexColuna = 0;
            foreach (Columns column in gridParameters.Columns)
            {
                CarregarColuna(column, compColumns, compCreatedRow, compImprimirTodos, indexColuna);
                indexColuna++;
            }

            sb.Append("  columns: [ " + compColumns + " ],");
            sb.Append("                    createdRow: function (row, data, index) {");
            sb.Append("                        $(row).addClass('row');");
            sb.Append("                        $('#" + gridParameters.IdTable + " th').each(function (index) {");
            sb.Append("                            var $td = $('td', row).eq(index);");
            sb.Append("                            $td.addClass($(this).attr('class'));");
            sb.Append("                        });");
            sb.Append("                        var linkModal = null;");
            sb.Append("                        var column = null;");
            sb.Append(compCreatedRow);
            sb.Append("                    },");
        }

        /// <summary>
        /// Método privado para Carregar a Coluna na Grid.
        /// </summary>
        /// <param name="column">Coluna Atual.</param>
        /// <param name="compColumns">StringBuilder Colunas.</param>
        /// <param name="compCreatedRow">StringBuilder Evento CreatedRow.</param>
        /// <param name="compImprimirTodos">StringBuilder Botão Imprimir.</param>
        /// <param name="indexColuna">Index da Coluna Atual.</param>
        private static void CarregarColuna(Columns column, StringBuilder compColumns, StringBuilder compCreatedRow, StringBuilder compImprimirTodos, int indexColuna)
        {
            switch (column.TypeColumn)
            {
                case TypeColumn.Normal:
                    compColumns.Append("                        {");
                    compColumns.Append("                            data: '" + DashBoardAgil.CrossCutting.Util.PrimeiroCaracterMinusculo(column.DataFieldAlias) + "',");
                    compColumns.Append("                            name: '" + DashBoardAgil.CrossCutting.Util.PrimeiroCaracterMinusculo(column.DataField) + "',");
                    compColumns.Append("                            searchable: " + column.Searchable.ToString().ToLower(CultureInfo.InvariantCulture) + ",");
                    compColumns.Append("                            sortable: " + column.Sortable.ToString().ToLower(CultureInfo.InvariantCulture) + ",");
                    compColumns.Append("                            className: '" + column.ClassName + "',");
                    compColumns.Append("                        },");

                    compImprimirTodos.Append("<td>' + item." + column.DataFieldAlias + " + '</td>");

                    break;
                case TypeColumn.CheckBox:
                    compColumns.Append("                        {");
                    compColumns.Append("                            data: null,");
                    compColumns.Append("                            name: null,");
                    compColumns.Append("                            searchable: false,");
                    compColumns.Append("                            sortable: false,");
                    compColumns.Append("                            className: '" + column.ClassName + "',");
                    compColumns.Append("                            defaultContent: '<input type=\\'checkbox\\'>'");
                    compColumns.Append("                        }, ");

                    compCreatedRow.Append("                        var $td = $('td', row).eq(" + indexColuna + ");");
                    compCreatedRow.Append("                        $td.find('input[type=\\'checkbox\\']').attr('id', data." + DashBoardAgil.CrossCutting.Util.PrimeiroCaracterMinusculo(column.DataField) + ");");
                    compCreatedRow.Append("                        $td.find('input[type=\\'checkbox\\']').val(data." + DashBoardAgil.CrossCutting.Util.PrimeiroCaracterMinusculo(column.DataField) + ");");
                    compCreatedRow.Append("                        $td.find('input[type=\\'checkbox\\']').attr('name', '" + DashBoardAgil.CrossCutting.Util.PrimeiroCaracterMinusculo(column.DataField) + "' );");

                    compImprimirTodos.Append("<td></td>");
                    break;
                case TypeColumn.Links:
                    compColumns.Append("                        {");
                    compColumns.Append("                            data: null,");
                    compColumns.Append("                            name: " + (!string.IsNullOrEmpty(column.DataField) ? "'" + DashBoardAgil.CrossCutting.Util.PrimeiroCaracterMinusculo(column.DataField) + "'," : "null,"));
                    compColumns.Append("                            searchable: " + column.Searchable.ToString().ToLower(CultureInfo.InvariantCulture) + ",");
                    compColumns.Append("                            sortable: " + column.Sortable.ToString().ToLower(CultureInfo.InvariantCulture) + ",");
                    compColumns.Append("                            className: '" + column.ClassName + "',");
                    compColumns.Append("                            defaultContent: ''");
                    compColumns.Append("                        }, ");

                    compImprimirTodos.Append(!string.IsNullOrEmpty(column.DataFieldAlias) ? "<td>' + item." + column.DataFieldAlias + " + '</td>" : "<td></td>");

                    CarregarLinks(compCreatedRow, column, indexColuna);
                    break;
            }
        }

        /// <summary>
        /// Método privado para Carregar os Links em colunas na Grid.
        /// </summary>
        /// <param name="column">Coluna Atual.</param>
        /// <param name="compCreatedRow">String Builer da Criação da Linha.</param>
        /// <param name="indexColuna">Coluna Atual.</param>
        private static void CarregarLinks(StringBuilder compCreatedRow, Columns column, int indexColuna)
        {
            if (column.Links != null)
            {
                foreach (Link link in column.Links)
                {
                    CarregarLink(link, compCreatedRow, indexColuna);
                }
            }
        }

        /// <summary>
        /// Método privado para Carregar o Link em colunas na Grid.
        /// </summary>
        /// <param name="link">Link Atual.</param>
        /// <param name="compCreatedRow">String Builer da Criação da Linha.</param>
        /// <param name="indexColuna">Coluna Atual.</param>
        private static void CarregarLink(Link link, StringBuilder compCreatedRow, int indexColuna)
        {
            if (!string.IsNullOrEmpty(link.VisibleIf)) compCreatedRow.Append("if (data." + DashBoardAgil.CrossCutting.Util.PrimeiroCaracterMinusculo(link.VisibleIf) + " === true) {");

            string nameLink = GetNameLink(link);
            string parameter = GetParameter(link);

            if (link.TypeLink.Equals(TypeLink.Modal) || link.TypeLink.Equals(TypeLink.Remove))
            {
                AppendLinkModal(link, compCreatedRow, nameLink, parameter);
            }
            else
            {
                AppendLinkNaoModal(link, compCreatedRow, nameLink, parameter);
            }

            compCreatedRow.Append("                        column = $('td', row).eq(" + indexColuna + ");");
            compCreatedRow.Append("                        column.html(column.html() + linkModal);");
            if (!string.IsNullOrEmpty(link.VisibleIf)) compCreatedRow.Append("}");
        }

        /// <summary>
        /// Obtém o nome do link.
        /// </summary>
        /// <param name="link">Link Atual.</param>
        /// <returns></returns>
        private static string GetNameLink(Link link)
        {
            return link.IsIcon ? string.Empty : "' + data." + DashBoardAgil.CrossCutting.Util.PrimeiroCaracterMinusculo(link.DataFieldText) + " + '";
        }

        /// <summary>
        /// Obtém o nome do Parâmetro.
        /// </summary>
        /// <param name="link">Link Atual.</param>
        /// <returns></returns>
        private static string GetParameter(Link link)
        {
            string retorno = string.Empty;
            if (link.ParametersNamesAndValuesDataField != null && link.ParametersNamesAndValuesDataField.Count > 0)
            {
                foreach (var key in link.ParametersNamesAndValuesDataField.Keys)
                {
                    retorno += key + "=' + " + GetValueDataId(link.ParametersNamesAndValuesDataField[key]) + " + '&";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(link.ParameterName))
                {
                    retorno = link.ParameterName + "=' + " + GetValueDataId(link.ParameterValueDataField) + " + '";
                }
            }
            return retorno;
        }

        /// <summary>
        /// Obtém o Valor do ProjetoId Principal da Grid.
        /// </summary>
        /// <param name="link">Link Atual.</param>
        /// <returns></returns>
        private static string GetValueDataId(string value)
        {
            return "data." + DashBoardAgil.CrossCutting.Util.PrimeiroCaracterMinusculo(value);
        }

        /// <summary>
        /// Gera Link do Tipo Não Modal.
        /// </summary>
        /// <param name="link">Link Atual.</param>
        /// <param name="compCreatedRow">String Builer da Criação da Linha.</param>
        /// <param name="nameLink">Nome do Link.</param>
        /// <param name="parameter">Nome do Parâmetro.</param>
        private static void AppendLinkNaoModal(Link link, StringBuilder compCreatedRow, string nameLink, string parameter)
        {
            string target = string.Empty;

            switch (link.TypeLink)
            {
                case TypeLink.Padrao:
                    target = "href=\\'/" + link.Controller + "/" + link.Action + "?" + parameter + "\\'";
                    break;
                case TypeLink.AbsoluteUrl:
                    target = "href=\\'" + link.AbsoluteUrl + "\\'";
                    break;
                case TypeLink.UrlViaDataField:
                    target = "href=\\'' + data." + DashBoardAgil.CrossCutting.Util.PrimeiroCaracterMinusculo(link.UrlViaDataField) + " + '\\'";
                    break;
                case TypeLink.JavaScript:
                    target = "href=\\'javascript:\\' onclick=\\'" + link.JavaScriptFunction + "(\"' + data." + DashBoardAgil.CrossCutting.Util.PrimeiroCaracterMinusculo(link.JavaScripParamName) + " + '\")\\'";
                    break;
            }

            string targetBlank = link.IsBlank ? "target=\\'_blank\\'" : string.Empty;

            compCreatedRow.Append(" linkModal = '<span aria-label=\\'" + link.TitleAlt + "\\' class=\\'hint--top hint--medium\\'><a " + target + " " + targetBlank + " aria-label=\\'" + link.TitleAlt + "\\' class=\\'" + link.Class + "\\'>" + nameLink + "</a></span>';");
        }

        /// <summary>
        /// Gera Link do Tipo Modal (TypeLink.Modal)
        /// </summary>
        /// <param name="link">Link Atual.</param>
        /// <param name="compCreatedRow">String Builer da Criação da Linha.</param>
        /// <param name="nameLink">Nome do Link.</param>
        /// <param name="parameter">Nome do Parâmetro.</param>
        private static void AppendLinkModal(Link link, StringBuilder compCreatedRow, string nameLink, string parameter)
        {
            compCreatedRow.Append("linkModal = '<span aria-label=\\'" + link.TitleAlt + "\\' class=\\'hint--top hint--medium\\'><a " + GetId(link) + " href=\\'/" + link.Controller + "/" + link.Action + "?" + parameter + "\\' " + GetDataAjaxSuccess(link) + " class=\\'" + link.Class + "\\' " + GetDataAjaxUpdate(link) + " " + GetDataAjaxBegin(link) + " data-ajax=\\'true\\' data-ajax-loading=\\'#divCarregando\\' data-ajax-method=\\'" + link.HttpMethod.GetDisplayName() + "\\' " + GetDataAjaxMode(link) + ">" + nameLink + "</a></span>';");
        }

        /// <summary>
        /// Obtém o atributo 'data-ajax-success' preenchido.
        /// </summary>
        /// <param name="link">Link Atual.</param>
        /// <returns></returns>
        public static string GetDataAjaxSuccess(Link link)
        {
            StringBuilder dataAjaxSuccess = new StringBuilder("data-ajax-success=\\'");
            dataAjaxSuccess.Append(link.TypeLink.Equals(TypeLink.Remove) ? "AlertOnReturn(data.Data,\"" + link.CallBackFunction + "\");" : "comprasnet.showModal(\"" + link.ModalTitle + "\"," + link.ModalResponsive.ToString().ToLower(CultureInfo.InvariantCulture) + ", null, \"" + link.ModalWidth + "\", \"" + link.Modalheight + "\");");
            dataAjaxSuccess.Append("\\'");

            return dataAjaxSuccess.ToString();
        }

        /// <summary>
        /// Obtém o atributo 'data-ajax-mode' preenchido.
        /// </summary>
        /// <param name="link">Link Atual.</param>
        /// <returns></returns>
        public static string GetHref(Link link)
        {
            return link.TypeLink.Equals(TypeLink.Modal) ? "data-ajax-mode=\\'replace\\'" : string.Empty;
        }

        /// <summary>
        /// Obtém o atributo 'data-ajax-mode' preenchido.
        /// </summary>
        /// <param name="link">Link Atual.</param>
        /// <returns></returns>
        public static string GetDataAjaxMode(Link link)
        {
            return link.TypeLink.Equals(TypeLink.Modal) ? "data-ajax-mode=\\'replace\\'" : string.Empty;
        }

        /// <summary>
        /// Obtém o atributo 'data-ajax-update' preenchido.
        /// </summary>
        /// <param name="link">Link Atual.</param>
        /// <returns></returns>
        public static string GetDataAjaxUpdate(Link link)
        {
            return link.TypeLink.Equals(TypeLink.Modal) ? "data-ajax-update=\\'#ModalContent\\'" : string.Empty;
        }

        /// <summary>
        /// Obtém o atributo 'data-ajax-begin' preenchido.
        /// </summary>
        /// <param name="link">Link Atual.</param>
        /// <returns></returns>
        public static string GetDataAjaxBegin(Link link)
        {
            return link.TypeLink.Equals(TypeLink.Remove) ? "data-ajax-begin=\\'return ConfirmaRemoveArquivo(\"link_' + " + GetValueDataId(link.ParameterValueDataField) + " + '\");\\'" : string.Empty;
        }

        /// <summary>
        /// Obtém o atributo 'id' preenchido, se for o caso.
        /// </summary>
        /// <param name="link">Link Atual.</param>
        /// <returns></returns>
        public static string GetId(Link link)
        {
            return link.TypeLink.Equals(TypeLink.Remove) ? "id=\\'link_' + " + GetValueDataId(link.ParameterValueDataField) + " + '\\'" : string.Empty;
        }
    }
}
