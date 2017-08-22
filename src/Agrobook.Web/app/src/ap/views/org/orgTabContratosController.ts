﻿/// <reference path="../../../_all.ts" />

module apArea {
    export class orgTabContratosController {
        static $inject = ['$routeParams', '$mdPanel', 'apQueryService']

        constructor(
            private $routeParams: angular.route.IRouteParamsService,
            private $mdPanel: angular.material.IPanelService,
            private apQueryService: apQueryService
        ) {
            this.idOrg = this.$routeParams['idOrg'];

            this.recuperarContratos();
        }

        // estados
        formVisible: boolean;
        editMode: boolean;
        submitting = false;

        // object
        idOrg: string; 
        dirty: editContratoDto;

        // listas
        contratos: contratoDto[];

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

        mostrarOpciones($events: Event, contrato: contratoDto) {
            //let position = this.$mdPanel.newPanelPosition()
            //.relativeTo
        }

        //--------------------------
        // Private
        //--------------------------
        private recuperarContratos() {
            this.apQueryService.getContratos(this.idOrg,
                new common.callbackLite<contratoDto[]>(
                    value => {
                        this.contratos = value.data;
                    },
                    reason => { }
                ));
        }

        private resetForm() {
            this.formVisible = false;
            this.dirty = undefined;
            this.submitting = false;
        }
    }
}