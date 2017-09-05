/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var serviciosTabResumenController = (function () {
        function serviciosTabResumenController(config, apService, apQueryService, toasterLite, $routeParams, $rootScope, $scope, awService) {
            var _this = this;
            this.config = config;
            this.apService = apService;
            this.apQueryService = apQueryService;
            this.toasterLite = toasterLite;
            this.$routeParams = $routeParams;
            this.$rootScope = $rootScope;
            this.$scope = $scope;
            this.awService = awService;
            this.submitting = false;
            this.eliminando = false;
            this.restaurando = false;
            this.loading = false;
            // Objetos---------------------------------------
            this.momentInstance = moment;
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
        serviciosTabResumenController.prototype.enableEditMode = function () {
            window.location.replace("#!/servicios/" + this.idProd + "/" + this.idServicio + "?tab=resumen&action=edit");
        };
        serviciosTabResumenController.prototype.cancelar = function () {
            if (this.action === 'new')
                window.location.replace("#!/prod/" + this.idProd);
            else if (this.action === 'edit')
                window.location.replace("#!/servicios/" + this.idProd + "/" + this.idServicio + "?tab=resumen&action=view");
        };
        serviciosTabResumenController.prototype.eliminar = function () {
            var _this = this;
            this.eliminando = true;
            this.apService.eliminarServicio(this.idServicio, new common.callbackLite(function (value) {
                _this.servicio.eliminado = true;
                _this.eliminando = false;
            }, function (reason) {
                _this.eliminando = false;
            }));
        };
        serviciosTabResumenController.prototype.restaurar = function () {
            var _this = this;
            this.restaurando = true;
            this.apService.eliminarServicio(this.idServicio, new common.callbackLite(function (value) {
                _this.servicio.eliminado = false;
                _this.restaurando = false;
            }, function (reason) {
                _this.restaurando = false;
            }));
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
            if (this.contratoSeleccionado.idOrg !== this.orgConContratosSeleccionada.org.id) {
                this.toasterLite.error('Debe seleccionar un contrato válido');
                return;
            }
            this.submitting = true;
            var servicio = new apArea.servicioDto(null, this.contratoSeleccionado.id, this.contratoSeleccionado.display, this.orgConContratosSeleccionada.org.id, this.orgConContratosSeleccionada.org.display, this.idProd, this.fechaSeleccionada);
            switch (this.action) {
                case 'new':
                    this.registrarNuevoServicio(servicio);
                    break;
                case 'edit':
                    this.actualizarDatosBasicos(servicio);
                    break;
            }
        };
        // Privados
        serviciosTabResumenController.prototype.cargarDatosSegunEstado = function () {
            var _this = this;
            this.action = this.$routeParams['action'] === undefined ? 'view' : this.$routeParams['action'];
            if (this.action === 'new') {
                this.recuperarYEstablecerContratos();
            }
            if (this.action === 'edit') {
                this.recuperarServicio(function () { return _this.recuperarYEstablecerContratos(); });
            }
            else if (this.action === 'view') {
                this.recuperarServicio();
            }
        };
        serviciosTabResumenController.prototype.recuperarYEstablecerContratos = function () {
            var _this = this;
            this.loading = true;
            this.apQueryService.getOrgsConContratos(this.idProd, new common.callbackLite(function (value) {
                _this.orgsConContratos = value.data;
                if (_this.action == 'edit') {
                    // Setear en el combo la org actual;
                    for (var i = 0; i < _this.orgsConContratos.length; i++) {
                        var orgConContratos_1 = _this.orgsConContratos[i];
                        if (orgConContratos_1.org.id === _this.servicio.idOrg) {
                            _this.orgConContratosSeleccionada = orgConContratos_1;
                            break;
                        }
                    }
                    // Setear en el combo el contrato actual
                    for (var i = 0; i < _this.orgConContratosSeleccionada.contratos.length; i++) {
                        var contrato = _this.orgConContratosSeleccionada.contratos[i];
                        if (contrato.id === _this.servicio.idContrato) {
                            _this.contratoSeleccionado = contrato;
                            break;
                        }
                    }
                    // Setear la fecha original del servicio
                    _this.fechaSeleccionada = _this.servicio.fecha;
                }
                _this.loading = false;
            }, function (reason) {
                _this.loading = false;
            }));
        };
        serviciosTabResumenController.prototype.recuperarServicio = function (callback) {
            var _this = this;
            if (callback === void 0) { callback = function () { }; }
            this.loading = true;
            this.apQueryService.getServicio(this.idServicio, new common.callbackLite(function (value) {
                _this.servicio = value.data;
                _this.loading = false;
                callback();
            }, function (reason) {
                _this.loading = false;
            }));
        };
        serviciosTabResumenController.prototype.registrarNuevoServicio = function (servicio) {
            var _this = this;
            this.apService.registrarNuevoServicio(servicio, new common.callbackLite(function (value) {
                _this.toasterLite.success('El servicio se registró con el id ' + value.data);
                _this.action = 'view';
                servicio.id = value.data;
                _this.submitting = false;
                _this.$rootScope.$broadcast(_this.config.eventIndex.ap_servicios.nuevoServicioCreado, servicio);
                _this.servicio = servicio;
                window.location.replace("#!/servicios/" + _this.idProd + "/" + _this.idServicio + "?tab=resumen&action=view");
            }, function (reason) {
                _this.submitting = false;
            }));
        };
        serviciosTabResumenController.prototype.actualizarDatosBasicos = function (servicio) {
            var _this = this;
            this.apService.actualizarServicio(servicio, new common.callbackLite(function (value) {
                _this.toasterLite.success('Los datos básicos del servicio han sido actualizados');
                _this.action = 'view';
                _this.servicio = servicio;
                _this.submitting = false;
                window.location.replace("#!/servicios/" + _this.idProd + "/" + _this.idServicio + "?tab=resumen&action=view");
            }, function (reason) {
                _this.submitting = false;
            }));
        };
        serviciosTabResumenController.prototype.awInit = function () {
            this.awTitle = 'Documento del informe final.';
            this.awAllowUpload = true;
            this.awUploadLink = 'Levantar archivo...';
        };
        serviciosTabResumenController.prototype.awPrepareFiles = function (element) {
            this.awService.resetFileInput();
            var vm = angular.element(this)[0];
            vm.$scope.$apply(function (scope) {
                vm.awFileUnits = vm.awService.prepareFiles(element.files, vm.awFileUnits);
            });
        };
        return serviciosTabResumenController;
    }());
    serviciosTabResumenController.$inject = ['config', 'apService', 'apQueryService', 'toasterLite', '$routeParams', '$rootScope', '$scope', 'awService'];
    apArea.serviciosTabResumenController = serviciosTabResumenController;
})(apArea || (apArea = {}));
//# sourceMappingURL=serviciosTabResumenController.js.map