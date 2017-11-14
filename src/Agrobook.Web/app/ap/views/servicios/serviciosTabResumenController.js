/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var serviciosTabResumenController = (function () {
        function serviciosTabResumenController(config, apService, apQueryService, toasterLite, $routeParams, $rootScope, $scope, loginService, $mdDialog, $mdMedia, nf) {
            var _this = this;
            this.config = config;
            this.apService = apService;
            this.apQueryService = apQueryService;
            this.toasterLite = toasterLite;
            this.$routeParams = $routeParams;
            this.$rootScope = $rootScope;
            this.$scope = $scope;
            this.loginService = loginService;
            this.$mdDialog = $mdDialog;
            this.$mdMedia = $mdMedia;
            this.nf = nf;
            this.submitting = false;
            this.eliminando = false;
            this.restaurando = false;
            this.loading = false;
            this.tienePermiso = false;
            // Objetos---------------------------------------
            this.momentInstance = moment;
            // Reactors
            this.onServiciosLoadedForDev = function () { };
            this.idProd = this.$routeParams['idProd'];
            this.idServicio = this.$routeParams['idServicio'];
            this.setIdColeccion();
            var roles = config.claims.roles;
            this.tienePermiso = this.loginService.autorizar([roles.Gerente, roles.Tecnico]);
            this.$scope.$on('$routeUpdate', function (scope, next, current) {
                _this.cargarDatosSegunEstado();
            });
            this.$scope.$on(this.config.eventIndex.ap_servicios.cambioDeParcelaEnServicio, function (e, idParcela, parcelaDisplay, hectareas) {
                _this.servicio.parcelaId = idParcela;
                _this.servicio.parcelaDisplay = parcelaDisplay;
                _this.servicio.hectareas = hectareas;
            });
            this.cargarDatosSegunEstado();
            // Esto esta solo para desarrollo.
            //this.onServiciosLoaded = () => setTimeout(() => this.fijarOAjustarPrecio(null, false), 1000);
        }
        // Api
        serviciosTabResumenController.prototype.goToOrg = function () {
            if (!this.tienePermiso) {
                this.toasterLite.info('Usted pertenece a ' + this.servicio.orgDisplay);
                return;
            }
            window.location.href = "#!/org/" + this.servicio.idOrg;
        };
        serviciosTabResumenController.prototype.enableEditMode = function () {
            window.location.href = "#!/servicios/" + this.idProd + "/" + this.idServicio + "?tab=resumen&action=edit";
        };
        serviciosTabResumenController.prototype.cancelar = function () {
            if (this.action === 'new')
                window.location.href = "#!/prod/" + this.idProd;
            else if (this.action === 'edit')
                window.location.href = "#!/servicios/" + this.idProd + "/" + this.idServicio + "?tab=resumen&action=view";
        };
        serviciosTabResumenController.prototype.eliminar = function () {
            var _this = this;
            this.eliminando = true;
            this.apService.eliminarServicio(this.idServicio, new common.callbackLite(function (value) {
                _this.servicio.eliminado = true;
                _this.eliminando = false;
            }, function (reason) {
                _this.eliminando = false;
                _this.toasterLite.error("No se pudo eliminar. Lo sentimos");
            }));
        };
        serviciosTabResumenController.prototype.restaurar = function () {
            var _this = this;
            this.restaurando = true;
            this.apService.restaurarServicio(this.idServicio, new common.callbackLite(function (value) {
                _this.servicio.eliminado = false;
                _this.restaurando = false;
            }, function (reason) {
                _this.restaurando = false;
                _this.toasterLite.error("No se pudo restaurar. Lo sentimos!");
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
            var servicio = new apArea.servicioDto(this.idServicio, this.contratoSeleccionado.id, this.contratoSeleccionado.esAdenda, this.contratoSeleccionado.idContratoDeLaAdenda, this.contratoSeleccionado.display, this.orgConContratosSeleccionada.org.id, this.orgConContratosSeleccionada.org.display, this.idProd, null, this.fechaSeleccionada);
            switch (this.action) {
                case 'new':
                    this.registrarNuevoServicio(servicio);
                    break;
                case 'edit':
                    this.actualizarDatosBasicos(servicio);
                    break;
            }
        };
        serviciosTabResumenController.prototype.fijarOAjustarPrecio = function ($event, esAjuste) {
            var _this = this;
            if (this.servicio.parcelaId === null) {
                this.toasterLite.info("Debe especificar la parcela primero para poder fijar el precio");
                return;
            }
            var scope = this.$rootScope;
            scope.servicioActual = this.servicio;
            this.$mdDialog.show({
                templateUrl: './views/servicios/dialogs/precio-form-dialog.html',
                parent: angular.element(document.body),
                targetEvent: $event,
                controller: apArea.precioFormDialogController,
                controllerAs: 'vm',
                clickOutsideToClose: true,
                fullscreen: this.$mdMedia('xs')
            }).then(function (precioTotal) {
                _this.setPrecio(precioTotal);
                if (_this.servicio.tienePrecio) {
                    _this.toasterLite.success('Precio fijado correctamente');
                }
                else {
                    _this.servicio.tienePrecio = true;
                    _this.toasterLite.success('Precio ajustado correctamente');
                }
            }, function () {
                //console.log('error');
            });
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
                // Esta logica esta aqui para que no se cargue el tab cada vez que se pasa por el 
                if (this.servicio === undefined || this.servicio === null)
                    this.recuperarServicio();
            }
        };
        serviciosTabResumenController.prototype.recuperarYEstablecerContratos = function () {
            var _this = this;
            this.loading = true;
            this.apQueryService.getOrgsConContratosDelProductor(this.idProd, new common.callbackLite(function (value) {
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
                if (_this.servicio.tienePrecio)
                    _this.setPrecio(parseFloat(_this.servicio.precioTotal));
                _this.loading = false;
                callback();
                _this.onServiciosLoadedForDev();
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
                _this.servicio = servicio;
                _this.idServicio = servicio.id;
                _this.setIdColeccion();
                _this.$rootScope.$broadcast(_this.config.eventIndex.ap_servicios.nuevoServicioCreado, servicio);
                //window.location.href = `#!/servicios/${this.idProd}/${this.idServicio}?tab=resumen&action=view`;
            }, function (reason) {
                _this.submitting = false;
            }));
        };
        serviciosTabResumenController.prototype.actualizarDatosBasicos = function (servicio) {
            var _this = this;
            this.apService.editarDatosBasicosDelServicio(servicio, new common.callbackLite(function (value) {
                _this.toasterLite.success('Los datos básicos del servicio han sido actualizados');
                _this.action = 'view';
                _this.servicio = servicio;
                _this.submitting = false;
                window.location.replace("#!/servicios/" + _this.idProd + "/" + _this.idServicio + "?tab=resumen&action=view");
            }, function (reason) {
                _this.toasterLite.error('Hubo un error al intentar editar el servicio. Verifique los datos por favor.');
                _this.submitting = false;
            }));
        };
        serviciosTabResumenController.prototype.setIdColeccion = function () {
            this.idColeccion = this.config.categoriaDeArchivos.servicioDatosBasicos + "-" + this.idServicio;
        };
        serviciosTabResumenController.prototype.setPrecio = function (precioTotal) {
            this.servicio.precioTotal = this.nf.formatFromUSNumber(precioTotal);
            var ha = this.nf.parseCommaAsDecimalSeparatorToUSNumber(this.servicio.hectareas);
            var pph = precioTotal / ha;
            this.servicio.precioPorHectarea = this.nf.formatFromUSNumber(pph);
        };
        return serviciosTabResumenController;
    }());
    serviciosTabResumenController.$inject = ['config', 'apService', 'apQueryService', 'toasterLite', '$routeParams', '$rootScope',
        '$scope', 'loginService', '$mdDialog', '$mdMedia', 'numberFormatter'];
    apArea.serviciosTabResumenController = serviciosTabResumenController;
})(apArea || (apArea = {}));
//# sourceMappingURL=serviciosTabResumenController.js.map