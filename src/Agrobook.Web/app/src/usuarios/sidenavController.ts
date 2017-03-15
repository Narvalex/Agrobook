/// <reference path="../_all.ts" />

module usuariosArea {
    export class sidenavController {
        static $inject = ['$mdSidenav', '$mdDialog', '$mdMedia', '$mdToast'];

        constructor(
            private $mdSidenav: angular.material.ISidenavService,
            private $mdDialog: angular.material.IDialogService,
            private $mdMedia: angular.material.IMedia,
            private $mdToast: angular.material.IToastService)
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
                var message = 'El usuario de nombre ' + usuario.nombreDeUsuario + ' fue creado exitosamente';
                self.showToast(message);
                
            }, () => {
                console.log('Usted canceló la creación de un nuevo usuario')
            });
        }

        private showToast(message: string): void {
            var toast = this.$mdToast
                .simple()
                .textContent(message)
                .position('top right')
                .hideDelay(3000);

            this.$mdToast.show(toast);
        }
    }
}