/// <reference path="../../../../_all.ts" />

module apArea {
    export class precioFormDialogController {
        static $inject = ['$mdDialog', '$rootScope', '$scope', 'numberFormatter'];

        private apScope: IApScope;

        constructor(
            private $mdDialog: angular.material.IDialogService,
            private $rootScope: angular.IRootScopeService,
            private $scope: angular.IScope,
            private nf: common.numberFormatter
        ) {
            this.apScope = $rootScope as IApScope;
            this.servicio = this.apScope.servicioActual;

            this.inicializar();
        }

        // states
        ajustarDesdeElTotal: boolean = false;

        // dtos
        servicio: servicioDto;
        hectareas: number;

        // Two-way
        precioInput: number;
        precioLabel: string;

        //----------------------------
        // Public API
        //----------------------------

        inicializar() {
            if (this.servicio.tienePrecio) {
            }
            else {
                this.precioInput = null;
                this.precioLabel = '0';
            }

            this.hectareas = this.nf.parseNumberWithCommaAsDecimalSeparator(this.servicio.hectareas);

            var self = this;
            this.$scope.$watch(angular.bind(this.$scope, () => this.precioInput),
                (newValue: number, oldValue: number) => {
                    if (newValue === undefined && oldValue.toString().length !== 1) {
                        self.precioInput = oldValue;
                        return;
                    }
                    self.calcularYMostrarPrecio();
                });
            this.$scope.$watch(angular.bind(this.$scope, () => this.ajustarDesdeElTotal),
                (newValue: boolean, oldValue: boolean) => {
                    self.calcularYMostrarPrecio();
                });
        }

        private calcularYMostrarPrecio() {
            let precioInput = this.precioInput;
            let precioLabel: number;
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
        }

        cancelar() {
            this.$mdDialog.cancel();
        }
    }
}