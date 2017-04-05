/// <reference path="../_all.ts" />

module usuariosArea {
    export class sidenavController {
        static $inject = ['$mdSidenav', '$mdDialog', '$mdMedia', 'toasterLite'];

        constructor(
            private $mdSidenav: angular.material.ISidenavService,
            private $mdDialog: angular.material.IDialogService,
            private $mdMedia: angular.material.IMedia,
            private toasterLite: common.toasterLite
        ) {
            this.cargarListaDeUsuarios();
        }

        usuarios: usuarioEnLista[] = [];
        usuarioSeleccionado: usuarioEnLista = null;

        toggleSideNav(): void {
            this.$mdSidenav('left').toggle();
        }

        seleccionarUsuario(usuario: usuarioEnLista) {
            this.usuarioSeleccionado = usuario;
        }

        crearNuevoUsuario($event): void {
            var self = this;

            this.$mdDialog.show({
                templateUrl: '../app/dist/usuarios/dialogs/nuevo-usuario-dialog.html',
                parent: angular.element(document.body),
                targetEvent: $event,
                controller: nuevoUsuarioDialogController,
                controllerAs: 'vm',
                clickOutsideToClose: true,
                fullscreen: (this.$mdMedia('sm') || this.$mdMedia('xs'))
            }).then((usuario: UsuarioDto) => {
               
            }, () => {
                this.toasterLite.info('Creación de nuevo usuario cancelada');
            });
        }

        private cargarListaDeUsuarios() {
            this.usuarios = [
                new usuarioEnLista('Pepito', './assets/img/avatar/1.png'),
                new usuarioEnLista('Fulanito', './assets/img/avatar/2.png'),
                new usuarioEnLista('Menganito', './assets/img/avatar/3.png'),
                new usuarioEnLista('Sultanito', './assets/img/avatar/3.png')
            ];
        }
    }
}