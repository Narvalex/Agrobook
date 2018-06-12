/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var reportesMainContentController = (function () {
        function reportesMainContentController(config, loginService, toasterLite) {
            this.config = config;
            this.loginService = loginService;
            this.toasterLite = toasterLite;
        }
        reportesMainContentController.prototype.getReporte = function (url) {
            window.open(url, '_blank', '');
        };
        return reportesMainContentController;
    }());
    reportesMainContentController.$inject = ['config', 'loginService', 'toasterLite'];
    apArea.reportesMainContentController = reportesMainContentController;
})(apArea || (apArea = {}));
//# sourceMappingURL=reportesMainContentController.js.map