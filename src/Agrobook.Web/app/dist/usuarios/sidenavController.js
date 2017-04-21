/// <reference path="../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var sidenavController = (function () {
        function sidenavController($mdSidenav, $mdDialog, $mdMedia, toasterLite, usuariosQueryService, $rootScope, config) {
            this.$mdSidenav = $mdSidenav;
            this.$mdDialog = $mdDialog;
            this.$mdMedia = $mdMedia;
            this.toasterLite = toasterLite;
            this.usuariosQueryService = usuariosQueryService;
            this.$rootScope = $rootScope;
            this.config = config;
            this.usuarios = [];
            this.usuarioSeleccionado = null;
            this.loaded = false;
            this.cargarListaDeUsuarios();
        }
        sidenavController.prototype.toggleSideNav = function () {
            this.$mdSidenav('left').toggle();
        };
        sidenavController.prototype.seleccionarUsuario = function (usuario) {
            this.usuarioSeleccionado = usuario;
            this.$rootScope.$broadcast(this.config.eventIndex.usuarios.usuarioSeleccionado, {});
            window.location.href = '#!/usuario/' + usuario.nombre;
            this.toggleSideNav();
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
                _this.usuarios.unshift(new usuariosArea.usuarioInfoBasica(usuario.nombreDeUsuario, usuario.nombreParaMostrar, usuario.avatarUrl));
            }, function () {
                _this.toasterLite.info('Creación de nuevo usuario cancelada');
            });
        };
        sidenavController.prototype.cargarListaDeUsuarios = function () {
            var _this = this;
            this.usuariosQueryService.obtenerListaDeTodosLosUsuarios(function (value) {
                _this.loaded = true;
                _this.usuarios = value.data;
            }, function (reason) {
                _this.toasterLite.error('Ocurrió un error al recuperar lista de usuarios', _this.toasterLite.delayForever);
            });
        };
        return sidenavController;
    }());
    sidenavController.$inject = ['$mdSidenav', '$mdDialog', '$mdMedia', 'toasterLite', 'usuariosQueryService', '$rootScope',
        'config'];
    usuariosArea.sidenavController = sidenavController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=sidenavController.js.map