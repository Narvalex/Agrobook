/// <reference path="../../../_all.ts" />

module apArea {
    export class orgTabContratosController {
        static $inject = ['$routeParams', '$mdPanel', 'apQueryService', 'apService']

        constructor(
            private $routeParams: angular.route.IRouteParamsService,
            private $mdPanel: angular.material.IPanelService,
            private apQueryService: apQueryService,
            private apService: apService
        ) {
            this.idOrg = this.$routeParams['idOrg'];

            this.recuperarContratos();
        }

        // estados
        formVisible: boolean;
        editMode: boolean;
        submitting = false;
        tieneContrato = false;

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
            this.formVisible = true;
            setTimeout(() => document.getElementById('nombreContratoInput').focus(), 0);
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
            this.dirty = contrato;
            this.mostrarForm(true);
        }

        eliminar(contrato: contratoDto) {
            
        }

        restaurar(contrato: contratoDto) {

        }

        submit() {

        }

        //--------------------------
        // Private
        //--------------------------
        private recuperarContratos() {
            this.apQueryService.getContratos(this.idOrg,
                new common.callbackLite<contratoDto[]>(
                    value => {
                        this.contratos = value.data;
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
                    },
                    reason => { }
                ));
        }

        private resetForm() {
            this.formVisible = false;
            this.dirty = undefined;
            this.submitting = false;
        }

        private formatearFecha(fecha: Date): string {
            return moment(fecha).format('DD/MM/YYYY');
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