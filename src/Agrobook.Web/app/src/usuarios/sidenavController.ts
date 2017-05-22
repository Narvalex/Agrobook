/// <reference path="../_all.ts" />

module usuariosArea {
    export class sidenavController {
        static $inject = ['$mdSidenav', '$mdDialog', '$mdMedia', 'toasterLite', 'usuariosQueryService', '$rootScope',
        'config', '$scope'];

        constructor(
            private $mdSidenav: angular.material.ISidenavService,
            private $mdDialog: angular.material.IDialogService,
            private $mdMedia: angular.material.IMedia,
            private toasterLite: common.toasterLite,
            private usuariosQueryService: usuariosQueryService,
            private $rootScope: ng.IRootScopeService,
            private config: common.config,
            private $scope: ng.IScope
        ) {
            this.cargarListaDeUsuarios();

            this.$scope.$on(this.config.eventIndex.usuarios.perfilActualizado,
                (e, args: common.perfilActualizado) => {
                    for (var i = 0; i < this.usuarios.length; i++) {
                        if (this.usuarios[i].nombre == args.usuario) {
                            this.usuarios[i].avatarUrl = args.avatarUrl;
                            this.usuarios[i].nombreParaMostrar = args.nombreParaMostrar;
                            break;
                        }
                    }
                });
        }

        usuarios: usuarioInfoBasica[] = [];
        usuarioSeleccionado: usuarioInfoBasica = null;
        loaded: boolean = false;

        toggleSideNav(): void {
            this.$mdSidenav('left').toggle();
        }

        seleccionarUsuario(usuario: usuarioInfoBasica) {
            this.usuarioSeleccionado = usuario;
            this.$rootScope.$broadcast(this.config.eventIndex.usuarios.usuarioSeleccionado, {});
            window.location.replace('#!/usuario/' + usuario.nombre + '?tab=perfil');
            this.toggleSideNav();
        }

        crearNuevoUsuario($event): void {

            this.$mdDialog.show({
                templateUrl: '../app/dist/usuarios/dialogs/nuevo-usuario-dialog.html',
                parent: angular.element(document.body),
                targetEvent: $event,
                controller: nuevoUsuarioDialogController,
                controllerAs: 'vm',
                clickOutsideToClose: true,
                fullscreen: (this.$mdMedia('sm') || this.$mdMedia('xs'))
            }).then((usuario: usuarioDto) => {
                this.usuarios.unshift(
                    new usuarioInfoBasica(
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
                    this.loaded = true;
                    this.usuarios = value.data;
                },
                reason => {
                    this.toasterLite.error('Ocurrió un error al recuperar lista de usuarios', this.toasterLite.delayForever);
                });
        }
    }
}