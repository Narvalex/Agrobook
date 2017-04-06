/// <reference path="../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var sidenavController = (function () {
        function sidenavController($mdSidenav, $mdDialog, $mdMedia, toasterLite, usuariosQueryService) {
            this.$mdSidenav = $mdSidenav;
            this.$mdDialog = $mdDialog;
            this.$mdMedia = $mdMedia;
            this.toasterLite = toasterLite;
            this.usuariosQueryService = usuariosQueryService;
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
                _this.usuarios.unshift(new usuariosArea.usuarioEnLista(usuario.nombreDeUsuario, usuario.nombreParaMostrar, usuario.avatarUrl));
            }, function () {
                _this.toasterLite.info('Creación de nuevo usuario cancelada');
            });
        };
        sidenavController.prototype.cargarListaDeUsuarios = function () {
            var _this = this;
            this.usuariosQueryService.obtenerListaDeTodosLosUsuarios(function (value) {
                _this.usuarios = value.data;
            }, function (reason) {
                _this.toasterLite.error(JSON.stringify(reason), _this.toasterLite.delayForever);
            });
        };
        return sidenavController;
    }());
    sidenavController.$inject = ['$mdSidenav', '$mdDialog', '$mdMedia', 'toasterLite', 'usuariosQueryService'];
    usuariosArea.sidenavController = sidenavController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=sidenavController.js.map