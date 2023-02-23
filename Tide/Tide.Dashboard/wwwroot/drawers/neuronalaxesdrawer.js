const NeuronalAxesDrawer = function () {

    class NeuronalAxesInstance {
        model = {
            data: []
        };
        id = null;


        constructor(id, model) {
            this.id = id;

            var model = model;

            console.log(model);
/*            {
                data: [
                    {
                        id: "id1",
                        name: "CC-001",
                        description: "2019",
                        value: 500,
                        linkWith:["id2"],
                        children: [
                            {
                                name: "Standards",
                                value: 300,
                                children: [
                                    {
                                        name: "Std1",
                                        description: "descriere",
                                        value: 100 / 10
                                    },
                                    {
                                        name: "Std2",
                                        description: "descriere",
                                        value: 90 / 10
                                    },
                                    {
                                        name: "Std4",
                                        description: "descriere",
                                        value: 90 / 10
                                    },
                                ]
                            },
                         

                        ]
            
                    },
                    {
                        id: "id2",
                        name: "CC-002",
                        description: "2020",
                        value:500
                    
                    }
                ]
            }
            */

            this.#preprocessModel(model);
            this.#init();
        }

        #preprocessModel(model) {
            this.model = model;

        }

        #init() {
            am5.ready(() => {
                var root = am5.Root.new(this.id);

                // Set themes
                // https://www.amcharts.com/docs/v5/concepts/themes/
                root.setThemes([
                    am5themes_Animated.new(root)
                ]);


                let model = this.model.data;
                for (var i = 0; i < model.length; i++) {
                    var cc = model[i];
                    cc.name = cc.name + "\n" + cc.description;

                }
                var data = model;

                var data = {
                    name: "Root",
                    value: 0,
                    children: data
                };



                // Create wrapper container
                var container = root.container.children.push(
                    am5.Container.new(root, {
                        width: am5.percent(100),
                        height: am5.percent(100),
                        layout: root.verticalLayout
                    })
                );

                // Create series
                // https://www.amcharts.com/docs/v5/charts/hierarchy/#Adding
                var series = container.children.push(
                    am5hierarchy.ForceDirected.new(root, {
                        singleBranchOnly: false,
                        downDepth: 1,
                        topDepth: 1,
                        maxRadius: 25,
                        minRadius: 12,
                        valueField: "value",
                        categoryField: "name",
                        childDataField: "children",
                        idField: "id",
                        linkWithStrength: 0.3,
                        linkWithField: "linkWith",
                        manyBodyStrength: -15,
                        centerStrength: 0.5
                    })
                );

                series.get("colors").set("step", 2);

                series.data.setAll([data]);
                series.set("selectedDataItem", series.dataItems[0]);

                series.nodes.template.set("tooltipText", "{description}: [bold]{value}[/]");
                // Make stuff animate on load
                series.appear(1000, 100);

            });
        }
    }


    function createInstance(id, url) {
        return new Promise((resolve, reject) => {
            _core.ajax.request(url, null, {
                success: function (data) {
                    resolve(new NeuronalAxesInstance(id, data));
                }
            })
        });
    }

    return {
        createInstance
    }
}();