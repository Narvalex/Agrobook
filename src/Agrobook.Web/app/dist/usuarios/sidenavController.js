/// <reference path="../_all.ts" />
var UsuariosArea;
(function (UsuariosArea) {
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
            var _this = this;
            this.$mdDialog.show({
                templateUrl: '../app/dist/usuarios/dialogs/nuevo-usuario-dialog.html',
                parent: angular.element(document.body),
                targetEvent: $event,
                controller: UsuariosArea.nuevoUsuarioDialogController,
                controllerAs: 'vm',
                clickOutsideToClose: true,
                fullscreen: (this.$mdMedia('sm') || this.$mdMedia('xs'))
            }).then(function (usuario) {
                var message = 'El usuario de nombre ' + usuario.nombreDeUsuario + ' fue creado exitosamente';
                var toast = _this.$mdToast
                    .simple()
                    .textContent(message)
                    .position('top right')
                    .hideDelay(3000);
                _this.$mdToast.show(toast);
            }, function () {
                console.log('Usted canceló la creación de un nuevo usuario');
            });
        };
        return sidenavController;
    }());
    sidenavController.$inject = ['$mdSidenav', '$mdDialog', '$mdMedia', '$mdToast'];
    UsuariosArea.sidenavController = sidenavController;
})(UsuariosArea || (UsuariosArea = {}));
//# sourceMappingURL=sidenavController.js.map