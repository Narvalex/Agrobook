/// <reference path="../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var toolbarHeaderController = (function () {
        function toolbarHeaderController($mdSidenav, loginService, config) {
            this.$mdSidenav = $mdSidenav;
            this.loginService = loginService;
            this.config = config;
            this.mostrarSidenav = false;
            var claims = this.config.claims;
            var esTecnicoOSuperior = this.loginService.autorizar([claims.roles.Gerente, claims.roles.Tecnico]);
            this.mostrarSidenav = esTecnicoOSuperior;
            if (esTecnicoOSuperior) {
                this.titulo = 'Usuarios';
            }
            else {
                this.titulo = 'Mi Perfil';
            }
        }
        toolbarHeaderController.prototype.goTo = function (location) {
            window.location.href = location;
        };
        toolbarHeaderController.prototype.toggleSideNav = function () {
            this.$mdSidenav('left').toggle();
        };
        return toolbarHeaderController;
    }());
    toolbarHeaderController.$inject = ['$mdSidenav', 'loginService', 'config'];
    usuariosArea.toolbarHeaderController = toolbarHeaderController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=toolbarHeaderController.js.map