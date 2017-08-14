/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var prodTabParcelasController = (function () {
        function prodTabParcelasController(config, apService, toasterLite) {
            this.config = config;
            this.apService = apService;
            this.toasterLite = toasterLite;
            this.creandoNuevaParcela = false;
        }
        // Listas
        // Api
        prodTabParcelasController.prototype.habilitarCreacionDeNuevaParcela = function () {
            this.creandoNuevaParcela = true;
            setTimeout(function () {
                return document.getElementById('nuevaParcelaInput').focus();
            }, 0);
        };
        prodTabParcelasController.prototype.checkIfEnter = function ($event) {
            var keyCode = $event.keyCode;
            if (keyCode === this.config.keyCodes.enter)
                this.registrarNuevaParcela();
            else if (keyCode === this.config.keyCodes.esc) {
                this.nuevaParcelaInput = '';
                this.cancelarCreacionDeNuevaParcela();
            }
        };
        prodTabParcelasController.prototype.registrarNuevaParcela = function () {
            var _this = this;
            if (this.nuevaParcelaInput.length === 0) {
                this.toasterLite.error("Debe especificar el nombre de la parcela");
                return;
            }
            this.intentandoRegistrarParcela = true;
            this.apService.registrarNuevaParcela(this.nuevaParcelaInput, new common.callbackLite(function (value) {
                _this.intentandoRegistrarParcela = false;
                _this.creandoNuevaParcela = false;
                _this.toasterLite.success('Parcela creada');
            }, function (reason) {
                _this.intentandoRegistrarParcela = false;
                _this.creandoNuevaParcela = false;
            }));
        };
        prodTabParcelasController.prototype.cancelarCreacionDeNuevaParcela = function () {
            this.creandoNuevaParcela = false;
        };
        return prodTabParcelasController;
    }());
    prodTabParcelasController.$inject = ['config', 'apService', 'toasterLite'];
    apArea.prodTabParcelasController = prodTabParcelasController;
})(apArea || (apArea = {}));
//# sourceMappingURL=prodTabParcelasController.js.map