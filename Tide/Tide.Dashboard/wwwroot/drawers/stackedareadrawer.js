const StackedAreaDrawer = function () {

    class StackedAreaInstance {
        model = {
            data: [],
            seriesCount: 0,
            legendLabels: [],
            labelColors: []
        };
        id = null;
        options = {
            fill: null,
            interpolation: null
        };

        constructor(id, model, options) {
            this.id = id;
            this.options = options;

            //model = {
            //    lines: [
            //        {
            //            data: [
            //                { x: 0.3, y: 'Air' },
            //                { x: 0.4, y: 'OpsCmd' },
            //                { x: 0.2, y: 'FFT' },
            //                { x: 0.25, y: 'Maritime' }
            //            ],
            //            id: '2022',
            //            color: '#FF0000'
            //        },
            //        {
            //            data: [
            //                { x: 0.2, y: 'Air' },
            //                { x: 0.5, y: 'OpsCmd' },
            //                { x: 0.7, y: 'FFT' },
            //                { x: 0.1, y: 'Maritime' },
            //                { x: 0.5, y: 'Demo' }
            //            ],
            //            id: '2021'
            //        },
            //    ]
            //}

            this.#preprocessModel(model);
            this.#init();
        }

        #preprocessModel(model) {
            function defaultLabelObject(label) {
                let labelObject = { category: label, label: 'demo' };

                for (var i = 0; i < model.lines.length; i++) {
                    labelObject['value' + i] = 0;
                }

                return labelObject;
            }

            var labels = {};
            var legendLabels = [];
            var labelColors = [];

            // Iterate through series to retrieve data.
            for (var i = 0; i < model.lines.length; i++) {
                let series = model.lines[i];
                legendLabels[i] = series.id;
                labelColors[i] = series.color;

                for (let j = 0; j < series.data.length; j++) {
                    let entry = series.data[j];

                    if (!labels[entry.y]) {
                        labels[entry.y] = defaultLabelObject(entry.y);
                    }

                    labels[entry.y]['value' + i] = entry.x;
                }
            }


            this.model = {
                data: Object.values(labels),
                seriesCount: model.lines.length,
                legendLabels: legendLabels,
                labelColors: labelColors
            };

        }

        #init() {
            am5.ready(() => {

                // Create root element
                var root = am5.Root.new(this.id);

                // Set themes
                root.setThemes([
                    am5themes_Animated.new(root)
                ]);


                // Create chart
                var chart = root.container.children.push(am5xy.XYChart.new(root, {
                    panX: true,
                    panY: true,
                    wheelX: "panX",
                    wheelY: "zoomX",
                    pinchZoomX: true
                }));


                // Add cursor
                var cursor = chart.set("cursor", am5xy.XYCursor.new(root, {
                    behavior: "none"
                }));
                cursor.lineY.set("visible", false);



                // Create axes
                // https://www.amcharts.com/docs/v5/charts/xy-chart/axes/
                var xAxis = chart.xAxes.push(am5xy.CategoryAxis.new(root, {
                    categoryField: "category",
                    renderer: am5xy.AxisRendererX.new(root, {}),
                    tooltip: am5.Tooltip.new(root, {})
                }));

                xAxis.data.setAll(this.model.data);

                var yAxis = chart.yAxes.push(am5xy.ValueAxis.new(root, {
                    renderer: am5xy.AxisRendererY.new(root, {})
                }));

                // Add series

                var createSeries = (name, field, color) => {
                    let options = {
                        name: name,
                        xAxis: xAxis,
                        yAxis: yAxis,
                        stacked: this.options.fill,
                        valueYField: field,
                        sequencedInterpolation: this.options.interpolation,
                        categoryXField: "category",
                        tooltip: am5.Tooltip.new(root, {
                            pointerOrientation: "horizontal",
                            labelText: "[bold]{name}[/]\n{categoryX}: {valueY}"
                        })
                    };

                    if (color) {
                        options.stroke = am5.color(color)
                    }


                    var series = chart.series.push(am5xy.LineSeries.new(root, options));
                     

                    if (this.options.fill) {
                        series.fills.template.setAll({
                            fillOpacity: 0.5,
                            visible: true
                        });
                    }

                    if (this.options.interpolation) {
                        series.bullets.push(function () {
                            return am5.Bullet.new(root, {
                                sprite: am5.Circle.new(root, {
                                    radius: 4,
                                    fill: series.get("fill")                                    
                                })
                            });
                        });
                    }

                    series.data.setAll(this.model.data);
                    series.appear(1000);

                }

                for (let i = 0; i < this.model.seriesCount; i++) {
                    createSeries(this.model.legendLabels[i], "value" + i, this.model.labelColors[i]);
                }


                // Add scrollbar
                // https://www.amcharts.com/docs/v5/charts/xy-chart/scrollbars/
                chart.set("scrollbarX", am5.Scrollbar.new(root, {
                    orientation: "horizontal"
                }));

                // Make stuff animate on load
                // https://www.amcharts.com/docs/v5/concepts/animations/
                chart.appear(1000, 100);

            });
        }
    }


    function createInstance(id, url, options) {
        // Define options for stacked area drawer.
        if (!options) {
            options = {
                fill: true,
                interpolation: false
            };
        }

        return new Promise((resolve, reject) => {
            _core.ajax.request(url, null, {
                success: function (data) {
                    resolve(new StackedAreaInstance(id, data, options));
                }
            })
        });
    }

    return {
        createInstance
    }
}();