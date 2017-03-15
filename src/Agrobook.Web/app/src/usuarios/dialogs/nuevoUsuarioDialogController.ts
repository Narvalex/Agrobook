/// <reference path="../../_all.ts" />

module usuariosArea {
    export class nuevoUsuarioDialogController {

        static $inject = ['$mdDialog', 'usuariosService'];

        constructor(
            private $mdDialog: angular.material.IDialogService,
            private usuariosService: usuariosArea.usuariosService
        ) {
        }

        avatarUrls = [
            './assets/img/avatar/1.png',
            './assets/img/avatar/2.png',
            './assets/img/avatar/3.png',
            './assets/img/avatar/4.png',
            './assets/img/avatar/5.png',
            './assets/img/avatar/6.png',
            './assets/img/avatar/7.png',
            './assets/img/avatar/8.png',
            './assets/img/avatar/9.png'
        ];

        usuario: UsuarioDto

        cancelar(): void {
            this.$mdDialog.cancel();
        }

        crearNuevoUsuario(): void {
            this.usuariosService.crearNuevoUsuario(this.usuario,
                (value) => {
                },
                (reason) => {
                });

            this.$mdDialog.hide(this.usuario);
        }
    }
}