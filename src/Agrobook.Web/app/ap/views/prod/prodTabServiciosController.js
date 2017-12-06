/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var prodTabServiciosController = /** @class */ (function () {
        function prodTabServiciosController(config, apService, apQueryService, toasterLite, $routeParams, $mdPanel, loginService) {
            this.config = config;
            this.apService = apService;
            this.apQueryService = apQueryService;
            this.toasterLite = toasterLite;
            this.$routeParams = $routeParams;
            this.$mdPanel = $mdPanel;
            this.loginService = loginService;
            // helpers
            this.momentInstance = moment;
            // Estados
            this.loadingServicios = false;
            this.ocultarEliminados = true;
            this.idProd = this.$routeParams['idProd'];
            var roles = this.config.claims.roles;
            this.puedeCrearServicios = this.loginService.autorizar([roles.Gerente, roles.Tecnico]);
            this.obtenerServicios();
        }
        // Api
        prodTabServiciosController.prototype.nuevoServicio = function () {
            window.location.href = "#!/servicios/" + this.idProd + "/new?tab=resumen&action=new";
        };
        prodTabServiciosController.prototype.irAServicio = function (servicio) {
            window.location.href = "#!/servicios/" + this.idProd + "/" + servicio.id;
        };
        prodTabServiciosController.prototype.toggleMostrarEliminados = function () {
            this.ocultarEliminados = !this.ocultarEliminados;
        };
        // Privados
        prodTabServiciosController.prototype.obtenerServicios = function () {
            var _this = this;
            this.loadingServicios = true;
            this.apQueryService.getServiciosPorProd(this.idProd, new common.callbackLite(function (value) {
                _this.servicios = value.data;
                _this.loadingServicios = false;
            }, function (reason) {
                _this.loadingServicios = false;
            }));
        };
        prodTabServiciosController.$inject = ['config', 'apService', 'apQueryService', 'toasterLite', '$routeParams', '$mdPanel', 'loginService'];
        return prodTabServiciosController;
    }());
    apArea.prodTabServiciosController = prodTabServiciosController;
})(apArea || (apArea = {}));
//# sourceMappingURL=prodTabServiciosController.js.map