/// <reference path="../../_all.ts" />
var apArea;
(function (apArea) {
    var mainContentController = /** @class */ (function () {
        function mainContentController(apQueryService, toasterLite, loginService, loginQueryService, config) {
            this.apQueryService = apQueryService;
            this.toasterLite = toasterLite;
            this.loginService = loginService;
            this.loginQueryService = loginQueryService;
            this.config = config;
            this.puedeVerElMainDeAp = false;
            // Model
            this.momentJs = moment;
            var claims = this.config.claims;
            this.puedeVerElMainDeAp = this.loginService.autorizar([claims.roles.Gerente, claims.roles.Tecnico]);
            if (this.puedeVerElMainDeAp)
                this.getServicios();
            else {
                window.location.href = "./index.html#!/prod/" + this.loginQueryService.tryGetLocalLoginInfo().usuario;
            }
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
        mainContentController.$inject = ['apQueryService', 'toasterLite', 'loginService', 'loginQueryService', 'config'];
        return mainContentController;
    }());
    apArea.mainContentController = mainContentController;
})(apArea || (apArea = {}));
//# sourceMappingURL=mainContentController.js.map