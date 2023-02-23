var NationPage = function () {
    const context = {
        id: null
    }


    function init() {

        context.id = parseInt($('#nation_id').val());

        RadarChartDrawer.createInstance('nationevolution', '/nation/evolution?nationId=' + context.id);
    }

    return {
        init
    }
}();



$(document).ready(function () {
    NationPage.init();
});