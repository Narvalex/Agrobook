/// <reference path="../../_all.ts" />

module usuariosArea {

    export class nuevoGrupoDialogController {

        static $inject = ['$mdDialog', 'toasterLite', '$rootScope'];

        constructor(
            private $mdDialog: angular.material.IDialogService,
            private toasterLite: common.toasterLite,
            private $rootScope: ng.IRootScopeService
        ) {
            this.orgSeleccionada = $rootScope.gruposController.orgSeleccionada.display;
            this.setDefaultSubmitText();
        }

        orgSeleccionada: string;
        bloquearSubmit: boolean = false;
        submitLabel: string;

        cancelar(): void {
            this.$mdDialog.cancel();
        }

        private setDefaultSubmitText() {
            this.submitLabel = 'Crear nuevo grupo';
        }
    }
}