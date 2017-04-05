/// <reference path="../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var sidenavController = (function () {
        function sidenavController($mdSidenav, $mdDialog, $mdMedia, toasterLite) {
            this.$mdSidenav = $mdSidenav;
            this.$mdDialog = $mdDialog;
            this.$mdMedia = $mdMedia;
            this.toasterLite = toasterLite;
            this.usuarios = [];
            this.usuarioSeleccionado = null;
            this.cargarListaDeUsuarios();
        }
        sidenavController.prototype.toggleSideNav = function () {
            this.$mdSidenav('left').toggle();
        };
        sidenavController.prototype.seleccionarUsuario = function (usuario) {
            this.usuarioSeleccionado = usuario;
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
        sidenavController.prototype.cargarListaDeUsuarios = function () {
            this.usuarios = [
                new usuariosArea.usuarioEnLista('Pepito', './assets/img/avatar/1.png'),
                new usuariosArea.usuarioEnLista('Fulanito', './assets/img/avatar/2.png'),
                new usuariosArea.usuarioEnLista('Menganito', './assets/img/avatar/3.png'),
                new usuariosArea.usuarioEnLista('Sultanito', './assets/img/avatar/3.png')
            ];
        };
        return sidenavController;
    }());
    sidenavController.$inject = ['$mdSidenav', '$mdDialog', '$mdMedia', 'toasterLite'];
    usuariosArea.sidenavController = sidenavController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=sidenavController.js.map