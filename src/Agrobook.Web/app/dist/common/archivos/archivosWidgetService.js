/// <reference path="../../_all.ts" />
// this will be a sample
var common;
(function (common) {
    var archivosWidgetService = (function () {
        function archivosWidgetService() {
        }
        archivosWidgetService.prototype.selectFiles = function () {
            setTimeout(function () { return document.getElementById('awFileInputBtn').click(); }, 0);
        };
        return archivosWidgetService;
    }());
    archivosWidgetService.$inject = [];
    common.archivosWidgetService = archivosWidgetService;
})(common || (common = {}));
//# sourceMappingURL=archivosWidgetService.js.map