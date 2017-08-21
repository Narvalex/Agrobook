/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var prodTabParcelasController = (function () {
        function prodTabParcelasController(config, apService, apQueryService, toasterLite, $routeParams, $mdPanel) {
            this.config = config;
            this.apService = apService;
            this.apQueryService = apQueryService;
            this.toasterLite = toasterLite;
            this.$routeParams = $routeParams;
            this.$mdPanel = $mdPanel;
            // Estados
            this.ocultarEliminados = true;
            this.mostrarForm = false;
            this.idProd = this.$routeParams['idProd'];
            this.obtenerParcelasDelProd();
        }
        // Api
        prodTabParcelasController.prototype.toggleMostrarEliminados = function () {
            this.ocultarEliminados = !this.ocultarEliminados;
        };
        prodTabParcelasController.prototype.habilitarCreacionDeNuevaParcela = function () {
            this.formIsEditing = false;
            this.mostrarFormYHacerFocus();
        };
        prodTabParcelasController.prototype.habilitarEdicionDeParcela = function (parcela) {
            this.formIsEditing = true;
            this.parcelaObject = new apArea.edicionParcelaDto();
            this.parcelaObject.display = parcela.display;
            this.parcelaObject.hectareas = parcela.hectareas;
            this.parcelaObject.idProd = parcela.idProd;
            this.parcelaObject.idParcela = parcela.id;
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
                templateUrl: './dist/ap/views/prod/menu-panel-tab-parcelas.html',
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
            this.mostrarForm = false;
            this.resetForm();
        };
        prodTabParcelasController.prototype.eliminar = function (parcela) {
            var _this = this;
            this.apService.eliminar(parcela.id, new common.callbackLite(function (value) {
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
            this.apService.restaurar(parcela.id, new common.callbackLite(function (value) {
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
            this.apService.registrarNuevaParcela(this.parcelaObject, new common.callbackLite(function (value) {
                _this.resetForm();
                _this.parcelas.push(value.data);
                _this.toasterLite.success('Parcela creada');
            }, function (reason) {
                _this.toasterLite.error('Hubo un error al registrar la parcela. Verifique que el nombre ya no exista por favor');
            }));
        };
        prodTabParcelasController.prototype.editarParcela = function () {
            var _this = this;
            this.apService.editarParcela(this.parcelaObject, new common.callbackLite(function (value) {
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
                _this.resetForm();
            }));
        };
        prodTabParcelasController.prototype.mostrarFormYHacerFocus = function () {
            this.mostrarForm = true;
            setTimeout(function () {
                return document.getElementById('parcelaInput').focus();
            }, 0);
        };
        prodTabParcelasController.prototype.resetForm = function () {
            this.parcelaObject = undefined;
            this.submitting = false;
            this.mostrarForm = false;
        };
        prodTabParcelasController.prototype.obtenerParcelasDelProd = function () {
            var _this = this;
            this.apQueryService.getParcelasDelProd(this.idProd, new common.callbackLite(function (response) {
                _this.parcelas = response.data;
            }, function (reason) { }));
        };
        return prodTabParcelasController;
    }());
    prodTabParcelasController.$inject = ['config', 'apService', 'apQueryService', 'toasterLite', '$routeParams', '$mdPanel'];
    apArea.prodTabParcelasController = prodTabParcelasController;
    var panelMenuController = (function () {
        function panelMenuController(mdPanelref) {
            this.mdPanelref = mdPanelref;
        }
        panelMenuController.prototype.editar = function () {
            var _this = this;
            this.mdPanelref.close().then(function (value) {
                _this.parent.habilitarEdicionDeParcela(_this.parcela);
            })
                .finally(function () { return _this.mdPanelref.destroy(); });
        };
        panelMenuController.prototype.eliminar = function () {
            var _this = this;
            this.mdPanelref.close().then(function (value) {
                _this.parent.eliminar(_this.parcela);
            })
                .finally(function () { return _this.mdPanelref.destroy(); });
        };
        panelMenuController.prototype.restaurar = function () {
            var _this = this;
            this.mdPanelref.close().then(function (value) {
                _this.parent.restaurar(_this.parcela);
            })
                .finally(function () { return _this.mdPanelref.destroy(); });
        };
        panelMenuController.prototype.cancelar = function () {
            var _this = this;
            this.mdPanelref.close().finally(function () { return _this.mdPanelref.destroy(); });
        };
        return panelMenuController;
    }());
    panelMenuController.$inject = ['mdPanelRef'];
})(apArea || (apArea = {}));
//# sourceMappingURL=prodTabParcelasController.js.map