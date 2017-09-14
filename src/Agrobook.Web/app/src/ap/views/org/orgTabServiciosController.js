/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var orgTabServiciosController = (function () {
        function orgTabServiciosController($routeParams, apQueryService, $mdSidenav, toasterLite) {
            this.$routeParams = $routeParams;
            this.apQueryService = apQueryService;
            this.$mdSidenav = $mdSidenav;
            this.toasterLite = toasterLite;
            this.ocultarEliminados = true;
            var idOrg = this.$routeParams['idOrg'];
            this.recuperarServiciosPorOrg(idOrg);
        }
        //--------------------------
        // Api
        //--------------------------
        orgTabServiciosController.prototype.nuevoServicio = function () {
            this.$mdSidenav('left').open();
            this.toasterLite.default('Seleccione un productor para poder registrar un servicio', 7000, true, 'top left');
        };
        orgTabServiciosController.prototype.toggleMostrarEliminados = function () {
            this.ocultarEliminados = !this.ocultarEliminados;
        };
        orgTabServiciosController.prototype.irAServicio = function (servicio) {
            window.location.replace("#!/servicios/" + servicio.idProd + "/" + servicio.id);
        };
        //--------------------------
        // Private
        //--------------------------
        orgTabServiciosController.prototype.recuperarServiciosPorOrg = function (idOrg) {
            var _this = this;
            this.loadingServicios = true;
            this.apQueryService.getServiciosPorOrg(idOrg, new common.callbackLite(function (value) {
                _this.servicios = value.data;
                _this.loadingServicios = false;
            }, function (reason) { }));
        };
        return orgTabServiciosController;
    }());
    orgTabServiciosController.$inject = ['$routeParams', 'apQueryService', '$mdSidenav', 'toasterLite'];
    apArea.orgTabServiciosController = orgTabServiciosController;
})(apArea || (apArea = {}));
//# sourceMappingURL=orgTabServiciosController.js.map