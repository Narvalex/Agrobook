/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var prodTabParcelasController = (function () {
        function prodTabParcelasController(config, apService, apQueryService, toasterLite, $routeParams) {
            this.config = config;
            this.apService = apService;
            this.apQueryService = apQueryService;
            this.toasterLite = toasterLite;
            this.$routeParams = $routeParams;
            this.mostrarForm = false;
            this.idProd = this.$routeParams['idProd'];
            this.obtenerParcelasDelProd();
        }
        // Api
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
        // Privados
        prodTabParcelasController.prototype.registrarNuevaParcela = function () {
            var _this = this;
            this.apService.registrarNuevaParcela(this.parcelaObject, new common.callbackLite(function (value) {
                _this.resetForm();
                _this.parcelas.push(value.data);
                _this.toasterLite.success('Parcela creada');
            }, function (reason) {
                _this.resetForm();
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
            this.apQueryService.gerParcelasDelProd(this.idProd, new common.callbackLite(function (response) {
                _this.parcelas = response.data;
            }, function (reason) { }));
        };
        return prodTabParcelasController;
    }());
    prodTabParcelasController.$inject = ['config', 'apService', 'apQueryService', 'toasterLite', '$routeParams'];
    apArea.prodTabParcelasController = prodTabParcelasController;
})(apArea || (apArea = {}));
//# sourceMappingURL=prodTabParcelasController.js.map