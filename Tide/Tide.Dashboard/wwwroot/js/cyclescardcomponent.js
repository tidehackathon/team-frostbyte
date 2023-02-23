var CyclesCardComponent = function () {

    class CyclesCardInstance {
        dom = null;
        jdom = null;
        loadedPages = {};
        pages = {};
        options = {
            firstDisplay: null
        };

        constructor(dom, options) {
            this.dom = dom;
            this.jdom = $(dom);
            this.options = options;

            this.#init();
            this.#resetContent();
        }

        #init() {
            var select = this.jdom.find('[data-cwix-select="true"]');

            select.on('change', () => {
                this.#resetContent();
            });
        }

        #resetContent() {
            // hide all tabs
            this.jdom.find('[data-cwix-id]').hide();

            var select = this.jdom.find('[data-cwix-select="true"]');

            let selectedYear = select.val();

            // display tab
            this.jdom.find('[data-cwix-id="' + selectedYear + '"]').show();

            setTimeout(() => {
                if (!this.loadedPages[selectedYear]) {
                    this.loadedPages[selectedYear] = true;

                    this.options.firstDisplay(selectedYear);
                }
            }, 50);
        }
    }


    function init(options) {
        // Find all card components in view
        if (!options) {
            options = {
                firstDisplay: year => { }
            }
        }

        var cards = $('[data-cwix-card="true"]');
        var instances = [];
        for (let i = 0; i < cards.length; i++) {
            // create instance
            instances.push(new CyclesCardInstance(cards[i], options));
        }

        return instances;
    }

    return {
        init: init
    }

}();