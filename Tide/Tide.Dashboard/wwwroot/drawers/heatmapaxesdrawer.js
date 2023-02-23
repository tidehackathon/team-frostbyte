
const HeatMapAxesDrawer = function () {

    class HeatMapAxesInstance {
        model = {}
        id = null;
      


        constructor(id, model) {
            //model = {
            //    tags: [
            //        { tag: "Nation1" },
            //        { tag: "Nation2" },

            //    ],
            //    circleItems: [
            //        { circleItem: "2020" },
            //        { circleItem: "2021" },
            //    ],
            //    data: [
            //        {
            //            tag: "Nation1",
            //            circleItem: "2020",
            //            value: 2990
            //        },
            //        {
            //            tag: "Nation1",
            //            circleItem: "2021",
            //            value: 2520
            //        },
            //        {
            //            tag: "Nation2",
            //            circleItem: "2020",
            //            value: 2334
            //        },
            //        {
            //            tag: "Nation2",
            //            circleItem: "2021",
            //            value: 2230
            //        },
            //    ],
            //}
            this.id = id;
            this.#preprocessModel(model);
            this.#init();

        }

        #preprocessModel(model) {
            this.model = model;
        }

        #init() {
            am5.ready(() => {
                

                // Create root element
                // https://www.amcharts.com/docs/v5/getting-started/#Root_element
                var root = am5.Root.new(this.id);


                // Set themes
                // https://www.amcharts.com/docs/v5/concepts/themes/
                root.setThemes([
                    am5themes_Animated.new(root)
                ]);


                // Create chart
                // https://www.amcharts.com/docs/v5/charts/radar-chart/
                var chart = root.container.children.push(am5radar.RadarChart.new(root, {
                    innerRadius: am5.percent(50),
                    panX: false,
                    panY: false,
                    wheelX: "panX",
                    wheelY: "zoomX",
                    maxTooltipDistance: 0,
                    layout: root.verticalLayout
                }));


                // Create axes and their renderers
                // https://www.amcharts.com/docs/v5/charts/radar-chart/#Adding_axes
                var yRenderer = am5radar.AxisRendererRadial.new(root, {
                    visible: false,
                    axisAngle: 90,
                    minGridDistance: 10,
                    inversed: true
                });

                yRenderer.labels.template.setAll({
                    textType: "circular",
                    textAlign: "center",
                    radius: -8
                });

                yRenderer.grid.template.set("visible", false);

                var yAxis = chart.yAxes.push(am5xy.CategoryAxis.new(root, {
                    maxDeviation: 0,
                    renderer: yRenderer,
                    categoryField: "circleItem"
                }));

                var xRenderer = am5radar.AxisRendererCircular.new(root, {
                    visible: false,
                    minGridDistance: 30
                });

                xRenderer.labels.template.setAll({
                    textType: "circular",
                    radius: 10
                });

                xRenderer.grid.template.set("visible", false);

                var xAxis = chart.xAxes.push(am5xy.CategoryAxis.new(root, {
                    renderer: xRenderer,
                    categoryField: "tag"
                }));


                // Create series
                // https://www.amcharts.com/docs/v5/charts/radar-chart/#Adding_series
                var series = chart.series.push(am5radar.RadarColumnSeries.new(root, {
                    calculateAggregates: true,
                    stroke: am5.color(0xffffff),
                    clustered: false,
                    xAxis: xAxis,
                    yAxis: yAxis,
                    categoryXField: "tag",
                    categoryYField: "circleItem",
                    valueField: "value"
                }));

                series.columns.template.setAll({
                    tooltipText: "{value}",
                    strokeOpacity: 1,
                    strokeWidth: 2,
                    width: am5.percent(100),
                    height: am5.percent(100)
                });

                series.columns.template.events.on("pointerover", function (event) {
                    var di = event.target.dataItem;
                    if (di) {
                        heatLegend.showValue(di.get("value", 0));
                    }
                });

                series.events.on("datavalidated", function () {
                    heatLegend.set("startValue", series.getPrivate("valueHigh"));
                    heatLegend.set("endValue", series.getPrivate("valueLow"));
                });


                // Set up heat rules
                // https://www.amcharts.com/docs/v5/concepts/settings/heat-rules/
                series.set("heatRules", [{
                    target: series.columns.template,
                    min: am5.color(0xfffb77),
                    max: am5.color(0xfe131a),
                    dataField: "value",
                    key: "fill"
                }]);


                // Add heat legend
                // https://www.amcharts.com/docs/v5/concepts/legend/heat-legend/
                var heatLegend = chart.children.push(am5.HeatLegend.new(root, {
                    orientation: "horizontal",
                    endColor: am5.color(0xfffb77),
                    startColor: am5.color(0xfe131a)
                }));


                // Set data
                // https://www.amcharts.com/docs/v5/charts/radar-chart/#Setting_data

                series.data.setAll(this.model.data);

                yAxis.data.setAll(this.model.circleItems);

                xAxis.data.setAll(this.model.tags);

                // Make stuff animate on load
                // https://www.amcharts.com/docs/v5/concepts/animations/#Initial_animation
                chart.appear(1000, 100);
           

            });
        }
    }


    function createInstance(id, url) {
        return new Promise((resolve, reject) => {

            _core.ajax.request(url, null, {
                success: function (data) {
                    resolve(new HeatMapAxesInstance(id, data));
                }
            })
        });
    }

    return {
        createInstance
    }
}();