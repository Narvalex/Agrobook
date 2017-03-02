/// <reference path="../_all.ts" />

module UsuariosArea {
    export class sidenavController {
        static $inject = ['$mdSidenav', '$mdDialog', '$mdMedia'];


        constructor(
            private $mdSidenav: angular.material.ISidenavService,
            private $mdDialog: angular.material.IDialogService,
            private $mdMedia: angular.material.IMedia)
        { }

        toggleSideNav(): void {
            this.$mdSidenav('left').toggle();
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
            }).then((usuario: any) => {
                }, () => {
                    console.log('Usted canceló la creación de un nuevo usuario')
                });
        }
    }
}