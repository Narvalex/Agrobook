/// <reference path="../../_all.ts" />

module usuariosArea {
    export class nuevoUsuarioDialogController {

        static $inject = ['$mdDialog', 'usuariosService', 'toasterLite'];

        constructor(
            private $mdDialog: angular.material.IDialogService,
            private usuariosService: usuariosArea.usuariosService,
            private toasterLite: common.toasterLite
        ) {
            this.setDefaultSubmitText();
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

        bloquearSubmit: boolean = false;
        submitLabel: string;

        cancelar(): void {
            this.$mdDialog.cancel();
        }

        crearNuevoUsuario(): void {
            var nombre = this.usuario.nombreDeUsuario;
            this.setWorkingText();
            this.bloquearSubmit = true;
            this.usuariosService.crearNuevoUsuario(this.usuario,
                (value) => {
                    this.toasterLite.success('El usuario ' + nombre + ' fue creado exitosamente');
                    this.$mdDialog.hide(this.usuario);
                },
                (reason) => {
                    this.setDefaultSubmitText();
                    this.bloquearSubmit = false;
                    window.alert('Ocurrió un error y no se pudo crear el usuario');
                });
        }

        private setDefaultSubmitText() {
            this.submitLabel = 'Crear nuevo usuario';
        }

        private setWorkingText() {
            this.submitLabel = 'Creando nuevo usuario...'
        }
    }
}