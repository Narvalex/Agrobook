/// <reference path="../_all.ts" />

module usuariosArea {
    export class sidenavController {
        static $inject = ['$mdSidenav', '$mdDialog', '$mdMedia', 'toasterLite'];

        constructor(
            private $mdSidenav: angular.material.ISidenavService,
            private $mdDialog: angular.material.IDialogService,
            private $mdMedia: angular.material.IMedia,
            private toasterLite: common.toasterLite)
        { }

        toggleSideNav(): void {
            this.$mdSidenav('left').toggle();
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
    }
}