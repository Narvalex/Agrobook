﻿/// <reference path="../../../_all.ts" />

module apArea {
    export class orgTabContratosController {
        static $inject = ['$routeParams', '$scope', '$mdPanel', 'apQueryService', 'apService', 'toasterLite', 'awService']

        constructor(
            private $routeParams: angular.route.IRouteParamsService,
            private $scope: angular.IScope,
            private $mdPanel: angular.material.IPanelService,
            private apQueryService: apQueryService,
            private apService: apService,
            private toasterLite: common.toasterLite,
            private awService: common.filesWidgetService
        ) {
            this.idOrg = this.$routeParams['idOrg'];

            this.recuperarContratos();
        }

        // estados
        formVisible: boolean;
        editMode: boolean;
        submitting = false;
        tieneContrato = false;
        ocultarEliminados = true;

        // object
        idOrg: string;
        dirty: contratoDto;
        tipoContrato: string;
        contratoAdendado: contratoDto;

        // listas
        contratos: contratoDto[];
        soloContratos: contratoDto[];

        //--------------------------
        // Api
        //--------------------------
        mostrarForm(editMode: boolean) {
            this.editMode = editMode;
            this.refrescarEstadoDelForm();
            this.formVisible = true;
            setTimeout(() => document.getElementById('nombreContratoInput').focus(), 0);
        }

        toggleMostrarEliminados() {
            this.ocultarEliminados = !this.ocultarEliminados;
        }

        cancelar() {
            this.formVisible = false;
            this.resetForm();
        }

        mostrarOpciones($event: Event, contrato: contratoDto) {
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
                templateUrl: './dist/ap/views/org/menu-panel-tab-contratos.html',
                position: position,
                trapFocus: true,
                locals: {
                    'contrato': contrato,
                    'parent': this
                },
                panelClass: 'menu-panel-container',
                openFrom: $event,
                focusOnOpen: true,
                zIndex: 150,
                disableParentScroll: true,
                clickOutsideToClose: true,
                escapeToClose: true
            };

            this.$mdPanel.open(config);
        }

        habilitarEdicion(contrato: contratoDto) {
            this.dirty = new contratoDto(
                contrato.id,
                contrato.idOrg,
                contrato.display,
                contrato.esAdenda,
                contrato.eliminado,
                contrato.idContratoDeLaAdenda,
                contrato.fecha);

            this.mostrarForm(true);
        }

        eliminar(contrato: contratoDto) {
            this.apService.eliminarContrato(contrato.id,
                new common.callbackLite(
                    value => {
                        for (var i = 0; i < this.contratos.length; i++) {
                            if (this.contratos[i].id === contrato.id) {
                                this.contratos[i].eliminado = true;
                                break;
                            }
                        }

                        for (var i = 0; i < this.soloContratos.length; i++) {
                            if (this.contratos[i].id === contrato.id) {
                                this.contratos[i].eliminado = true;
                                break;
                            }
                        }

                        if (this.contratoAdendado.id === contrato.id)
                            this.contratoAdendado.eliminado = true;
                    },
                    reason => { })
            );
        }

        restaurar(contrato: contratoDto) {
            this.apService.restaurarParcela(contrato.id,
                new common.callbackLite(
                    value => {
                        for (var i = 0; i < this.contratos.length; i++) {
                            if (this.contratos[i].id === contrato.id) {
                                this.contratos[i].eliminado = false;
                                break;
                            }
                        }

                        for (var i = 0; i < this.soloContratos.length; i++) {
                            if (this.contratos[i].id === contrato.id) {
                                this.contratos[i].eliminado = false;
                                break;
                            }
                        }

                        if (this.contratoAdendado.id === contrato.id)
                            this.contratoAdendado.eliminado = false;
                    },
                    reason => { })
            );
        }

