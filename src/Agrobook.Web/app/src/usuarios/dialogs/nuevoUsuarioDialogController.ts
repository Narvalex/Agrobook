/// <reference path="../../_all.ts" />

module UsuariosArea {
    export class nuevoUsuarioDialogController {

        constructor(
            private $mdDialog: angular.material.IDialogService) { }

        cancelar(): void {
            this.$mdDialog.cancel();
        }
    }
}