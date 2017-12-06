/// <reference path="../../../../_all.ts" />
var apArea;
(function (apArea) {
    var precioFormDialogController = /** @class */ (function () {
        function precioFormDialogController($mdDialog, $rootScope, $scope, nf, apService, $timeout, config, toasterLite) {
            this.$mdDialog = $mdDialog;
            this.$rootScope = $rootScope;
            this.$scope = $scope;
            this.nf = nf;
            this.apService = apService;
            this.$timeout = $timeout;
            this.config = config;
            this.toasterLite = toasterLite;
            this.working = false;
            this.apScope = this.$rootScope;
            this.servicio = this.apScope.servicioActual;
            this.inicializar();
        }
        //----------------------------
        // Public API
        //----------------------------
        precioFormDialogController.prototype.cancelar = function () {
            this.$mdDialog.cancel();
        };
        precioFormDialogController.prototype.checkIfEnter = function ($event) {
            if ($event.keyCode === this.config.keyCodes.enter)
                this.fijarOAjustarPrecio();
        };
        precioFormDialogController.prototype.fijarOAjustarPrecio = function () {
            var _this = this;
            this.working = true;
            if (this.servicio.tienePrecio) {
                this.apService.ajustarPrecio(this.servicio.id, this.precioTotal, new common.callbackLite(function (value) {
                    _this.$mdDialog.hide(_this.precioTotal);
                    _this.working = false;
                }, function (error) {
                    _this.toasterLite.error('Hubo un error al intentar ajustar el precio');
                    _this.working = false;
                }));
            }
            else {
                this.apService.fijarPrecio(this.servicio.id, this.precioTotal, new common.callbackLite(function (value) {
                    _this.$mdDialog.hide(_this.precioTotal);
                    _this.working = false;
                }, function (error) {
                    _this.toasterLite.error('Hubo un error al intentar fijar el precio');
                    _this.working = false;
                }));
            }
        };
        //---------------------------
        // Private
        //---------------------------
        precioFormDialogController.prototype.inicializar = function () {
            var _this = this;
            this.$timeout(function () {
                _this.$scope.$apply(function () {
                    var e = document.getElementById('precioInput');
                    e.focus();
                });
            }, 750);
            this.ajustarDesdeElTotal = false;
            this.hectareas = this.nf.parseCommaAsDecimalSeparatorToUSNumber(this.servicio.hectareas);
            var self = this;
            this.$scope.$watch(angular.bind(this.$scope, function () { return _this.precioInput; }), function (newValue, oldValue) {
                self.calcularYMostrarPrecio();
            });
            this.$scope.$watch(angular.bind(this.$scope, function () { return _this.ajustarDesdeElTotal; }), function (newValue, oldValue) {
                self.calcularYMostrarPrecio();
            });
            if (this.servicio.tienePrecio) {
                this.precioInput = this.servicio.precioTotal;
            }
            else {
                this.precioInput = undefined;
                this.precioLabel = '0';
            }
        };
        precioFormDialogController.prototype.calcularYMostrarPrecio = function () {
            // Mostrando el input formateado
            var precioInput;
            if (this.precioInput === undefined)
                precioInput = 0;
            else {
                precioInput = this.nf.parseCommaAsDecimalSeparatorToUSNumber(this.precioInput);
                // Hay que ver la necesidad, si formatear o no
                var lastChar = this.precioInput[this.precioInput.length - 1];
                if (lastChar !== ',')
                    this.precioInput = this.nf.formatFromUSNumber(precioInput);
            }
            // Mostrando el label calculado
            var precioLabel;
            if (this.ajustarDesdeElTotal) {
                precioLabel = precioInput / this.hectareas;
                this.precioTotal = precioInput;
            }
            else {
                precioLabel = precioInput * this.hectareas;
                this.precioTotal = precioLabel;
            }
            this.precioLabel = this.nf.formatFromUSNumber(precioLabel);
        };
        precioFormDialogController.$inject = ['$mdDialog', '$rootScope', '$scope', 'numberFormatter', 'apService', '$timeout', 'config', 'toasterLite'];
        return precioFormDialogController;
    }());
    apArea.precioFormDialogController = precioFormDialogController;
})(apArea || (apArea = {}));
//# sourceMappingURL=precioFormDialogController.js.map