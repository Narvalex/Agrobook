/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var serviciosTabResumenController = (function () {
        function serviciosTabResumenController(config, apService, apQueryService, toasterLite, $routeParams, $mdPanel, $rootScope, $scope) {
            var _this = this;
            this.config = config;
            this.apService = apService;
            this.apQueryService = apQueryService;
            this.toasterLite = toasterLite;
            this.$routeParams = $routeParams;
            this.$mdPanel = $mdPanel;
            this.$rootScope = $rootScope;
            this.$scope = $scope;
            this.submitting = false;
            this.idProd = this.$routeParams['idProd'];
            this.action = this.$routeParams['action'] === undefined ? 'view' : this.$routeParams['action'];
            this.$scope.$on('$routeUpdate', function (scope, next, current) {
                _this.action = _this.$routeParams['action'];
            });
            this.recuperarContratos();
        }
        // Api
        serviciosTabResumenController.prototype.cancelar = function () {
            window.location.replace("#!/prod/" + this.idProd);
        };
        serviciosTabResumenController.prototype.submit = function () {
            if (this.contratoSeleccionado === undefined || this.contratoSeleccionado.id === undefined) {
                this.toasterLite.error("Debe seleccionar un contrato");
                return;
            }
            if (this.fechaSeleccionada === undefined) {
                this.toasterLite.error("Debe seleccionar la fecha del contrato");
                return;
            }
            this.submitting = true;
            switch (this.action) {
                case 'new':
                    this.registrarNuevoServicio();
                    break;
            }
        };
        // Privados
        serviciosTabResumenController.prototype.recuperarContratos = function () {
            var _this = this;
            this.apQueryService.getOrgsConContratos(this.idProd, new common.callbackLite(function (value) {
                _this.orgsConContratos = value.data;
            }, function (reason) {
            }));
        };
        serviciosTabResumenController.prototype.registrarNuevoServicio = function () {
            var _this = this;
            var servicio = new apArea.servicioDto(null, this.contratoSeleccionado.id, this.orgConContratosSeleccionada.org.id, this.idProd, this.fechaSeleccionada);
            this.apService.registrarNuevoServicio(new apArea.servicioDto(null, this.contratoSeleccionado.id, this.orgConContratosSeleccionada.org.id, this.idProd, this.fechaSeleccionada), new common.callbackLite(function (value) {
                _this.toasterLite.success('El servicio se registró con el id ' + value.data);
                _this.action = 'view';
                servicio.id = value.data;
                _this.submitting = false;
                _this.$rootScope.$broadcast(_this.config.eventIndex.ap_servicios.nuevoServicioCreado, servicio);
            }, function (reason) {
                _this.submitting = false;
            }));
        };
        return serviciosTabResumenController;
    }());
    serviciosTabResumenController.$inject = ['config', 'apService', 'apQueryService', 'toasterLite', '$routeParams', '$mdPanel', '$rootScope', '$scope'];
    apArea.serviciosTabResumenController = serviciosTabResumenController;
})(apArea || (apArea = {}));
//# sourceMappingURL=serviciosTabResumenController.js.map