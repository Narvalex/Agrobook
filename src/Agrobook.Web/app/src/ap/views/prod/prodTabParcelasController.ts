/// <reference path="../../../_all.ts" />

module apArea {
    export class prodTabParcelasController {
        static $inject = ['config', 'apService', 'apQueryService', 'toasterLite', '$routeParams'];

        constructor(
            private config: common.config,
            private apService: apService,
            private apQueryService: apQueryService,
            private toasterLite: common.toasterLite,
            private $routeParams: angular.route.IRouteParamsService
        ) {
            this.mostrarForm = false;

            this.idProd = this.$routeParams['idProd'];

            this.obtenerParcelasDelProd();
        }

        // Estados
        mostrarForm: boolean;
        submitting: boolean;
        formIsEditing: boolean; // editMode Actually

        // Objetos
        idProd: string;
        parcelaObject: edicionParcelaDto;

        // Listas
        parcelas: parcelaDto[];

        // Api
        habilitarCreacionDeNuevaParcela() {
            this.formIsEditing = false;
            this.mostrarFormYHacerFocus();
        }

        habilitarEdicionDeParcela(parcela: parcelaDto) {
            this.formIsEditing = true;

            this.parcelaObject = new edicionParcelaDto();
            this.parcelaObject.display = parcela.display;
            this.parcelaObject.hectareas = parcela.hectareas;
            this.parcelaObject.idProd = parcela.idProd;
            this.parcelaObject.idParcela = parcela.id;

            this.mostrarFormYHacerFocus();
        }

        checkIfEnter($event) {
            var keyCode = $event.keyCode;
            if (keyCode === this.config.keyCodes.enter)
                this.registrarNuevaParcela();
            else if (keyCode === this.config.keyCodes.esc) {
                this.cancel();
            }
        }

        submit() {
            if (this.parcelaObject.display.length === 0) {
                this.toasterLite.error("Debe especificar el nombre de la parcela");
                return;
            }
            this.submitting = true;

            if (this.formIsEditing) 
                this.editarParcela();
            else 
                this.registrarNuevaParcela();
        }

        cancel() {
            this.mostrarForm = false;
            this.resetForm();
        }

        // Privados
        registrarNuevaParcela() {
            this.apService.registrarNuevaParcela(this.parcelaObject,
                new common.callbackLite<parcelaDto>(
                    value => {
                        this.resetForm();
                        this.parcelas.push(value.data);
                        this.toasterLite.success('Parcela creada')
                    },
                    reason => {
                        this.resetForm();
                    })
            );
        }

        editarParcela() {
            this.apService.editarParcela(this.parcelaObject,
                new common.callbackLite<{}>(
                    value => {
                        // eventual consistency handling before reseting form
                        for (var i = 0; i < this.parcelas.length; i++) {
                            if (this.parcelas[i].id === this.parcelaObject.idParcela) {
                                this.parcelas[i].hectareas = this.parcelaObject.hectareas;
                                this.parcelas[i].display = this.parcelaObject.display;
                                break;
                            }
                        }
                        this.toasterLite.success('Parcela editada')

                        this.resetForm();
                    },
                    reason => {
                        this.resetForm();
                    })
            );
        }

        private mostrarFormYHacerFocus() {
            this.mostrarForm = true;
            setTimeout(() =>
                document.getElementById('parcelaInput').focus(), 0);
        }

        private resetForm() {
            this.parcelaObject = undefined;
            this.submitting = false;
            this.mostrarForm = false;
        }

        private obtenerParcelasDelProd() {
            this.apQueryService.gerParcelasDelProd(this.idProd,
                new common.callbackLite<parcelaDto[]>(
                    response => {
                        this.parcelas = response.data;
                    },
                    reason => { })
            );
        }
    }
}