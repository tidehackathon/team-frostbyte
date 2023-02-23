
var MultyDomainPage = function () {

    function init() {
        TimelineAxesDrawer.createInstance('chartdivTimeline', '/multidomain/testingtimelinefocusarea');
    }

    return {
        init
    }
}();



$(document).ready(function () {
    MultyDomainPage.init();
});