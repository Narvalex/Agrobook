/// <reference path="../../../../_all.ts" />
var apArea;
(function (apArea) {
    var precioFormDialogController = (function () {
        function precioFormDialogController($mdDialog, $rootScope, $scope, nf) {
            this.$mdDialog = $mdDialog;
            this.$rootScope = $rootScope;
            this.$scope = $scope;
            this.nf = nf;
            this.apScope = $rootScope;
            this.servicio = this.apScope.servicioActual;
            this.inicializar();
        }
        //----------------------------
        // Public API
        //----------------------------
        precioFormDialogController.prototype.cancelar = function () {
            this.$mdDialog.cancel();
        };
        precioFormDialogController.prototype.fijarOAjustarPrecio = function () {
        };
        //---------------------------
        // Private
        //---------------------------
        precioFormDialogController.prototype.inicializar = function () {
            var _this = this;
            this.ajustarDesdeElTotal = true;
            if (this.servicio.tienePrecio) {
            }
            else {
                this.precioInput = undefined;
                this.precioLabel = '0';
            }
            this.hectareas = this.nf.parseNumberWithCommaAsDecimalSeparator(this.servicio.hectareas);
            var self = this;
            this.$scope.$watch(angular.bind(this.$scope, function () { return _this.precioInput; }), function (newValue, oldValue) {
                self.calcularYMostrarPrecio();
            });
            this.$scope.$watch(angular.bind(this.$scope, function () { return _this.ajustarDesdeElTotal; }), function (newValue, oldValue) {
                self.calcularYMostrarPrecio();
            });
        };
        precioFormDialogController.prototype.calcularYMostrarPrecio = function () {
            // Mostrando el input formateado
            var precioInput;
            if (this.precioInput === undefined)
                precioInput = 0;
            else {
                precioInput = this.nf.parseNumberWithCommaAsDecimalSeparator(this.precioInput);
                // Hay que ver la necesidad, si formatear o no
                var lastChar = this.precioInput[this.precioInput.length - 1];
                if (lastChar !== ',')
                    this.precioInput = this.nf.formatFromUSNumber(precioInput);
            }
            // Mostrando el label calculado
            var precioLabel;
            if (this.ajustarDesdeElTotal) {
                precioLabel = precioInput / this.hectareas;
            }
            else {
                precioLabel = precioInput * this.hectareas;
            }
            this.precioLabel = this.nf.formatFromUSNumber(precioLabel);
        };
        return precioFormDialogController;
    }());
    precioFormDialogController.$inject = ['$mdDialog', '$rootScope', '$scope', 'numberFormatter'];
    apArea.precioFormDialogController = precioFormDialogController;
})(apArea || (apArea = {}));
//# sourceMappingURL=precioFormDialogController.js.map