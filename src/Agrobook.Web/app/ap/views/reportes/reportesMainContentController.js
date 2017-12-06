/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var reportesMainContentController = /** @class */ (function () {
        function reportesMainContentController(config, loginService, toasterLite) {
            this.config = config;
            this.loginService = loginService;
            this.toasterLite = toasterLite;
        }
        reportesMainContentController.prototype.getReporte = function (url) {
            window.open(url, '_blank', '');
        };
        reportesMainContentController.$inject = ['config', 'loginService', 'toasterLite'];
        return reportesMainContentController;
    }());
    apArea.reportesMainContentController = reportesMainContentController;
})(apArea || (apArea = {}));
//# sourceMappingURL=reportesMainContentController.js.map