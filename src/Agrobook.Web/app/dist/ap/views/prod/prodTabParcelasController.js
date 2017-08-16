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
            this.creandoNuevaParcela = false;
            this.idProd = this.$routeParams['idProd'];
            this.obtenerParcelasDelProd();
        }
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
                this.cancelarCreacionDeNuevaParcela();
            }
        };
        prodTabParcelasController.prototype.registrarNuevaParcela = function () {
            var _this = this;
            if (this.nuevaParcela.display.length === 0) {
                this.toasterLite.error("Debe especificar el nombre de la parcela");
                return;
            }
            this.intentandoRegistrarParcela = true;
            this.apService.registrarNuevaParcela(this.nuevaParcela, new common.callbackLite(function (value) {
                _this.resetearNuevaParcelaInput();
                _this.intentandoRegistrarParcela = false;
                _this.creandoNuevaParcela = false;
                _this.parcelas.push(value.data);
                _this.toasterLite.success('Parcela creada');
            }, function (reason) {
                _this.intentandoRegistrarParcela = false;
                _this.creandoNuevaParcela = false;
            }));
        };
        prodTabParcelasController.prototype.cancelarCreacionDeNuevaParcela = function () {
            this.creandoNuevaParcela = false;
            this.resetearNuevaParcelaInput();
        };
        // Privados
        prodTabParcelasController.prototype.resetearNuevaParcelaInput = function () {
            this.nuevaParcela = undefined;
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