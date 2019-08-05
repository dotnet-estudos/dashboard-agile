$(function() {

    $('#side-menu').metisMenu();

});

//Loads the correct sidebar on window load,
//collapses the sidebar on window resize.
// Sets the min-height of #page-wrapper to window size
$(function() {
    $(window).bind("load resize", function() {
        topOffset = 50;
        width = (this.window.innerWidth > 0) ? this.window.innerWidth : this.screen.width;
        if (width < 768) {
            $('div.navbar-collapse').addClass('collapse');
            $(".sidebar").css({ "position": "absolute", "left": "0px", "top": (50) + "px" });
            topOffset = 100; // 2-row-menu
        } else {
            $('div.navbar-collapse').removeClass('collapse');
            $(".sidebar").css({ "position": "absolute", "left": "0px", "top": "0px" });
        }

        height = ((this.window.innerHeight > 0) ? this.window.innerHeight : this.screen.height) - 1;
        height = height - topOffset;
        if (height < 1) height = 1;
        if (height > topOffset) {
            $("#page-wrapper").css("min-height", (height) + "px");
            $("#page-wrapper").css('margin-left', '');
        }
    });

    var url = window.location;
    var element = $('ul.nav a').filter(function() {
        return (this.href == url || url.href.indexOf(this.href) == 0) && this.id != "__agent";
    }).addClass('active').parent().parent().addClass('in').parent(); //
    if (element.is('li')) {

        element.addClass('active');
    }

    $('#iconMenu').click(function (ev) {
        time = 300;
        margin_left = $('#page-wrapper').css('margin-left');
        if (margin_left != '0px' || margin_left == undefined) {
            $('#page-wrapper').animate({ "margin-left": "0px" }, time, function () { chartSizeble(); });
            $(".sidebar").animate({ "left": "-250px" }, time, function () { chartSizeble(); });
        } else {
            $('#page-wrapper').animate({ "margin-left": "250px" }, time, function () { chartSizeble(); });
            $(".sidebar").animate({ "left": "0px" }, time, function () { chartSizeble(); });
        }

    });
});
function chartSizeble() {
   
    $(".chartSizeble").each(function () {
        if ($(this).highcharts()) {
            $(this).highcharts().reflow();
        }
    });
}


var MesesNome = ["Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", "Agustus", "September", "Oktober", "November", "Desember"];
var MesesNomeBr = ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outrubro", "Novembro", "Dezembro"];

