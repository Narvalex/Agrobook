/// <reference path="../_all.ts" />
var archivosArea;
(function (archivosArea) {
    var toolbarHeaderController = (function () {
        function toolbarHeaderController($mdSidenav, config, loginService) {
            this.$mdSidenav = $mdSidenav;
            this.config = config;
            this.loginService = loginService;
            //Auth
            this.showUsuarios = false;
            // Auth
            var roles = this.config.claims.roles;
            this.showUsuarios = this.loginService.autorizar([roles.Gerente, roles.Tecnico]);
        }
        toolbarHeaderController.prototype.goTo = function (location) {
            window.location.href = location;
        };
        toolbarHeaderController.prototype.toggleSideNav = function () {
            this.$mdSidenav('left').toggle();
        };
        return toolbarHeaderController;
    }());
    toolbarHeaderController.$inject = ['$mdSidenav', 'config', 'loginService'];
    archivosArea.toolbarHeaderController = toolbarHeaderController;
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=toolbarHeaderController.js.map