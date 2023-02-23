
var MultyDomainPage = function () {

    function init() {
        TimelineAxesDrawer.createInstance('chartdivTimeline', '/multidomain/testingtimelinefocusarea');


        CyclesCardComponent.init({
            firstDisplay: function (year) {
                ChordDiagramDrawer.createInstance('famultidomain' + year, '/multidomain/fasconnections?year=' + year);
            }
        });

        StackedAreaDrawer.createInstance('interoperabilitypartial', '/multidomain/partialinteroperability', { fill: false, interpolation: true });

        StackedAreaDrawer.createInstance('interoperabilityfull', '/multidomain/interoperability', { fill: false, interpolation: true });
    }

    return {
        init
    }
}();



$(document).ready(function () {
    MultyDomainPage.init();
});