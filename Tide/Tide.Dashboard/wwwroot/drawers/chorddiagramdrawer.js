const ChordDiagramDrawer = function () {

    class ChordDiagramInstance {
        model = {}

    //var model = {
    //    connections: [
    //        {
    //            from: "Air Force", // Focus Area 1
    //            to: "Comms", // Focus Area 2
    //            value: 30, // Value of the connection
    //            valueLabel: "valueLabel",
    //            additionalText: [
    //                {
    //                    label: "additionalTextLabel",
    //                    value: "additionalTextValue",
    //                },
    //                {
    //                    label: "additionalTextLabel",
    //                    value: "additionalTextValue",
    //                },
    //            ],
    //        },
    //        {
    //            from: "Air Fryer", // Focus Area 1
    //            to: "D", // Focus Area 2
    //            value: 29, // Value of the connection
    //            additionalText: [
    //                {
    //                    label: "additionalTextLabel",
    //                    value: "additionalTextValue",
    //                },
    //                {
    //                    label: "additionalTextLabel",
    //                    value: "additionalTextValue",
    //                },
    //            ],
    //        },
    //        {
    //            from: "Comms", // Focus Area 1
    //            to: "D", // Focus Area 2
    //            value: 10, // Value of the connection
    //        },
    //    ],
    //};


        constructor(id, model, options) {
            this.id = id;
            this.options = options;

            this.#preprocessModel(model);

            this.#init();
        }

        #preprocessModel(model) {
            // Preprocess data
            // Append additionalText
            model.connections.forEach((connection) => {
                var newAdditionalText = " ";
                if (!connection.additionalText) {
                    connection.additionalText = "";
                } else {
                    if (typeof connection.additionalText !== 'string') {
                        connection.additionalText.forEach((text) => {
                            newAdditionalText += text.label + ": " + text.value + " ";
                        });
                        connection.additionalText = newAdditionalText;
                    }
                }
            });

            // Get abbreviations
            // Take the first 2 letters of each word
            model.connections.forEach((connection) => {
                connection.from = connection.from
                    .split(" ")
                    .map((word) => word.substring(0, 2))
                    .join("");
                connection.to = connection.to
                    .split(" ")
                    .map((word) => word.substring(0, 2))
                    .join("");
            });

            this.model = model;
        }

        #init() {

            am5.ready(() => {

                // Create root element
                var root = am5.Root.new(this.id);

                // Set themes
                root.setThemes([am5themes_Animated.new(root)]);

                // Create series
                if (this.options.directed) {
                    var series = root.container.children.push(
                        am5flow.ChordDirected.new(root, {
                            startAngle: 80,
                            padAngle: 1,
                            linkHeadRadius: undefined,
                            sourceIdField: "from",
                            targetIdField: "to",
                            valueField: "value",
                        })
                    );

                    series.nodes.labels.template.setAll({
                        textType: "radial",
                        centerX: 0,
                        fontSize: 9,
                    });
                } else {
                    this.#preprocessModel(this.model);

                    var series = root.container.children.push(
                        am5flow.ChordDirected.new(root, {
                            sourceIdField: "from",
                            targetIdField: "to",
                            valueField: "value",
                            sort: "ascending",
                            tooltip: am5.Tooltip.new(root, {
                                labelText: `{from} - {to}: {valueLabel} {value}{additionalText}`,
                            }),
                        })
                    );

                    series.bullets.push(function (_root, _series, dataItem) {
                        var bullet = am5.Bullet.new(root, {
                            locationY: Math.random(),
                            sprite: am5.Circle.new(root, {
                                radius: 5,
                                fill: dataItem.get("source").get("fill"),
                            }),
                        });

                        bullet.animate({
                            key: "locationY",
                            to: 1,
                            from: 0,
                            duration: Math.random() * 1000 + 2000,
                            loops: Infinity,
                        });

                        return bullet;
                    });

                    series.nodes.labels.template.setAll({
                        textType: "regular",
                        fill: root.interfaceColors.get("background"),
                        fontSize: "1.1em",
                        radius: -5,
                    });

                    series.nodes.bullets.push(function (_root, _series, dataItem) {
                        return am5.Bullet.new(root, {
                            sprite: am5.Circle.new(root, {
                                radius: 20,
                                fill: dataItem.get("fill"),
                            }),
                        });
                    });

                    series.children.moveValue(series.bulletsContainer, 0);
                }

                series.links.template.set("fillStyle", "source");

                series.nodes.get("colors").set("step", 2);

                // Set data
                series.data.setAll(this.model.connections);

                // Make stuff animate on load
                series.appear(1000, 100);

            });
        }
    }


    function createInstance(id, url, options) {
        if (!options) {
            options = {
                directed: false
            };
        }

        return new Promise((resolve, reject) => {
            _core.ajax.request(url, null, {
                success: function (data) {
                    resolve(new ChordDiagramInstance(id, data, options));
                }
            })
        });
    }

    return {
        createInstance
    }
}();