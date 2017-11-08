/// <reference path="../../../../_all.ts" />

module apArea {
    export class precioFormDialogController {
        static $inject = ['$mdDialog', '$rootScope', '$scope'];

        private apScope: IApScope;

        constructor(
            private $mdDialog: angular.material.IDialogService,
            private $rootScope: angular.IRootScopeService,
            private $scope: angular.IScope
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
        precioLabel: number;

        //----------------------------
        // Public API
        //----------------------------

        inicializar() {
            if (this.servicio.tienePrecio) {
            }
            else {
                this.precioInput = 0;
                this.precioLabel = 0;
            }

            this.hectareas = parseFloat(this.servicio.hectareas);

            this.$scope.$watch(angular.bind(this.$scope, () => this.precioInput),
                (newValue: number, oldValue: number) => {
                    this.calcularPrecioDisplay();
                });
        }

        private calcularPrecioDisplay() {
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
        }

        cancelar() {
            this.$mdDialog.cancel();
        }
    }
}