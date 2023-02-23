var NationPage = function () {
    const context = {
        id: null
    }


    function init() {

        context.id = parseInt($('#nation_id').val());

        RadarChartDrawer.createInstance('nationevolution', '/nation/evolution?nationId=' + context.id);
        StackedAreaDrawer.createInstance('interoperabilitypartial', '/nation/partialinteroperability?nationId=' + context.id, { fill: false, interpolation: true });
        StackedAreaDrawer.createInstance('interoperabilityfull', '/nation/interoperability?nationId=' + context.id, { fill: false, interpolation: true });
    }

    return {
        init
    }
}();



$(document).ready(function () {
    NationPage.init();
});