/// <reference path="../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var sidenavController = (function () {
        function sidenavController($mdSidenav, $mdDialog, $mdMedia, toasterLite) {
            this.$mdSidenav = $mdSidenav;
            this.$mdDialog = $mdDialog;
            this.$mdMedia = $mdMedia;
            this.toasterLite = toasterLite;
        }
        sidenavController.prototype.toggleSideNav = function () {
            this.$mdSidenav('left').toggle();
        };
        sidenavController.prototype.crearNuevoUsuario = function ($event) {
            var _this = this;
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
            }, function () {
                _this.toasterLite.info('Creación de nuevo usuario cancelada');
            });
        };
        return sidenavController;
    }());
    sidenavController.$inject = ['$mdSidenav', '$mdDialog', '$mdMedia', 'toasterLite'];
    usuariosArea.sidenavController = sidenavController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=sidenavController.js.map