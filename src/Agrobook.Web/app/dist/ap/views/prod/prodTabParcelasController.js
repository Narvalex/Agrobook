/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var prodTabParcelasController = (function () {
        function prodTabParcelasController() {
            this.creandoNuevaParcela = false;
        }
        // Objetos
        // Listas
        // Api
        prodTabParcelasController.prototype.habilitarCreacionDeNuevaParcela = function () {
            this.creandoNuevaParcela = true;
            setTimeout(function () {
                return document.getElementById('nuevaParcelaInput').focus();
            }, 0);
        };
        prodTabParcelasController.prototype.cancelarCreacionDeNuevaParcela = function () {
            this.creandoNuevaParcela = false;
        };
        return prodTabParcelasController;
    }());
    prodTabParcelasController.$inject = [];
    apArea.prodTabParcelasController = prodTabParcelasController;
})(apArea || (apArea = {}));
//# sourceMappingURL=prodTabParcelasController.js.map