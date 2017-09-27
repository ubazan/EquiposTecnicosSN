$(function () {

    
    var getChartData = function () {

        var $form = $("form.consulta-indicadores");

        var indicador = $(this).data("nqn-indicador");

        var indicadorTipo = $form.data("nqn-tipo-indicador")

        $("#ParetoChartContainer").empty();

        if (!$form.valid()) {
            return false;
        }

        var fechaInicio = $("#fechaInicio").val();
        var fechaFin = $("#fechaFin").val();


        $("#ParetoChart").html("<div class='loader'></div>");

        $.ajax({
            type: 'GET',
            url: window.location.pathname.split('/')[1] + '/Indicadores/Pareto' + indicador + 'Data' + indicadorTipo,
            dataType: 'json',
            data: $form.serialize(),
            success: function (response) {
                var keys = [];
                var values = [];

                for (key in response) {
                    keys.push(key);
                    values.push(response[key]);
                }

                var paretoChartConfig = {
                    "type": "pareto",
                    "options": {
                        "line-plot": {
                            "line-color": "#FF7F45",
                            "value-box": {
                                "background-color": "#FF7F45",
                                "shadow": false
                            },
                            "marker": {
                                "background-color": "#FF7F45",
                            }
                        }
                    },
                    "series": [],
                    "scale-x": {
                        "values": [],
                        "tooltip": {
                            "text": "%v"
                        },
                        "margin-bottom": "200px",
                        "line-width": 1,
                        "line-color": "#3285A6",
                        "items-overlap": true,
                        "guide": {
                            "visible": true
                        },
                        "item": {
                            "font-color": "#444",
                            "font-size": 10,
                            "angle": -30
                        },
                        "tick": {
                            "line-width": 1,
                            "line-color": "#3285A6"
                        }
                    }
                };

                paretoChartConfig["scale-x"]["values"] = keys;
                paretoChartConfig.series.push({ "values": values });

                zingchart.exec('ParetoChartContainer', 'destroy');
                $("#ParetoChartContainer").empty();

                zingchart.render({
                    id: 'ParetoChartContainer',
                    data: paretoChartConfig,
                    height: "100%",
                    width: "100%"
                });

            },
            error: function (ex) {
                alert('Ocurrió un error. Inténtelo de nuevo.');
            }
        });

        return false;
    };


    $("[id$=Chart]").on("click", getChartData);    
});

