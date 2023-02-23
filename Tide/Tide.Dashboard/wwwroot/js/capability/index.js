var CapabilityPage = function () {
    const context = {
        id: null
    }


    function init() {

        context.id = parseInt($('#capability_id').val());

        RadarChartDrawer.createInstance('focusareaevolution', '/capability/evolution?capabilityId=' + context.id);
        StackedAreaDrawer.createInstance('interoperabilitypartial', '/capability/partialinteroperability?capabilityId=' + context.id, { fill: false, interpolation: true});
        StackedAreaDrawer.createInstance('interoperabilityfull', '/capability/interoperability?capabilityId=' + context.id, { fill: false, interpolation: true });
    }

    return {
        init
    }
}();



$(document).ready(function () {
    CapabilityPage.init();
});