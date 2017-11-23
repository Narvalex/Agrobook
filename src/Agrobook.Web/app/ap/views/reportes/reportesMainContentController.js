/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var reportesMainContentController = (function () {
        function reportesMainContentController(config, loginService, toasterLite) {
            this.config = config;
            this.loginService = loginService;
            this.toasterLite = toasterLite;
        }
        reportesMainContentController.prototype.getReporteListaDeProductores = function () {
            window.open('./report/lista-de-productores', '_blank', '');
        };
        return reportesMainContentController;
    }());
    reportesMainContentController.$inject = ['config', 'loginService', 'toasterLite'];
    apArea.reportesMainContentController = reportesMainContentController;
})(apArea || (apArea = {}));
//# sourceMappingURL=reportesMainContentController.js.map