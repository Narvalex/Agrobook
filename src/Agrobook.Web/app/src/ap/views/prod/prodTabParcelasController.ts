/// <reference path="../../../_all.ts" />

module apArea {
    export class prodTabParcelasController {
        static $inject = ['config', 'apService', 'apQueryService', 'toasterLite', '$routeParams', '$mdPanel'];

        constructor(
            private config: common.config,
            private apService: apService,
            private apQueryService: apQueryService,
            private toasterLite: common.toasterLite,
            private $routeParams: angular.route.IRouteParamsService,
            private $mdPanel: angular.material.IPanelService
        ) {
            this.mostrarForm = false;

            this.idProd = this.$routeParams['idProd'];

            this.obtenerParcelasDelProd();
        }

        // Estados
        ocultarEliminados = true;
        mostrarForm: boolean;
        submitting: boolean;
        formIsEditing: boolean; // editMode Actually

        // Objetos
        idProd: string;
        parcelaObject: edicionParcelaDto;

        // Listas
        parcelas: parcelaDto[];

        // Api
        toggleMostrarEliminados() {
            this.ocultarEliminados = !this.ocultarEliminados;
        }

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

        mostrarOpciones($event: Event, parcela: parcelaDto) {
            let position = this.$mdPanel.newPanelPosition()
                .relativeTo($event.srcElement)
                .addPanelPosition(
                this.$mdPanel.xPosition.ALIGN_START,
                this.$mdPanel.yPosition.BELOW);

            let config: angular.material.IPanelConfig = {
                attachTo: angular.element(document.body),
                controller: panelMenuController,
                controllerAs: 'vm',
                hasBackdrop: true,
                templateUrl: './dist/ap/views/prod/menu-panel-tab-parcelas.html',
                position: position,
                trapFocus: true,
                locals: {
                    'parcela': parcela,
                    'parent': this
                },
                panelClass: 'menu-panel-container',
                openFrom: $event,
                focusOnOpen: true,
                zIndex: 150,
                disableParentScroll: true,
                clickOutsideToClose: true,
                escapeToClose: true,
            };

            this.$mdPanel.open(config);
        }

        checkIfEnter($event) {
            let keyCode = $event.keyCode;
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

        eliminar(parcela: parcelaDto) {
            this.apService.eliminar(parcela.id,
                new common.callbackLite<{}>(
                    value => {
                        for (var i = 0; i < this.parcelas.length; i++) {
                            if (this.parcelas[i].id === parcela.id) {
                                this.parcelas[i].eliminado = true;
                                break;
                            }
                        }

                        this.toasterLite.info('Parcela elimnada');
                    },
                    reason => { })
            );
        }

        restaurar(parcela: parcelaDto) {
            this.apService.restaurar(parcela.id,
                new common.callbackLite<{}>(
                    value => {
                        for (var i = 0; i < this.parcelas.length; i++) {
                            if (this.parcelas[i].id === parcela.id) {
                                this.parcelas[i].eliminado = false;
                                break;
                            }
                        }

                        this.toasterLite.success('Parcela restaurada');
                    },
                    reason => { })
            );
        }

        // Privados
        registrarNuevaParcela() {
            this.parcelaObject.idProd = this.idProd;
            this.apService.registrarNuevaParcela(this.parcelaObject,
                new common.callbackLite<parcelaDto>(
                    value => {
                        this.resetForm();
                        this.parcelas.push(value.data);
                        this.toasterLite.success('Parcela creada')
                    },
                    reason => {
                        this.toasterLite.error('Hubo un error al registrar la parcela. Verifique que el nombre ya no exista por favor');
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
            this.apQueryService.getParcelasDelProd(this.idProd,
                new common.callbackLite<parcelaDto[]>(
                    response => {
                        this.parcelas = response.data;
                    },
                    reason => { })
            );
        }
    }

    class panelMenuController {
        static $inject = ['mdPanelRef'];

        constructor(
            private mdPanelref: angular.material.IPanelRef
        ) {
        }

        parcela: parcelaDto;
        parent: prodTabParcelasController;

        editar() {
            this.mdPanelref.close().then(
                value => {
                    this.parent.habilitarEdicionDeParcela(this.parcela);
                })
                .finally(() => this.mdPanelref.destroy());
        }

        eliminar() {
            this.mdPanelref.close().then(
                value => {
                    this.parent.eliminar(this.parcela);
                })
                .finally(() => this.mdPanelref.destroy());
        }

        restaurar() {
            this.mdPanelref.close().then(
                value => {
                    this.parent.restaurar(this.parcela);
                })
                .finally(() => this.mdPanelref.destroy());
        }

        cancelar() {
            this.mdPanelref.close().finally(() => this.mdPanelref.destroy());
        }
    }
}