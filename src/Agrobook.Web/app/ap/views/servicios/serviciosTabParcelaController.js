/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var serviciosTabParcelaController = (function () {
        function serviciosTabParcelaController(config, apService, apQueryService, toasterLite, $routeParams, $rootScope, $scope) {
            var _this = this;
            this.config = config;
            this.apService = apService;
            this.apQueryService = apQueryService;
            this.toasterLite = toasterLite;
            this.$routeParams = $routeParams;
            this.$rootScope = $rootScope;
            this.$scope = $scope;
            // Estados--------------------------------------
            this.loading = false;
            this.tieneParcela = false;
            this.submitting = false;
            this.idProd = this.$routeParams['idProd'];
            this.idColeccion = this.config.categoriaDeArchivos.servicioParcelas + "-" + this.idProd;
            this.idServicio = this.$routeParams['idServicio'];
            this.$scope.$on('$routeUpdate', function (scope, next, current) {
                _this.cargarDatosSegunEstado();
            });
            this.cargarDatosSegunEstado();
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
                    return;
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
            if (this.servicio === undefined && this.idServicio.toLowerCase() !== 'new')
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
                    // esta logica no tiene sentido... solamente esta por las hectareas
                    _this.apQueryService.getParcela(_this.servicio.parcelaId, new common.callbackLite(function (value) {
                        _this.parcela = value.data;
                        _this.loading = false;
                    }, function (reason) {
                        _this.loading = false;
                    }));
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
                _this.$rootScope.$broadcast(_this.config.eventIndex.ap_servicios.cambioDeParcelaEnServicio, parcela.display);
                window.location.replace("#!/servicios/" + _this.idProd + "/" + _this.idServicio + "?tab=parcela&action=view");
                _this.submitting = false;
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
                _this.$rootScope.$broadcast(_this.config.eventIndex.ap_servicios.cambioDeParcelaEnServicio, parcela.display);
                window.location.replace("#!/servicios/" + _this.idProd + "/" + _this.idServicio + "?tab=parcela&action=view");
                _this.submitting = false;
            }, function (reason) {
                _this.submitting = false;
            }));
        };
        return serviciosTabParcelaController;
    }());
    serviciosTabParcelaController.$inject = ['config', 'apService', 'apQueryService', 'toasterLite', '$routeParams', '$rootScope', '$scope'];
    apArea.serviciosTabParcelaController = serviciosTabParcelaController;
})(apArea || (apArea = {}));
//# sourceMappingURL=serviciosTabParcelaController.js.map