/// <reference path="../../_all.ts" />

module usuariosArea {

    export class nuevoGrupoDialogController {

        static $inject = ['$mdDialog', 'toasterLite', '$rootScope', 'usuariosService'];

        constructor(
            private $mdDialog: angular.material.IDialogService,
            private toasterLite: common.toasterLite,
            private $rootScope: ng.IRootScopeService,
            private usuariosService: usuariosService
        ) {
            this.orgSeleccionada = $rootScope.gruposController.orgSeleccionada;
            this.setDefaultSubmitText();
        }

        orgSeleccionada: organizacionDto;
        bloquearSubmit: boolean = false;
        submitLabel: string;

        //model
        nuevoGrupo: string;

        crearNuevoGrupo() {
            this.usuariosService.crearNuevoGrupo(this.orgSeleccionada.id, this.nuevoGrupo,
                response => {
                    this.toasterLite.success(`Nuevo grupo ${this.nuevoGrupo} creado para ${this.orgSeleccionada.display}.`);
                    this.$mdDialog.hide(response.data);
                },
                reason => { this.toasterLite.error('Ocurrio un error!'); });
        }

        cancelar(): void {
            this.$mdDialog.cancel();
        }

        // Internal
        //********************

        private setDefaultSubmitText() {
            this.submitLabel = 'Crear nuevo grupo';
        }
    }
}