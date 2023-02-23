const RadarChartDrawer = function () {

    class RadarChartInstance {
        model = {
            data: [],
        };


        constructor(id, model) {
            this.id = id;
            this.#preprocessModel(model);
            this.#init();
        }

        #preprocessModel(model) {
            // Get labels;
            var labels = [];
            for (const values of model["groups"]) {
                for (var value of values["values"]) {
                    const label = value["label"];
                    if (!labels.map((e) => e.label).includes(label)) {
                        labels.push({
                            label: label,
                        });
                    }
                }
            }


            // Normalize data, modify it and attach new value as shownValue
            for (const values of model["groups"]) {
                const id = values["id"];
                for (var value of values["values"]) {
                    // Remove values that are 0 from the modelCopy
                    if (value["value"] == 0) {
                        //delete value["value"];
                        // values["values"].splice(values["values"].indexOf(value), 1);
                    } else {
                        const threshold = value["threshold"];
                        const actualValue = (value["value"] / threshold) * 100;
                        value["shownValue"] = id + ": " + value["value"];
                        value["value"] = actualValue;
                    }
                }
            }

            model.labels = labels;
            console.log({ model });
            this.model = model;
        }

        #init() {
            am5.ready(() => {

                // Create root element
                var root = am5.Root.new(this.id);

                // Set theme
                root.setThemes([am5themes_Animated.new(root)]);

                // Create chart
                var chart = root.container.children.push(
                    am5radar.RadarChart.new(root, {
                        panX: false,
                        panY: false,
                        wheelX: "panX",
                        wheelY: "zoomX",
                    })
                );

                // Add cursor
                var cursor = chart.set(
                    "cursor",
                    am5radar.RadarCursor.new(root, {
                        behavior: "zoomX",
                    })
                );

                cursor.lineY.set("visible", false);

                // Create axes and their renderers
                var xRenderer = am5radar.AxisRendererCircular.new(root, {});

                xRenderer.labels.template.setAll({
                    radius: 20,
                    fontSize: 14,
                    textType: "adjusted",
                    // Allign labels to be horizontal
                });
                xRenderer.grid.template.setAll({
                    // opacity: 0
                });

                var xAxis = chart.xAxes.push(
                    am5xy.CategoryAxis.new(root, {
                        categoryField: "label",
                        renderer: xRenderer,
                        tooltip: am5.Tooltip.new(root, {}),
                    })
                );

                var yRenderer = am5radar.AxisRendererRadial.new(root, {});
                yRenderer.grid.template.setAll({
                    // opacity: 0
                });

                var yAxis = chart.yAxes.push(
                    am5xy.ValueAxis.new(root, {
                        min: 0,
                        max: 100,
                        visible: false,
                        renderer: yRenderer,
                    })
                );

                // Create series
                var _series = [];

                for (const values of this.model["groups"]) {
                    let random_color =
                        "#" + Math.floor(Math.random() * 16777215).toString(16);

                    var to_add = chart.series.push(
                        am5radar.RadarLineSeries.new(root, {
                            name: values["id"],
                            xAxis: xAxis,
                            yAxis: yAxis,
                            valueYField: "value",
                            categoryXField: "label",
                            tooltip: am5.Tooltip.new(root, {
                                labelText: "{shownValue} / {threshold}",
                            }),
                            stroke: am5.color(random_color),
                        })
                    );

                    to_add.strokes.template.setAll({
                        fill: am5.color(random_color),
                        fillOpacity: 0.6,
                        strokeWidth: 3,
                    });

                    // Set color only for this series

                    // to_add.bullets.push(function () {
                    //   return am5.Bullet.new(root, {
                    //     sprite: am5.Circle.new(root, {
                    //       radius: 3,
                    //       fill: am5.color(0x00),
                    //     }),
                    //   });
                    // });

                    // Add legend
                    var legend = chart.children.push(am5.Legend.new(root, {}));
                    legend.data.setAll(chart.series.values);

                    to_add.data.setAll(values["values"]);

                    _series.push(to_add);

                }

                xAxis.data.setAll(this.model.labels);

                for (var series of _series) series.appear(1000);

                chart.appear(1000, 100);

            });
        }
    }


    function createInstance(id, url) {
        return new Promise((resolve, reject) => {
            _core.ajax.request(url, null, {
                success: function (data) {
                    resolve(new RadarChartInstance(id, data));
                }
            })
        });
    }

    return {
        createInstance
    }
}();