        submit() {
            if (this.dirty.display.length === 0) {
                this.toasterLite.error(this.tipoContrato === 'contrato' ? 'Debe especificar el nombre del contrato' : 'Debe especificar el nombre de la adenda');
                return;
            }
            this.submitting = true;

            // Rellenar datos faltantes
            this.dirty.esAdenda = this.tipoContrato === 'adenda';
            if (this.dirty.esAdenda)
                this.dirty.idContratoDeLaAdenda = this.contratoAdendado.id;

            if (this.editMode) {
                // Edit
                this.apService.editarContrato(this.dirty,
                    new common.callbackLite(
                        value => {
                            for (var i = 0; i < this.contratos.length; i++) {
                                if (this.contratos[i].id === this.dirty.id) {
                                    this.contratos.splice(i, 1);
                                    this.contratos.push(this.dirty);
                                    break;
                                }
                            }

                            for (var i = 0; i < this.soloContratos.length; i++) {
                                if (this.soloContratos[i].id === this.dirty.id) {
                                    this.soloContratos.splice(i, 1);
                                    this.soloContratos.push(this.dirty);
                                    break;
                                }
                            }

                            this.toasterLite.success("Contrato editado");
                            this.resetForm();
                        },
                        reason => {
                            this.submitting = false;
                            this.toasterLite.error('Hubo un error al intentar editar. Verifique por favor.');
                        }
                    ));
            }
            else {
                // New
                this.dirty.idOrg = this.idOrg;
                this.apService.registrarNuevoContrato(this.dirty,
                    new common.callbackLite<contratoDto>(
                        value => {
                            this.contratos.push(value.data);

                            if (this.tipoContrato === 'contrato') {
                                this.soloContratos.push(value.data);
                            }

                            this.toasterLite.success(this.tipoContrato === 'contrato' ? 'Contrato creado' : 'Adenda agregada');
                            this.resetForm();
                        },
                        reason => {
                            this.submitting = false;
                            this.toasterLite.error('Hubo un error al intentar registrar el contrato. Verifique por favor.');
                        })
                );
            }
        }

        //--------------------------
        // Private
        //--------------------------
        private recuperarContratos() {
            this.apQueryService.getContratos(this.idOrg,
                new common.callbackLite<contratoDto[]>(
                    value => {
                        this.contratos = value.data;
                        this.refrescarEstadoDelForm();
                    },
                    reason => { }
                ));
        }

        private refrescarEstadoDelForm() {
            // Preparando
            this.soloContratos = this.contratos.filter(x => !x.esAdenda);
            // Si tiene contrato
            if (this.soloContratos.length > 0) {
                this.tieneContrato = true;
                this.contratoAdendado = this.soloContratos[0];
                this.tipoContrato = 'adenda';
            }
            else {
                this.tipoContrato = 'contrato';
            }
        }

        private resetForm() {
            this.formVisible = false;
            this.dirty = undefined;
            this.submitting = false;
        }

        private formatearFecha(fecha: Date): string {
            return moment(fecha).format('DD/MM/YYYY');
        }

        //-----------------------------
        // Archivos implementation
        //-----------------------------
        awTitle = this.tipoContrato === 'adenda' ? 'Documentos de respaldo de la adenda' : 'Documentos de respaldo del contrato';
        awUploadLink = 'Levantar archivo...';
        awFileUnits: common.fileUnit[] = [];
        awPrepareFiles(element: HTMLInputElement) {
            this.awService.resetFileInput();

            var vm = (angular.element(this)[0] as any) as orgTabContratosController;
            vm.$scope.$apply(scope => {
                vm.awFileUnits = vm.awService.prepareFiles(element.files, vm.awFileUnits);
            });
        }
    }

    class panelMenuController {
        static $inject = ['mdPanelRef'];

        constructor(
            private mdPanelRef: angular.material.IPanelRef
        ) {
        }

        contrato: contratoDto;
        parent: orgTabContratosController;

        editar() {
            this.mdPanelRef.close().then(
                value => {
                    this.parent.habilitarEdicion(this.contrato);
                })
                .finally(() => this.mdPanelRef.destroy());
        }

        eliminar() {
            this.mdPanelRef.close().then(
                value => {
                    this.parent.eliminar(this.contrato);
                })
                .finally(() => this.mdPanelRef.destroy());
        }

        restaurar() {
            this.mdPanelRef.close().then(
                value => {
                    this.parent.restaurar(this.contrato);
                })
                .finally(() => this.mdPanelRef.destroy());
        }
        cancelar() {
            this.mdPanelRef.close().finally(() => this.mdPanelRef.destroy());
        }
    }
}