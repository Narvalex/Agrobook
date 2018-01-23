/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var orgTabServiciosController = /** @class */ (function () {
        function orgTabServiciosController($routeParams, apQueryService, $mdSidenav, toasterLite) {
            this.$routeParams = $routeParams;
            this.apQueryService = apQueryService;
            this.$mdSidenav = $mdSidenav;
            this.toasterLite = toasterLite;
            this.ocultarEliminados = true;
            this.orderByDesc = true;
            // objetos
            this.momentInstance = moment;
            var idOrg = this.$routeParams['idOrg'];
            this.recuperarServiciosPorOrg(idOrg);
        }
        //--------------------------
        // Api
        //--------------------------
        orgTabServiciosController.prototype.toogleOrder = function () {
            this.orderByDesc = !this.orderByDesc;
        };
        orgTabServiciosController.prototype.toogleColapsado = function (contrato) {
            for (var i = 0; i < this.contratos.length; i++) {
                var c = this.contratos[i];
                if (c.id === contrato.id) {
                    c.colapsado = !c.colapsado;
                }
            }
        };
        orgTabServiciosController.prototype.nuevoServicio = function () {
            this.$mdSidenav('left').open();
            this.toasterLite.default('Seleccione un productor para poder registrar un servicio', 7000, true, 'top left');
        };
        orgTabServiciosController.prototype.toggleMostrarEliminados = function () {
            this.ocultarEliminados = !this.ocultarEliminados;
        };
        orgTabServiciosController.prototype.irAServicio = function (servicio) {
            window.location.href = "#!/servicios/" + servicio.idProd + "/" + servicio.id;
        };
        //--------------------------
        // Private
        //--------------------------
        orgTabServiciosController.prototype.recuperarServiciosPorOrg = function (idOrg) {
            var _this = this;
            this.loadingServicios = true;
            this.apQueryService.getServiciosPorOrgAgrupadosPorContrato(idOrg, new common.callbackLite(function (value) {
                _this.contratos = value.data;
                _this.loadingServicios = false;
            }, function (reason) { }));
        };
        orgTabServiciosController.$inject = ['$routeParams', 'apQueryService', '$mdSidenav', 'toasterLite'];
        return orgTabServiciosController;
    }());
    apArea.orgTabServiciosController = orgTabServiciosController;
})(apArea || (apArea = {}));
//# sourceMappingURL=orgTabServiciosController.js.map