/// <reference path="../../_all.ts" />

module UsuariosArea {
    export class nuevoUsuarioDialogController {

        constructor(
            private $mdDialog: angular.material.IDialogService) { }

        avatarUrls = [
            '../app/assets/img/avatar/1.png',
            '../app/assets/img/avatar/2.png',
            '../app/assets/img/avatar/3.png',
            '../app/assets/img/avatar/4.png',
            '../app/assets/img/avatar/5.png',
            '../app/assets/img/avatar/6.png',
            '../app/assets/img/avatar/7.png',
            '../app/assets/img/avatar/8.png',
            '../app/assets/img/avatar/9.png'
        ];

        usuario: Usuario

        cancelar(): void {
            this.$mdDialog.cancel();
        }

        crearNuevoUsuario(): void {
            this.$mdDialog.hide(this.usuario);
        }
    }
}