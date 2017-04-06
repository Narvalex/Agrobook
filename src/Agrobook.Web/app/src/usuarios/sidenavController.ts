/// <reference path="../_all.ts" />

module usuariosArea {
    export class sidenavController {
        static $inject = ['$mdSidenav', '$mdDialog', '$mdMedia', 'toasterLite', 'usuariosQueryService'];

        constructor(
            private $mdSidenav: angular.material.ISidenavService,
            private $mdDialog: angular.material.IDialogService,
            private $mdMedia: angular.material.IMedia,
            private toasterLite: common.toasterLite,
            private usuariosQueryService: usuariosQueryService
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
                this.usuarios.unshift(
                    new usuarioEnLista(
                        usuario.nombreDeUsuario,
                        usuario.nombreParaMostrar,
                        usuario.avatarUrl));
            }, () => {
                this.toasterLite.info('Creación de nuevo usuario cancelada');
            });
        }

        private cargarListaDeUsuarios() {
            this.usuariosQueryService.obtenerListaDeTodosLosUsuarios(
                value => {
                    this.usuarios = value.data;
                },
                reason => {
                    this.toasterLite.error(JSON.stringify(reason), this.toasterLite.delayForever);
                });
        }
    }
}