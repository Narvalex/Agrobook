/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var prodTabParcelasController = /** @class */ (function () {
        function prodTabParcelasController(config, apService, apQueryService, toasterLite, $routeParams, $mdPanel, loginService, numberFormatter) {
            this.config = config;
            this.apService = apService;
            this.apQueryService = apQueryService;
            this.toasterLite = toasterLite;
            this.$routeParams = $routeParams;
            this.$mdPanel = $mdPanel;
            this.loginService = loginService;
            this.numberFormatter = numberFormatter;
            this.ocultarEliminados = true;
            // Listas
            this.parcelas = [];
            this.mostrarForm = false;
            this.idProd = this.$routeParams['idProd'];
            var roles = config.claims.roles;
            this.tienePermiso = this.loginService.autorizar([roles.Gerente, roles.Tecnico]);
            this.obtenerParcelasDelProd();
            this.obtenerDepartamentos();
        }
        // Api
        prodTabParcelasController.prototype.toggleMostrarEliminados = function () {
            this.ocultarEliminados = !this.ocultarEliminados;
        };
        prodTabParcelasController.prototype.habilitarCreacionDeNuevaParcela = function () {
            this.formIsEditing = false;
            this.mostrarFormYHacerFocus();
        };
        prodTabParcelasController.prototype.habilitarEdicion = function (parcela) {
            this.formIsEditing = true;
            this.parcelaObject = new apArea.edicionParcelaDto();
            this.parcelaObject.display = parcela.display;
            this.parcelaObject.hectareas = parcela.hectareas;
            this.parcelaObject.idProd = parcela.idProd;
            this.parcelaObject.idParcela = parcela.id;
            this.departamentoSeleccionado = parcela.idDepartamento;
            this.establecerDistritos();
            this.distritoSeleccionado = parcela.idDistrito;
            this.mostrarFormYHacerFocus();
        };
        prodTabParcelasController.prototype.mostrarOpciones = function ($event, parcela) {
            var position = this.$mdPanel.newPanelPosition()
                .relativeTo($event.srcElement)
                .addPanelPosition(this.$mdPanel.xPosition.ALIGN_START, this.$mdPanel.yPosition.BELOW);
            var config = {
                attachTo: angular.element(document.body),
                controller: panelMenuController,
                controllerAs: 'vm',
                hasBackdrop: true,
                templateUrl: './views/prod/menu-panel-tab-parcelas.html',
                position: position,
                trapFocus: true,
                locals: {
                    'parcela': parcela,
                    'parent': this
                },
                panelClass: 'menu-panel-container',
                openFrom: $event,
                focusOnOpen: true,
                zIndex: 150,
                disableParentScroll: true,
                clickOutsideToClose: true,
                escapeToClose: true,
            };
            this.$mdPanel.open(config);
        };
        prodTabParcelasController.prototype.checkIfEnter = function ($event) {
            var keyCode = $event.keyCode;
            if (keyCode === this.config.keyCodes.enter)
                this.registrarNuevaParcela();
            else if (keyCode === this.config.keyCodes.esc) {
                this.cancel();
            }
        };
        prodTabParcelasController.prototype.submit = function () {
            if (this.parcelaObject.display.length === 0) {
                this.toasterLite.error("Debe especificar el nombre de la parcela");
                return;
            }
            this.submitting = true;
            if (this.formIsEditing)
                this.editarParcela();
            else
                this.registrarNuevaParcela();
        };
        prodTabParcelasController.prototype.cancel = function () {
            this.resetForm();
        };
        prodTabParcelasController.prototype.eliminar = function (parcela) {
            var _this = this;
            this.apService.eliminarParcela(parcela.idProd, parcela.id, new common.callbackLite(function (value) {
                for (var i = 0; i < _this.parcelas.length; i++) {
                    if (_this.parcelas[i].id === parcela.id) {
                        _this.parcelas[i].eliminado = true;
                        break;
                    }
                }
                _this.toasterLite.info('Parcela elimnada');
            }, function (reason) { }));
        };
        prodTabParcelasController.prototype.restaurar = function (parcela) {
            var _this = this;
            this.apService.restaurarParcela(parcela.idProd, parcela.id, new common.callbackLite(function (value) {
                for (var i = 0; i < _this.parcelas.length; i++) {
                    if (_this.parcelas[i].id === parcela.id) {
                        _this.parcelas[i].eliminado = false;
                        break;
                    }
                }
                _this.toasterLite.success('Parcela restaurada');
            }, function (reason) { }));
        };
        // Privados
        prodTabParcelasController.prototype.registrarNuevaParcela = function () {
            var _this = this;
            this.parcelaObject.idProd = this.idProd;
            this.apService.registrarNuevaParcela(this.parcelaObject.idProd, this.parcelaObject.display, this.numberFormatter.parseCommaAsDecimalSeparatorToUSNumber(this.parcelaObject.hectareas), this.departamentoSeleccionado, this.distritoSeleccionado, new common.callbackLite(function (value) {
                var parcela = new apArea.parcelaDto(value.data, _this.parcelaObject.idProd, _this.parcelaObject.display, _this.parcelaObject.hectareas, _this.departamentoSeleccionado, null, _this.distritoSeleccionado, null, false);
                _this.parcelas.push(parcela);
                _this.toasterLite.success('Parcela creada');
                _this.resetForm();
            }, function (reason) {
                _this.submitting = false;
                _this.toasterLite.error('Hubo un error al registrar la parcela. Verifique que el nombre ya no exista por favor');
            }));
        };
        prodTabParcelasController.prototype.editarParcela = function () {
            var _this = this;
            this.apService.editarParcela(this.parcelaObject.idProd, this.parcelaObject.idParcela, this.parcelaObject.display, this.numberFormatter.parseCommaAsDecimalSeparatorToUSNumber(this.parcelaObject.hectareas), this.departamentoSeleccionado, this.distritoSeleccionado, new common.callbackLite(function (value) {
                // eventual consistency handling before reseting form
                for (var i = 0; i < _this.parcelas.length; i++) {
                    if (_this.parcelas[i].id === _this.parcelaObject.idParcela) {
                        _this.parcelas[i].hectareas = _this.parcelaObject.hectareas;
                        _this.parcelas[i].display = _this.parcelaObject.display;
                        break;
                    }
                }
                _this.toasterLite.success('Parcela editada');
                _this.resetForm();
            }, function (reason) {
                _this.submitting = false;
                _this.toasterLite.error('Hubo un error al editar la parcela');
            }));
        };
        prodTabParcelasController.prototype.mostrarFormYHacerFocus = function () {
            this.mostrarForm = true;
            setTimeout(function () {
                return document.getElementById('parcelaInput').focus();
            }, 0);
        };
        prodTabParcelasController.prototype.resetForm = function () {
            this.mostrarForm = false;
            this.submitting = false;
            this.parcelaObject = undefined;
        };
        prodTabParcelasController.prototype.obtenerParcelasDelProd = function () {
            var _this = this;
            this.apQueryService.getParcelasDelProd(this.idProd, new common.callbackLite(function (response) {
                response.data.forEach(function (x) {
                    x.hectareas = _this.numberFormatter.formatFromUSNumber(parseFloat(x.hectareas));
                });
                _this.parcelas = response.data;
            }, function (reason) { return _this.toasterLite.error('Hubo un error al obtener parcelas'); }));
        };
        prodTabParcelasController.prototype.obtenerDepartamentos = function () {
            var _this = this;
            this.apQueryService.getDepartamentos(new common.callbackLite(function (response) {
                _this.departamentos = response.data;
            }, function (reason) { return _this.toasterLite.error('Hubo un error al obtener los departamentos'); }));
        };
        prodTabParcelasController.prototype.establecerDistritos = function () {
            for (var i = 0; i < this.departamentos.length; i++) {
                var depto = this.departamentos[i];
                if (depto.id === this.departamentoSeleccionado) {
                    this.distritos = depto.distritos;
                    break;
                }
            }
        };
        prodTabParcelasController.$inject = ['config', 'apService', 'apQueryService', 'toasterLite', '$routeParams', '$mdPanel', 'loginService',
            'numberFormatter'];
        return prodTabParcelasController;
    }());
    apArea.prodTabParcelasController = prodTabParcelasController;
    var panelMenuController = /** @class */ (function () {
        function panelMenuController(mdPanelRef) {
            this.mdPanelRef = mdPanelRef;
        }
        panelMenuController.prototype.editar = function () {
            var _this = this;
            this.mdPanelRef.close().then(function (value) {
                _this.parent.habilitarEdicion(_this.parcela);
            })
                .finally(function () { return _this.mdPanelRef.destroy(); });
        };
        panelMenuController.prototype.eliminar = function () {
            var _this = this;
            this.mdPanelRef.close().then(function (value) {
                _this.parent.eliminar(_this.parcela);
            })
                .finally(function () { return _this.mdPanelRef.destroy(); });
        };
        panelMenuController.prototype.restaurar = function () {
            var _this = this;
            this.mdPanelRef.close().then(function (value) {
                _this.parent.restaurar(_this.parcela);
            })
                .finally(function () { return _this.mdPanelRef.destroy(); });
        };
        panelMenuController.prototype.cancelar = function () {
            var _this = this;
            this.mdPanelRef.close().finally(function () { return _this.mdPanelRef.destroy(); });
        };
        panelMenuController.$inject = ['mdPanelRef'];
        return panelMenuController;
    }());
})(apArea || (apArea = {}));
//# sourceMappingURL=prodTabParcelasController.js.map