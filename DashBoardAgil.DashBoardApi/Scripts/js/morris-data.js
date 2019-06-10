$(function() {
    var times = ["time-demogorgon", "time-ze-murai", "time-traiket", "time-tomas-cafe"  ]
    var Metricas = Array();


    times.forEach(time => {
            $.ajax({
                url: "https://example.com/sip/api.redmine/metricas3.php?filtro=8654&time="+time,
                type: 'GET',
                dataType: 'json',
                async: false, //blocks window close
                success: function(data) {
                    Metricas.push(data);
                   // console.log(data);
                }
            });
    });  

    var donut = new Array();
    var bar = new Array();
    $("#tbLeadTime tbody tr").remove(); 

    Metricas.forEach(item =>{
       // console.log("Nome_do_Time" , item.Nome_do_Time);
        var nome_time = item.Nome_do_Time.replace("time-", "").replace("-", " ").toUpperCase();
        //console.log("nome_time 2" , nome_time);
        donut.push({
            label: nome_time,
            value: parseInt(item.amostra2)
        });

        bar.push({
            y: nome_time,
            Fechado: parseInt(item.qtd_chamados_leadtime),
            NovoAberto: parseInt(item.qtd_chamados_novo_aberto),
            Rejeitados:  parseInt(item.qtd_chamados_rejeitados),
        });

        

         /*
         <th>#</th>
        <th>Time</th>
        <th>LeadUtil Time</th>
        <th>Chamados Fechados</th>
        <tr>
            <td>3326</td>
            <td>10/21/2013</td>
            <td>3:29 PM</td>
            <td>$321.33</td>
        </tr>

        */

        var newRowContent = "<tr> "+
                           " <td>"+ parseInt(Metricas.indexOf(item))+1 +"</td> "+
                           " <td>"+ nome_time +"</td> "+
                           " <td>"+ item.leadtime +"</td> "+
                           " <td>"+ item.qtd_chamados_leadtime +"</td> "+
                           " </tr>";


        $("#tbLeadTime tbody").append(newRowContent); 
       
    });


    Morris.Donut({
        element: 'morris-donut-chart',
        data: donut,
        resize: true
    });

    

    Morris.Bar({
        element: 'morris-bar-chart',
        data: bar,
        xkey: 'y',
        ykeys: ['Fechado', 'NovoAberto', 'Rejeitados'],
        labels: ['Fechado', 'Novo->Aberto', 'Rejeitados'],
        // barColors: function (row, series, type) {
        //     console.log("--> "+row.label, series, type);
        //     if(row.label == "time-tomas-cafe") return "#AD1D28";
        //     else if(row.label == "time-traiket") return "#DEBB27";
        //     else if(row.label == "time-demogorgon") return "#fec04c";
        //     else if(row.label == "time-ze-murai") return "#1AB244";
        //     },
        hideHover: 'auto',
        resize: true
    });






/*
    $.when(
        $.ajax("https://example.com/sip/api.redmine/metricas3.php?filtro=8654&time="+times[0]),
        $.ajax("https://example.com/sip/api.redmine/metricas3.php?filtro=8654&time="+times[1]),
        $.ajax("https://example.com/sip/api.redmine/metricas3.php?filtro=8654&time="+times[2]),
        $.ajax("https://example.com/sip/api.redmine/metricas3.php?filtro=8654&time="+times[3]),
        )
        .done(function(data1, data2, data3, data4){
           
            Morris.Donut({
                element: 'morris-donut-chart',
                data: [{
                    label: data1["Nome_do_Time"],
                    value: data1["amostra2"]
                }, {
                    label: data2["Nome_do_Time"],
                    value: data2["amostra2"]
                }, {
                    label: data3["Nome_do_Time"],
                    value: data3["amostra2"]
                }, {
                    label: data4["Nome_do_Time"],
                    value: data4["amostra2"]
                }],
                resize: true
            });

            



        })
        .fail(function(){
            //handle errors
        });

*/


   // console.log(Metricas);




    Morris.Area({
        element: 'morris-area-chart',
        data: [{
            period: '2010 Q1',
            iphone: 2666,
            ipad: null,
            itouch: 2647
        }, {
            period: '2010 Q2',
            iphone: 2778,
            ipad: 2294,
            itouch: 2441
        }, {
            period: '2010 Q3',
            iphone: 4912,
            ipad: 1969,
            itouch: 2501
        }, {
            period: '2010 Q4',
            iphone: 3767,
            ipad: 3597,
            itouch: 5689
        }, {
            period: '2011 Q1',
            iphone: 6810,
            ipad: 1914,
            itouch: 2293
        }, {
            period: '2011 Q2',
            iphone: 5670,
            ipad: 4293,
            itouch: 1881
        }, {
            period: '2011 Q3',
            iphone: 4820,
            ipad: 3795,
            itouch: 1588
        }, {
            period: '2011 Q4',
            iphone: 15073,
            ipad: 5967,
            itouch: 5175
        }, {
            period: '2012 Q1',
            iphone: 10687,
            ipad: 4460,
            itouch: 2028
        }, {
            period: '2012 Q2',
            iphone: 8432,
            ipad: 5713,
            itouch: 1791
        }],
        xkey: 'period',
        ykeys: ['iphone', 'ipad', 'itouch'],
        labels: ['iPhone', 'iPad', 'iPod Touch'],
        pointSize: 2,
        hideHover: 'auto',
        resize: true
    });

    // Morris.Donut({
    //     element: 'morris-donut-chart',
    //     data: [{
    //         label: "Download Sales",
    //         value: 12
    //     }, {
    //         label: "In-Store Sales",
    //         value: 30
    //     }, {
    //         label: "Mail-Order Sales",
    //         value: 20
    //     }],
    //     resize: true
    // });

    // Morris.Bar({
    //     element: 'morris-bar-chart',
    //     data: [{
    //         y: '2006',
    //         a: 100,
    //         b: 90
    //     }, {
    //         y: '2007',
    //         a: 75,
    //         b: 65
    //     }, {
    //         y: '2008',
    //         a: 50,
    //         b: 40
    //     }, {
    //         y: '2009',
    //         a: 75,
    //         b: 65
    //     }, {
    //         y: '2010',
    //         a: 50,
    //         b: 40
    //     }, {
    //         y: '2011',
    //         a: 75,
    //         b: 65
    //     }, {
    //         y: '2012',
    //         a: 100,
    //         b: 90
    //     }],
    //     xkey: 'y',
    //     ykeys: ['a', 'b'],
    //     labels: ['Series A', 'Series B'],
    //     hideHover: 'auto',
    //     resize: true
    // });

});
