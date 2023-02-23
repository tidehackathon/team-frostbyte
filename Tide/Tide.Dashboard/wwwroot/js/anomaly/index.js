var AnomalyPage = function () {

    function init() {
        HorizontalAxesDrawer.createInstance('anomalies', '/anomaly/testinganomalies');
        StackedAreaDrawer.createInstance('cyclesdeviation', '/anomaly/cyclesdeviation');
    }

    return {
        init
    }
}();



$(document).ready(function () {
    AnomalyPage.init();
});