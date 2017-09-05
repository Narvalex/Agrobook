/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var serviciosTabParcelaController = (function () {
        function serviciosTabParcelaController(config, apService, apQueryService, toasterLite, $routeParams, $rootScope, $scope, awService) {
            var _this = this;
            this.config = config;
            this.apService = apService;
            this.apQueryService = apQueryService;
            this.toasterLite = toasterLite;
            this.$routeParams = $routeParams;
            this.$rootScope = $rootScope;
            this.$scope = $scope;
            this.awService = awService;
            // Estados--------------------------------------
            this.loading = false;
            this.tieneParcela = false;
            this.submitting = false;
            this.awFileUnits = [];
            this.idProd = this.$routeParams['idProd'];
            this.idServicio = this.$routeParams['idServicio'];
            this.$scope.$on('$routeUpdate', function (scope, next, current) {
                _this.cargarDatosSegunEstado();
            });
            this.cargarDatosSegunEstado();
            this.awInit();
        }
        // Api
        serviciosTabParcelaController.prototype.actualizar = function () {
            window.location.replace("#!/servicios/" + this.idProd + "/" + this.idServicio + "?tab=parcela&action=edit");
        };
        serviciosTabParcelaController.prototype.cancelar = function () {
            this.parcelaSeleccionada = undefined;
            window.location.replace("#!/servicios/" + this.idProd + "/" + this.idServicio + "?tab=parcela&action=view");
        };
        serviciosTabParcelaController.prototype.submit = function () {
            this.submitting = true;
            if (this.parcelaSeleccionada === undefined) {
                this.toasterLite.error('Debe seleccionar una parcela');
                this.submitting = false;
                return;
            }
            else {
                if (this.parcelaSeleccionada.id === this.servicio.parcelaId) {
                    this.toasterLite.info('La parcela es la misma que antes. No hay cambios');
                    this.submitting = false;
                    this.cancelar();
                }
            }
            if (this.tieneParcela)
                this.cambiarParcela();
            else
                this.especificarParcela();
        };
        // Privados
        serviciosTabParcelaController.prototype.cargarDatosSegunEstado = function () {
            this.action = this.$routeParams['action'] === undefined ? 'view' : this.$routeParams['action'];
            if (this.servicio === undefined)
                this.recuperarServicioYLaParcelaSiTiene();
            switch (this.action) {
                case 'edit':
                    this.recuperarParcelas();
                    break;
                case 'view':
                    break;
            }
        };
        serviciosTabParcelaController.prototype.recuperarServicioYLaParcelaSiTiene = function () {
            var _this = this;
            this.loading = true;
            this.apQueryService.getServicio(this.idServicio, new common.callbackLite(function (value) {
                _this.servicio = value.data;
                _this.tieneParcela = _this.servicio.parcelaId !== undefined && _this.servicio.parcelaId !== null;
                if (_this.tieneParcela) {
                    _this.apQueryService.getParcelasDelProd;
                }
                else
                    _this.loading = false;
            }, function (reason) {
                _this.loading = false;
            }));
        };
        serviciosTabParcelaController.prototype.recuperarParcelas = function () {
            var _this = this;
            this.loading = true;
            this.apQueryService.getParcelasDelProd(this.idProd, new common.callbackLite(function (value) {
                _this.parcelas = value.data.filter(function (x) { return !x.eliminado; });
                if (_this.tieneParcela) {
                    for (var i = 0; i < _this.parcelas.length; i++) {
                        var parcela = _this.parcelas[i];
                        if (parcela.id === _this.servicio.parcelaId) {
                            _this.parcelaSeleccionada = parcela;
                            break;
                        }
                    }
                }
                _this.loading = false;
            }, function (reason) {
                _this.loading = false;
            }));
        };
        serviciosTabParcelaController.prototype.especificarParcela = function () {
            var _this = this;
            var parcela = this.parcelaSeleccionada;
            this.apService.especificarParcelaDelServicio(this.idServicio, parcela, new common.callbackLite(function (value) {
                _this.servicio.parcelaId = parcela.id;
                _this.servicio.parcelaDisplay = parcela.display;
                _this.parcela = parcela;
                _this.toasterLite.success('Parcela especificada correctamente');
                _this.tieneParcela = true;
                _this.submitting = false;
                window.location.replace("#!/servicios/" + _this.idProd + "/" + _this.idServicio + "?tab=parcela&action=view");
            }, function (reason) {
                _this.submitting = false;
            }));
        };
        serviciosTabParcelaController.prototype.cambiarParcela = function () {
            var _this = this;
            var parcela = this.parcelaSeleccionada;
            this.apService.cambiarParcelaDelServicio(this.idServicio, parcela, new common.callbackLite(function (value) {
                _this.servicio.parcelaId = parcela.id;
                _this.servicio.parcelaDisplay = parcela.display;
                _this.parcela = parcela;
                _this.toasterLite.success('La parcela ha sido cambiada');
                _this.submitting = false;
                window.location.replace("#!/servicios/" + _this.idProd + "/" + _this.idServicio + "?tab=parcela&action=view");
            }, function (reason) {
                _this.submitting = false;
            }));
        };
        serviciosTabParcelaController.prototype.awInit = function () {
            this.awTitle = 'Mapas de límites y puntos georeferenciados';
            this.awAllowUpload = true;
            this.awUploadLink = 'Levantar archivo...';
        };
        serviciosTabParcelaController.prototype.awPrepareFiles = function (element) {
            this.awService.resetFileInput();
            var vm = angular.element(this)[0];
            vm.$scope.$apply(function (scope) {
                vm.awFileUnits = vm.awService.prepareFiles(element.files, vm.awFileUnits);
            });
        };
        return serviciosTabParcelaController;
    }());
    serviciosTabParcelaController.$inject = ['config', 'apService', 'apQueryService', 'toasterLite', '$routeParams', '$rootScope', '$scope', 'awService'];
    apArea.serviciosTabParcelaController = serviciosTabParcelaController;
})(apArea || (apArea = {}));
//# sourceMappingURL=serviciosTabParcelaController.js.map