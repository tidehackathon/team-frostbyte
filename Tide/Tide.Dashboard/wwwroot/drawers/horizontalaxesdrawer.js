const HorizontalAxesDrawer = function () {

    class HorizontalAxesInstance {
        model = {
            data: [],
            seriesCount: 0,
            legendLabels: []
        };
        id = null;


        constructor(id, model) {
            this.id = id;

            //model = {
            //    lines: [
            //        {
            //            data: [
            //                { x: 0.3, y: 'Air' },
            //                { x: 0.4, y: 'OpsCmd' },
            //                { x: 0.2, y: 'FFT' },
            //                { x: 0.25, y: 'Maritime' }
            //            ],
            //            id: '2022'
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

            // Iterate through series to retrieve data.
            for (var i = 0; i < model.lines.length; i++) {
                let series = model.lines[i];
                legendLabels[i] = series.id;

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
                legendLabels: legendLabels
            };
        }

        #init() {
            am5.ready(() => {

                var data = this.model.data;

                // Create root element
                var root = am5.Root.new(this.id);
                root.setThemes([
                    am5themes_Animated.new(root)
                ]);

                // Create chart
                var chart = root.container.children.push(
                    am5xy.XYChart.new(root, {
                        panX: false,
                        panY: true,
                        wheelX: "none",
                        wheelY: "zoomY",
                        arrangeTooltips: false,
                        pinchZoomY: true
                    })
                );

                // make x axes stack
                chart.bottomAxesContainer.set("layout", root.horizontalLayout);

                // Create axes
                var yRenderer = am5xy.AxisRendererY.new(root, {
                    minGridDistance: 25
                });

                yRenderer.labels.template.setAll({
                    multiLocation: 0.5,
                    location: 0.5,
                    paddingRight: 15
                });

                yRenderer.grid.template.set("location", 0.5);

                var yAxis = chart.yAxes.push(
                    am5xy.CategoryAxis.new(root, {
                        categoryField: "category",
                        tooltip: am5.Tooltip.new(root, {}),
                        renderer: yRenderer
                    })
                );

                yAxis.data.setAll(data);

                var xRenderer = am5xy.AxisRendererX.new(root, {
                    minGridDistance: 40
                });

                xRenderer.labels.template.setAll({
                    rotation: -90,
                    centerY: am5.p50
                });

                var xAxis = chart.xAxes.push(
                    am5xy.ValueAxis.new(root, {
                        renderer: xRenderer,
                        tooltip: am5.Tooltip.new(root, {
                            animationDuration: 0
                        }),
                        marginLeft: 3
                    })
                );

                // Add series
                function createSeries(field, legendLabel) {

                    var series = chart.series.push(
                        am5xy.LineSeries.new(root, {
                            xAxis: xAxis,
                            yAxis: yAxis,
                            valueXField: field,
                            label: legendLabel,
                            categoryYField: "category",
                            sequencedInterpolation: true,
                            tooltip: am5.Tooltip.new(root, {
                                pointerOrientation: "horizontal",
                                labelText: "{valueX}"
                            })
                        })
                    );

                    series.bullets.push(function () {
                        return am5.Bullet.new(root, {
                            locationX: 1,
                            locationY: 0.5,
                            sprite: am5.Circle.new(root, {
                                radius: 4,
                                fill: series.get("fill")
                            })
                        });
                    });

                    series.data.setAll(data);
                    series.appear();

                    return series;
                }

                for (let i = 0; i < this.model.seriesCount; i++) {
                    createSeries("value" + i, this.model.legendLabels[i]);
                }

                // Add cursor
                var cursor = chart.set("cursor", am5xy.XYCursor.new(root, {
                    behavior: "none",
                    yAxis: yAxis
                }));

                var legend = chart.children.push(am5.Legend.new(root, {
                    nameField: 'label',
                    x: am5.p50, 
                    y: 0,
                    centerX: am5.p50
                }));

                legend.data.setAll(chart.series.values);

                // Make stuff animate on load
                chart.appear(1000, 100);

            });
        }
    }


    function createInstance(id, url) {
        return new Promise((resolve, reject) => {
            _core.ajax.request(url, null, {
                success: function (data) {
                    resolve(new HorizontalAxesInstance(id, data));
                }
            })
        });
    }

    return {
        createInstance
    }
}();