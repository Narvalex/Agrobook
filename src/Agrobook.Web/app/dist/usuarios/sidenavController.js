/// <reference path="../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var sidenavController = (function () {
        function sidenavController($mdSidenav, $mdDialog, $mdMedia, $mdToast) {
            this.$mdSidenav = $mdSidenav;
            this.$mdDialog = $mdDialog;
            this.$mdMedia = $mdMedia;
            this.$mdToast = $mdToast;
        }
        sidenavController.prototype.toggleSideNav = function () {
            this.$mdSidenav('left').toggle();
        };
        sidenavController.prototype.crearNuevoUsuario = function ($event) {
            var self = this;
            this.$mdDialog.show({
                templateUrl: '../app/dist/usuarios/dialogs/nuevo-usuario-dialog.html',
                parent: angular.element(document.body),
                targetEvent: $event,
                controller: usuariosArea.nuevoUsuarioDialogController,
                controllerAs: 'vm',
                clickOutsideToClose: true,
                fullscreen: (this.$mdMedia('sm') || this.$mdMedia('xs'))
            }).then(function (usuario) {
                var message = 'El usuario de nombre ' + usuario.nombreDeUsuario + ' fue creado exitosamente';
                self.showToast(message);
            }, function () {
                console.log('Usted canceló la creación de un nuevo usuario');
            });
        };
        sidenavController.prototype.showToast = function (message) {
            var toast = this.$mdToast
                .simple()
                .textContent(message)
                .position('top right')
                .hideDelay(3000);
            this.$mdToast.show(toast);
        };
        return sidenavController;
    }());
    sidenavController.$inject = ['$mdSidenav', '$mdDialog', '$mdMedia', '$mdToast'];
    usuariosArea.sidenavController = sidenavController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=sidenavController.js.map