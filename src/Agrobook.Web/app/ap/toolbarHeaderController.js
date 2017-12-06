/// <reference path="../_all.ts" />
var apArea;
(function (apArea) {
    var toolbarHeaderController = /** @class */ (function () {
        function toolbarHeaderController($mdSidenav, loginService, config) {
            this.$mdSidenav = $mdSidenav;
            this.loginService = loginService;
            this.config = config;
            var claims = this.config.claims;
            var esTecnicoOSuperior = this.loginService.autorizar([claims.roles.Gerente, claims.roles.Tecnico]);
            this.mostrarSidenav = esTecnicoOSuperior;
            this.mostrarUsuarios = esTecnicoOSuperior;
        }
        toolbarHeaderController.prototype.goTo = function (location) {
            window.location.href = location;
        };
        toolbarHeaderController.prototype.toggleSideNav = function () {
            this.$mdSidenav('left').toggle();
        };
        toolbarHeaderController.$inject = ['$mdSidenav', 'loginService', 'config'];
        return toolbarHeaderController;
    }());
    apArea.toolbarHeaderController = toolbarHeaderController;
})(apArea || (apArea = {}));
//# sourceMappingURL=toolbarHeaderController.js.map