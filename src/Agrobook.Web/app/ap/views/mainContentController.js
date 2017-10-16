/// <reference path="../../_all.ts" />
var apArea;
(function (apArea) {
    var mainContentController = (function () {
        function mainContentController(apQueryService, toasterLite) {
            this.apQueryService = apQueryService;
            this.toasterLite = toasterLite;
            // Model
            this.momentJs = moment;
            this.getServicios();
        }
        mainContentController.prototype.irAlServicio = function (servicio) {
            window.location.href = "./index.html#!/servicios/" + servicio.idProd + "/" + servicio.id + "?tab=resumen&action=view";
        };
        mainContentController.prototype.getServicios = function () {
            var _this = this;
            this.loading = true;
            this.apQueryService.getUltimosServicios(30, new common.callbackLite(function (value) {
                if (value.data.length === 0) {
                    _this.sinServicios = true;
                }
                else {
                    _this.servicios = value.data;
                }
                _this.loading = false;
            }, function (reason) {
                _this.loading = false;
                _this.toasterLite.error('Hubo un error al obtener los últimos servicios', _this.toasterLite.delayForever);
            }));
        };
        return mainContentController;
    }());
    mainContentController.$inject = ['apQueryService', 'toasterLite'];
    apArea.mainContentController = mainContentController;
})(apArea || (apArea = {}));
//# sourceMappingURL=mainContentController.js.map