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
        ajustarDesdeElTotal: boolean;

        // dtos
        servicio: servicioDto;
        hectareas: number;

        // Two-way
        precioInput: string; 
        precioLabel: string;

        //----------------------------
        // Public API
        //----------------------------

        cancelar() {
            this.$mdDialog.cancel();
        }

        fijarOAjustarPrecio() {
        }

        //---------------------------
        // Private
        //---------------------------

        private inicializar() {
            this.ajustarDesdeElTotal = true;

            if (this.servicio.tienePrecio) {
            }
            else {
                this.precioInput = undefined;
                this.precioLabel = '0';
            }

            this.hectareas = this.nf.parseNumberWithCommaAsDecimalSeparator(this.servicio.hectareas);

            var self = this;
            this.$scope.$watch(angular.bind(this.$scope, () => this.precioInput),
                (newValue: string, oldValue: string) => {
                    self.calcularYMostrarPrecio();
                });
            this.$scope.$watch(angular.bind(this.$scope, () => this.ajustarDesdeElTotal),
                (newValue: boolean, oldValue: boolean) => {
                    self.calcularYMostrarPrecio();
                });
        }

        private calcularYMostrarPrecio() {
            // Mostrando el input formateado
            let precioInput: number;
            if (this.precioInput === undefined)
                precioInput = 0;
            else {
                precioInput = this.nf.parseNumberWithCommaAsDecimalSeparator(this.precioInput);

                // Hay que ver la necesidad, si formatear o no
                let lastChar = this.precioInput[this.precioInput.length - 1];
                if (lastChar !== ',')
                    this.precioInput = this.nf.formatFromUSNumber(precioInput);
            }

            // Mostrando el label calculado
            let precioLabel: number;
            if (this.ajustarDesdeElTotal) {
                precioLabel = precioInput / this.hectareas;
            }
            else {
                precioLabel = precioInput * this.hectareas;
            }

            this.precioLabel = this.nf.formatFromUSNumber(precioLabel);
        }
    }
}