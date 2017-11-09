/// <reference path="../../../../_all.ts" />
var apArea;
(function (apArea) {
    var precioFormDialogController = (function () {
        function precioFormDialogController($mdDialog, $rootScope, $scope, nf) {
            this.$mdDialog = $mdDialog;
            this.$rootScope = $rootScope;
            this.$scope = $scope;
            this.nf = nf;
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
                this.precioInput = null;
                this.precioLabel = '0';
            }
            this.hectareas = this.nf.parseNumberWithCommaAsDecimalSeparator(this.servicio.hectareas);
            var self = this;
            this.$scope.$watch(angular.bind(this.$scope, function () { return _this.precioInput; }), function (newValue, oldValue) {
                if (newValue === undefined && oldValue.toString().length !== 1) {
                    self.precioInput = oldValue;
                    return;
                }
                self.calcularYMostrarPrecio();
            });
            this.$scope.$watch(angular.bind(this.$scope, function () { return _this.ajustarDesdeElTotal; }), function (newValue, oldValue) {
                self.calcularYMostrarPrecio();
            });
        };
        precioFormDialogController.prototype.calcularYMostrarPrecio = function () {
            var precioInput = this.precioInput;
            var precioLabel;
            if (precioInput === null || precioInput === undefined) {
                precioInput = 0;
            }
            if (this.ajustarDesdeElTotal) {
                precioLabel = precioInput / this.hectareas;
            }
            else {
                precioLabel = precioInput * this.hectareas;
            }
            this.precioLabel = this.nf.formatFromUSNumber(precioLabel);
        };
        precioFormDialogController.prototype.cancelar = function () {
            this.$mdDialog.cancel();
        };
        return precioFormDialogController;
    }());
    precioFormDialogController.$inject = ['$mdDialog', '$rootScope', '$scope', 'numberFormatter'];
    apArea.precioFormDialogController = precioFormDialogController;
})(apArea || (apArea = {}));
//# sourceMappingURL=precioFormDialogController.js.map