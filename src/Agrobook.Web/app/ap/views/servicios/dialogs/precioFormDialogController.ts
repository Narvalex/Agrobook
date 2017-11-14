/// <reference path="../../../../_all.ts" />

module apArea {
    export class precioFormDialogController {
        static $inject = ['$mdDialog', '$rootScope', '$scope', 'numberFormatter', 'apService', '$timeout', 'config', 'toasterLite'];

        private apScope: IApScope;

        constructor(
            private $mdDialog: angular.material.IDialogService,
            private $rootScope: angular.IRootScopeService,
            private $scope: angular.IScope,
            private nf: common.numberFormatter,
            private apService: apService,
            private $timeout: angular.ITimeoutService,
            private config: common.config,
            private toasterLite: common.toasterLite
        ) {
            this.apScope = this.$rootScope as IApScope;
            this.servicio = this.apScope.servicioActual;

            this.inicializar();
        }

        // states
        ajustarDesdeElTotal: boolean;
        working: boolean = false;

        // dtos
        servicio: servicioDto;
        hectareas: number;

        // Two-way
        precioInput: string; 
        precioLabel: string;
        precioTotal: number; // este es el que se utiliza para pesistir.

        //----------------------------
        // Public API
        //----------------------------

        cancelar() {
            this.$mdDialog.cancel();
        }

        checkIfEnter($event: KeyboardEvent) {
            if ($event.keyCode === this.config.keyCodes.enter)
                this.fijarOAjustarPrecio();
        }

        fijarOAjustarPrecio() {
            this.working = true;
            if (this.servicio.tienePrecio) {
                this.apService.ajustarPrecio(this.servicio.id, this.precioTotal,
                    new common.callbackLite<any>(
                        value => {
                            this.$mdDialog.hide(this.precioTotal);
                            this.working = false;
                        },
                        error => {
                            this.toasterLite.error('Hubo un error al intentar ajustar el precio');
                            this.working = false;
                        }));
            }
            else {
                this.apService.fijarPrecio(this.servicio.id, this.precioTotal,
                    new common.callbackLite<any>(
                        value => {
                            this.$mdDialog.hide(this.precioTotal);
                            this.working = false;
                        },
                        error => {
                            this.toasterLite.error('Hubo un error al intentar fijar el precio');
                            this.working = false;
                        }));
            }
        }

        //---------------------------
        // Private
        //---------------------------

        private inicializar() {
            this.$timeout(() => {
                this.$scope.$apply(() => {
                    let e = document.getElementById('precioInput');
                    e.focus();
                });
            }, 750);

            this.ajustarDesdeElTotal = true;

            this.hectareas = this.nf.parseCommaAsDecimalSeparatorToUSNumber(this.servicio.hectareas);

            var self = this;
            this.$scope.$watch(angular.bind(this.$scope, () => this.precioInput),
                (newValue: string, oldValue: string) => {
                    self.calcularYMostrarPrecio();
                });
            this.$scope.$watch(angular.bind(this.$scope, () => this.ajustarDesdeElTotal),
                (newValue: boolean, oldValue: boolean) => {
                    self.calcularYMostrarPrecio();
                });

            if (this.servicio.tienePrecio) {
                this.precioInput = this.servicio.precioTotal;
            }
            else {
                this.precioInput = undefined;
                this.precioLabel = '0';
            }
        }

        private calcularYMostrarPrecio() {
            // Mostrando el input formateado
            let precioInput: number;
            if (this.precioInput === undefined)
                precioInput = 0;
            else {
                precioInput = this.nf.parseCommaAsDecimalSeparatorToUSNumber(this.precioInput);

                // Hay que ver la necesidad, si formatear o no
                let lastChar = this.precioInput[this.precioInput.length - 1];
                if (lastChar !== ',')
                    this.precioInput = this.nf.formatFromUSNumber(precioInput);
            }

            // Mostrando el label calculado
            let precioLabel: number;
            if (this.ajustarDesdeElTotal) {
                precioLabel = precioInput / this.hectareas;
                this.precioTotal = precioInput;
            }
            else {
                precioLabel = precioInput * this.hectareas;
                this.precioTotal = precioLabel;
            }

            this.precioLabel = this.nf.formatFromUSNumber(precioLabel);
        }
    }
}