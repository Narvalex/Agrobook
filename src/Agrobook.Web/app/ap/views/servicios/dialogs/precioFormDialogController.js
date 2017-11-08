/// <reference path="../../../../_all.ts" />
var apArea;
(function (apArea) {
    var precioFormDialogController = (function () {
        function precioFormDialogController($mdDialog, $rootScope, $scope) {
            this.$mdDialog = $mdDialog;
            this.$rootScope = $rootScope;
            this.$scope = $scope;
            // states
            this.ajustarDesdeElTotal = false;
            this.apScope = $rootScope;
            this.servicio = this.apScope.servicioActual;
            this.inicializar();
        }
        //----------------------------
        // Public API
        //----------------------------
        precioFormDialogController.prototype.inicializar = function () {
            var _this = this;
            if (this.servicio.tienePrecio) {
            }
            else {
                this.precioInput = 0;
                this.precioLabel = 0;
            }
            this.hectareas = parseFloat(this.servicio.hectareas);
            this.$scope.$watch(angular.bind(this.$scope, function () { return _this.precioInput; }), function (newValue, oldValue) {
                _this.calcularPrecioDisplay();
            });
        };
        precioFormDialogController.prototype.calcularPrecioDisplay = function () {
            var precioInput = this.precioInput;
            if (precioInput === null || precioInput === undefined) {
                precioInput = 0;
            }
            if (this.ajustarDesdeElTotal) {
                this.precioLabel = precioInput / this.hectareas;
            }
            else {
                this.precioLabel = precioInput * this.hectareas;
            }
        };
        precioFormDialogController.prototype.cancelar = function () {
            this.$mdDialog.cancel();
        };
        return precioFormDialogController;
    }());
    precioFormDialogController.$inject = ['$mdDialog', '$rootScope', '$scope'];
    apArea.precioFormDialogController = precioFormDialogController;
})(apArea || (apArea = {}));
//# sourceMappingURL=precioFormDialogController.js.map