var MesNomeShort = ['Jan', 'Feb', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'];
var dayName = ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sabado'];
var dayNomeShort = ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sab']

Number.prototype.padLeft = function (n, str) {
    if (n && str) {
        return Array(n - String(this).length + 1).join(str || '0') + this
    } else {
        return this;
    };
}

function renderDatatable(table) {
    $(table).DataTable({
        language: {
            "sEmptyTable": "Nenhum registro encontrado",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
            "sInfoFiltered": "(Filtrados de _MAX_ registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "_MENU_ resultados por página",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "Processando...",
            "sZeroRecords": "Nenhum registro encontrado",
            "sSearch": "Pesquisar",
            "oPaginate": {
                "sNext": "Próximo",
                "sPrevious": "Anterior",
                "sFirst": "Primeiro",
                "sLast": "Último"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            }
        }
    });
}

function renderCalendar(div, notes, ini, fim) {

    var my_calendar = $(div).dnCalendar({
        minDate: ini,
        maxDate: fim,
        defaultDate: ini,
        monthNames: MesesNomeBr,
        monthNamesShort: MesNomeShort,
        dayNames: dayName,
        dayNamesShort: dayNomeShort,
        dataTitles: { defaultDate: 'Inicio', today: 'Hoje' },
        notes: notes
        //[
        //    { "date": "2018-12-25", "note": ["Natal"] },
        //    { "date": "2018-12-12", "note": ["Tahun Baru"] }
        //]
        ,
        showNotes: true,
        startWeek: 'sunday',
        dayClick: function (date, view) {
            //alert(date.getDate() + "-" + (date.getMonth() + 1) + "-" + date.getFullYear());
        }
    });

    // init calendar
    my_calendar.build();

}


function renderComulativeFlow(div, title, subtitle, categories, yTitles, valueSuffix, series) {
    Highcharts.chart(div, {
        chart: {
            type: 'area'
        },
        title: {
            text: title //'CFD Traiket Semana 1-9'
        },
        subtitle: {
            text: subtitle//'Source: redmine.example.com'
        },
        xAxis: {
            //categories: ['1750', '1800', '1850', '1900', '1950', '1999', '2050'],
            categories:  categories,//['1', '2', '3', '4', '5', '6', '7', '8', '9'],
            tickmarkPlacement: 'on',
            title: {
                enabled: true
            },
            labels: {
                formatter: function () {
                    return this.value; //dateFromDay(2019, this.value);
                }
            }
        },
        yAxis: {
            title: {
                text: yTitles //'Chamado'
            },
            labels: {
                formatter: function () {
                    return this.value;/// 1000;
                }
            }
        },
        tooltip: {
            split: true,
            valueSuffix: valueSuffix  // ' Chamado(s)'
        },
        plotOptions: {
            area: {
                stacking: 'normal',
                lineColor: '#666666',
                lineWidth: 1,
                marker: {
                    lineWidth: 1,
                    lineColor: '#666666'
                }
            }
        },
        series: series
        /*
        series: [{
            name: 'Asia',
            data: [502, 635, 809, 947, 1402, 3634, 5268]
        }, {
            name: 'Africa',
            data: [106, 107, 111, 133, 221, 767, 1766]
        }, {
            name: 'Europe',
            data: [163, 203, 276, 408, 547, 729, 628]
        }, {
            name: 'America',
            data: [18, 31, 54, 156, 339, 818, 1201]
        }, {
            name: 'Oceania',
            data: [2, 2, 2, 6, 13, 30, 46]
        }] */
    });
}

function GetFormattedDate(date) {
    var todayTime = date;
    var month = todayTime.getMonth() + 1;
    var day = todayTime.getDate();
    var year = todayTime.getFullYear();
    return day + "/" + month + "/" + year;
}

function dateFromDay(year, day) {
    
    var date = new Date(year, 0); // initialize a date in `year-01-01`
    return GetFormattedDate(new Date(date.setDate(day))); // add the number of days
}

function renderDispersionDiagram(div, _title, _subtitle, _avarage,_avarageMediana, _serie, _year) {
    

    Highcharts.chart(div, {
        chart: {
            type: 'scatter',
            zoomType: 'xy'
        },
        title: {
            text: _title
        },
        subtitle: {
            text: _subtitle
        },
        xAxis: {
            title: {
                enabled: true,
                text: 'Data'
            },
            labels:{
                formatter: function() {
                    return dateFromDay(_year, this.value);
                }
            },
            startOnTick: true,
            endOnTick: true,
            showLastLabel: true
        },
        yAxis: {
            title: {
                text: 'Lead Time'
            },
            plotLines: [{
                color: 'blue',
                dashStyle: 'line', //dot, line
                width: 2,
                value: parseInt(_avarage),
                label: {
                    rotation: 0,
                    align: 'left',
                    y: 15,
                    style: {
                        fontStyle: 'italic',
                        color: 'blue'
                    },
                    text: 'Lead Time Médio: ' + _avarage
                },
                zIndex: 1000
            },
            {
                color: 'purple',
                dashStyle: 'line', //dot, line
                width: 2,
                value: parseInt(_avarageMediana),
                label: {
                    rotation: 0,
                    align: 'right',
                    y: 15,
                    style: {
                        fontStyle: 'italic',
                        color: 'purple'
                    },
                    text: 'Mediana: ' + _avarageMediana
                },
                zIndex: 1000
            }
            ]
        },
        legend: {
            layout: 'vertical',
            align: 'center',
            verticalAlign: 'top',
            x: 100,
            y: 70,
            floating: true,
            backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF',
            borderWidth: 1
        },
        plotOptions: {
            scatter: {
                marker: {
                    radius: 5,
                    states: {
                        hover: {
                            enabled: true,
                            lineColor: 'rgb(100,100,100)'
                        }
                    }
                },
                states: {
                    hover: {
                        marker: {
                            enabled: false
                        }
                    }
                },
                tooltip: {
                    headerFormat: '<b>{series.name} </b><br>',
                    pointFormat: 'Chamado: <b>#{point.name} </b>, {point.y} Lead Time'//Dia:{point.x} , 
                }
            }
        },
        series: _serie//[{}] 
        });

    /*
    {
        name: 'Female',
        color: 'rgba(223, 83, 83, .5)',
        data: [[161.2, 51.6], [167.5, 59.0], [159.5, 49.2], [157.0, 63.0], [155.8, 53.6],
            [170.0, 59.0], [159.1, 47.6], [166.0, 69.8], [176.2, 66.8], [160.2, 75.2],
            [172.5, 55.2], [170.9, 54.2], [172.9, 62.5], [153.4, 42.0], [160.0, 50.0],
            [147.2, 49.8], [168.2, 49.2], [175.0, 73.2], [157.0, 47.8], [167.6, 68.8],
            [176.5, 71.8], [164.4, 55.5], [160.7, 48.6], [174.0, 66.4], [163.8, 67.3]]

    },
    , {
        name: 'Male',
        color: 'rgba(119, 152, 191, .5)',
        data: 
		[[174.0, 65.6], [175.3, 71.8], [193.5, 80.7], [186.5, 72.6], [187.2, 78.8],
            [181.5, 74.8], [184.0, 86.4], [184.5, 78.4], [175.0, 62.0], [184.0, 81.6],
            [180.0, 76.6], [177.8, 83.6], [192.0, 90.0], [176.0, 74.6], [174.0, 71.0],
            [184.0, 79.6], [192.7, 93.8], [171.5, 70.0], [173.0, 72.4], [176.0, 85.9],
            [176.0, 78.8], [180.5, 77.8], [172.7, 66.2], [176.0, 86.4], [173.5, 81.8],
            [178.0, 89.6], [180.3, 82.8], [180.3, 76.4], [164.5, 63.2], [173.0, 60.9],
            [183.5, 74.8], [175.5, 70.0], [188.0, 72.4], [189.2, 84.1], [172.8, 69.1],
             [170.2, 62.3], [177.8, 82.7], [179.1, 79.1], [190.5, 98.2], [177.8, 84.1],
            [180.3, 83.2], [180.3, 83.2]]
    }
    */
}

function renderTaleDiagram(div, _title, _subtitle, _titleBarra, _TitleLine,_year, _avarage, _Categoria, _serieBarra, _SerieLine ) {

    Highcharts.chart(div, {
        chart: {
            zoomType: 'xy'
        },
        title: {
            text: _title
        },
        subtitle: {
            text: _subtitle
        },
        xAxis: [{
            categories: _Categoria,
            crosshair: true
        }],
        yAxis: [{ // Primary yAxis
            labels: {
                format: '{value} %',
                style: {
                    color: Highcharts.getOptions().colors[1]
                }
            },
            title: {
                text: 'Lead Time',
                style: {
                    color: Highcharts.getOptions().colors[1]
                }
            }
        }, { // Secondary yAxis
            title: {
                text: 'Quantidade',
                style: {
                    color: Highcharts.getOptions().colors[0]
                }
            },
            labels: {
                format: '{value}',
                style: {
                    color: Highcharts.getOptions().colors[0]
                }
            },
            opposite: true
        }],
        tooltip: {
            shared: true
        },
        legend: {
            layout: 'vertical',
            align: 'rigth',
            x: 120,
            verticalAlign: 'top',
            y: 100,
            floating: true,
            backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || 'rgba(255,255,255,0.25)'
        },
        series: [{
            name: 'Chamados/Lead Time',
            type: 'column',
            yAxis: 1,
            data: _serieBarra,
            tooltip: {
                valueSuffix: ' Chamados'
            }

        },{
            name: 'Porcentagem',
            type: 'spline',
            data: _SerieLine,
            tooltip: {
                valueSuffix: ' %'
            }
        }]
    });
}

function renderRejectionThroughput(div, _title, _subtitle, _Categoria, _serie, _avarageThroughput, _avarageRejection) {
    Highcharts.chart(div, {

    chart: {
        type: 'column'
    },

    title: {
        text: _title
    },
    subtitle: {
        text: _subtitle
    },
    legend: {
        labelFormatter: function () {
            return this.name + ' (' + this.userOptions.stack + ')';
        }
    },
    xAxis: {
        categories: _Categoria
    },

    yAxis: {
        allowDecimals: false,
        min: 0,
        title: {
            text: 'Qtd (Média: ' + _avarageThroughput + ' Rejeição: ' + _avarageRejection + ')'
        },
        //plotLines: [{
        //        color: 'blue',
        //        dashStyle: 'line', //dot, line
        //        width: 2,
        //        value: parseInt(_avarageThroughput),
        //        label: {
        //            rotation: 0,
        //            y: 15,
        //            style: {
        //                fontStyle: 'italic',
        //                color: 'blue'
        //            },
        //            text: 'Throughput Médio: ' + _avarageThroughput
        //        },
        //        zIndex: 1000
        //},
        //{
        //    color: 'red',
        //    dashStyle: 'line', //dot, line
        //    width: 2,
        //    value: parseFloat(_avarageRejection),
        //    label: {
        //        rotation: 0,
        //        y: 15,
        //        style: {
        //            fontStyle: 'italic',
        //            color: 'red'
        //        },
        //        text: 'Taxa de Rejeição Média: ' + _avarageRejection
        //    },
        //    zIndex: 1000
        //}]
    },

    tooltip: {
        formatter: function () {
            return '<b> Semana: ' + this.x + '</b><br/>' +
               this.series.userOptions.stack+': '+ this.series.name + ': ' + this.y + '<br/>' +
                'Total: ' + this.point.stackTotal;
        }
    },

    plotOptions: {
        column: {
            stacking: 'normal'
        }
    },

    series: _serie
});
}

function renderGaugeGoogle(div, data, options) {

    var ArrayData = [];
    var arrAux = [];
    var arrAux1 = [];
    $.each(data, function (chave, valor) {
        arrAux.push(chave);
        arrAux1.push(data[chave]);
    })

    ArrayData.push(arrAux);
    ArrayData.push(arrAux1);
   
    var chart = new google.visualization.Gauge(document.getElementById(div));
    var dataTableData = new google.visualization.arrayToDataTable(ArrayData);
    console.log(dataTableData);
    chart.draw(dataTableData, options);

}

function renderGaugeGoogleAll(elemnts) {
    elemnts.forEach(function(iten, index) {
        renderGaugeGoogle(iten.element, iten.GaugeGoogle.data, iten.GaugeGoogle.options);
    });
}

function blockCampos(block) {
    $(".lockable").each(function () {
        $(this).prop("disabled", block);
    });
}

$(document).ready(function () {

    $('table[data-datatable=true]').DataTable({
        language: {
            "sEmptyTable": "Nenhum registro encontrado",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
            "sInfoFiltered": "(Filtrados de _MAX_ registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "_MENU_ resultados por página",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "Processando...",
            "sZeroRecords": "Nenhum registro encontrado",
            "sSearch": "Pesquisar",
            "oPaginate": {
                "sNext": "Próximo",
                "sPrevious": "Anterior",
                "sFirst": "Primeiro",
                "sLast": "Último"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            }
        }
    });

    $.switcher('input[type=checkbox]');
    
});