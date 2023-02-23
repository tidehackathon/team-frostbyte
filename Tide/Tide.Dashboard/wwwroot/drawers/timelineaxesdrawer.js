const TimelineAxesDrawer = function () {

    class TimelineAxesInstance {
        model = null;
        id = null;
        seriesCount = 0;

        constructor(id, model) {
            this.id = id;

            //model = {
            //    series: [
            //        {
            //            "name": "Focus Area 1",
            //            "values": [{
            //                "year": "2019",
            //                "value": 100
            //            },
            //            {
            //                "year": "2020",
            //                "value": 200
            //            },
            //            {
            //                "year": "2021",
            //                "value": 300
            //            },
            //            {
            //                "year": "2022",
            //                "value": 400
            //            }
            //            ]
            //        },
            //        {
            //            "name": "Focus Area 2",
            //            "values": [{
            //                "year": "2019",
            //                "value": 100
            //            },
            //            {
            //                "year": "2020",
            //                "value": 200
            //            },
            //            {
            //                "year": "2021",
            //                "value": 300
            //            },
            //            {
            //                "year": "2022",
            //                "value": 400
            //            }
            //            ]
            //        }
            //    ],
            //    yearStart: 2019,
            //    yearEnd:2022
            //}
            
            this.#preprocessModel(model);
            this.#init();
        }

        #preprocessModel(model) {
            this.model = model;
        }

        #init() {
            am5.ready(() => {
                //paramaters
                //object is name and values : {year:value}
                var startYear = this.model.yearStart;
                var endYear = this.model.yearEnd;
                var dataValues = this.model.series;


                // Create root element
                // https://www.amcharts.com/docs/v5/getting-started/#Root_element
                var root = am5.Root.new(this.id);

                // Create custom theme
                // https://www.amcharts.com/docs/v5/concepts/themes/#Quick_custom_theme
                const myTheme = am5.Theme.new(root);
                myTheme.rule("Label").set("fontSize", 10);
                myTheme.rule("Grid").set("strokeOpacity", 0.06);

                // Set themes
                // https://www.amcharts.com/docs/v5/concepts/themes/
                root.setThemes([
                    am5themes_Animated.new(root),
                    myTheme
                ]);

                function generateDataFromModel(model) {
                    let data = [];
                    for (var i = 0; i < model.length; i++) {
                        let intermed = [];
                        var obj = model[i];
                        var name = obj.name;
                        var values = obj.values;
                        intermed.push(name);
                        intermed.push(0);
                        for (var j = 0; j < values.length; j++) {
                            var value = values[j];
                            var year = value.year;
                            var val = value.value;
                            intermed.push(val);
                        }
                        data.push(intermed);
                    }
                    return data;
                }


                // Data
                var cwix = {
                    "CWIX": generateDataFromModel(dataValues)
                }

                // calculate min and max values
                var minValue = 0;
                var maxValue = 0;
                var series = cwix["CWIX"];
                for (var i = 0; i < series.length; i++) {
                    for (var j = 1; j < series[i].length; j++) {
                        var value = series[i][j];
                        if (value < minValue) {
                            minValue = value;
                        }
                        if (value > maxValue) {
                            maxValue = value;
                        }
                    }
                }
                maxValue = maxValue * 2;


                // Modify defaults
                //root.numberFormatter.set("numberFormat", "+#.0°C|#.0°C|0.0°C");


                var currentYear = startYear;

                var div = document.getElementById("chartdiv");

                var colorSet = am5.ColorSet.new(root, {});


                // Create chart
                // https://www.amcharts.com/docs/v5/charts/radar-chart/
                var chart = root.container.children.push(am5radar.RadarChart.new(root, {
                    panX: false,
                    panY: false,
                    wheelX: "panX",
                    wheelY: "zoomX",
                    innerRadius: am5.percent(40),
                    radius: am5.percent(65),
                    startAngle: 270 - 170,
                    endAngle: 270 + 170
                }));


                // Add cursor
                // https://www.amcharts.com/docs/v5/charts/radar-chart/#Cursor
                var cursor = chart.set("cursor", am5radar.RadarCursor.new(root, {
                    behavior: "zoomX",
                    radius: am5.percent(40),
                    innerRadius: -25
                }));
                cursor.lineY.set("visible", false);


                // Create axes and their renderers
                // https://www.amcharts.com/docs/v5/charts/radar-chart/#Adding_axes
                var xRenderer = am5radar.AxisRendererCircular.new(root, {
                    minGridDistance: 10
                });

                xRenderer.labels.template.setAll({
                    radius: 10,
                    textType: "radial",
                    centerY: am5.p50
                });

                var yRenderer = am5radar.AxisRendererRadial.new(root, {
                    axisAngle: 90
                });

                yRenderer.labels.template.setAll({
                    centerX: am5.p50
                });

                var categoryAxis = chart.xAxes.push(am5xy.CategoryAxis.new(root, {
                    maxDeviation: 0,
                    categoryField: "focus",
                    renderer: xRenderer
                }));

                var valueAxis = chart.yAxes.push(am5xy.ValueAxis.new(root, {
                    min: minValue,
                    max: maxValue,
                    extraMax: 0.1,
                    renderer: yRenderer
                }));


                // Create series
                // https://www.amcharts.com/docs/v5/charts/radar-chart/#Adding_series
                var series = chart.series.push(am5radar.RadarColumnSeries.new(root, {
                    calculateAggregates: true,
                    name: "Series",
                    xAxis: categoryAxis,
                    yAxis: valueAxis,
                    valueYField: "value" + currentYear,
                    categoryXField: "focus",
                    tooltip: am5.Tooltip.new(root, {
                        labelText: "{categoryX}: {valueY}"
                    })
                }));

                series.columns.template.set("strokeOpacity", 0);


                // Set up heat rules
                // https://www.amcharts.com/docs/v5/concepts/settings/heat-rules/
                series.set("heatRules", [{
                    target: series.columns.template,
                    key: "fill",
                    min: am5.color(0x673AB7),
                    max: am5.color(0xF44336),
                    dataField: "valueY"
                }]);

                // Add scrollbars
                // https://www.amcharts.com/docs/v5/charts/xy-chart/scrollbars/
                chart.set("scrollbarX", am5.Scrollbar.new(root, { orientation: "horizontal" }));
                chart.set("scrollbarY", am5.Scrollbar.new(root, { orientation: "vertical" }));

                // Add year label
                var yearLabel = chart.radarContainer.children.push(am5.Label.new(root, {
                    fontSize: "2em",
                    text: currentYear.toString(),
                    centerX: am5.p50,
                    centerY: am5.p50,
                    fill: am5.color(0x673AB7)
                }));


                // Generate and set data
                // https://www.amcharts.com/docs/v5/charts/radar-chart/#Setting_data
                var data = generateRadarData();
                series.data.setAll(data);
                categoryAxis.data.setAll(data);

                series.appear(1000);
                chart.appear(1000, 100);

                function generateRadarData() {
                    var data = [];
                    var i = 0;
                    var focusData = cwix['CWIX'];

                    focusData.forEach(function (focus) {
                        var rawDataItem = { "focus": focus[0] }

                        for (var y = 2; y < focus.length; y++) {
                            rawDataItem["value" + (startYear + y - 2)] = focus[y];
                        }

                        data.push(rawDataItem);
                    });

                    createRange("CWIX", focusData, i);

                    return data;
                }


                function createRange(name, focusData, index) {
                    var axisRange = categoryAxis.createAxisRange(categoryAxis.makeDataItem({ above: true }));
                    axisRange.get("label").setAll({ text: name });
                    // first country
                    axisRange.set("category", focusData[0][0]);
                    // last country
                    axisRange.set("endCategory", focusData[focusData.length - 1][0]);

                    // every 3rd color for a bigger contrast
                    var fill = axisRange.get("axisFill");
                    fill.setAll({
                        toggleKey: "active",
                        cursorOverStyle: "pointer",
                        fill: colorSet.getIndex(index * 3),
                        visible: true,
                        innerRadius: -25
                    });
                    axisRange.get("grid").set("visible", false);

                    var label = axisRange.get("label");
                    label.setAll({
                        fill: am5.color(0xffffff),
                        textType: "circular",
                        radius: -16
                    });

                    fill.events.on("click", function (event) {
                        var dataItem = event.target.dataItem;
                        if (event.target.get("active")) {
                            categoryAxis.zoom(0, 1);
                        }
                        else {
                            categoryAxis.zoomToCategories(dataItem.get("category"), dataItem.get("endCategory"));
                        }
                    });
                }


                // Create controls
                var container = chart.children.push(am5.Container.new(root, {
                    y: am5.percent(95),
                    centerX: am5.p50,
                    x: am5.p50,
                    width: am5.percent(80),
                    layout: root.horizontalLayout
                }));

                var playButton = container.children.push(am5.Button.new(root, {
                    themeTags: ["play"],
                    centerY: am5.p50,
                    marginRight: 15,
                    icon: am5.Graphics.new(root, {
                        themeTags: ["icon"]
                    })
                }));

                playButton.events.on("click", function () {
                    if (playButton.get("active")) {
                        slider.set("start", slider.get("start") + 0.0001);
                    }
                    else {
                        slider.animate({
                            key: "start",
                            to: 1,
                            duration: 15000 * (1 - slider.get("start"))
                        });
                    }
                })

                var slider = container.children.push(am5.Slider.new(root, {
                    orientation: "horizontal",
                    start: 0.5,
                    centerY: am5.p50
                }));

                slider.on("start", function (start) {
                    if (start === 1) {
                        playButton.set("active", false);
                    }
                });

                slider.events.on("rangechanged", function () {
                    updateRadarData(startYear + Math.round(slider.get("start", 0) * (endYear - startYear)));
                });

                function updateRadarData(year) {
                    if (currentYear != year) {
                        currentYear = year;
                        yearLabel.set("text", currentYear.toString());
                        am5.array.each(series.dataItems, function (dataItem) {
                            var newValue = dataItem.dataContext["value" + year];
                            dataItem.set("valueY", newValue);
                            dataItem.animate({ key: "valueYWorking", to: newValue, duration: 500 });
                        });
                    }
                }

            }); 
        }
    }


    function createInstance(id, url) {
        return new Promise((resolve, reject) => {
            _core.ajax.request(url, null, {
                success: function (data) {
                    resolve(new TimelineAxesInstance(id, data));
                }
            })
        });
    }

    return {
        createInstance
    }
}();


