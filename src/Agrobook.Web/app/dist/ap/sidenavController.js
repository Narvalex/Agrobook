/// <reference path="../_all.ts" />
var apArea;
(function (apArea) {
    var sidenavController = (function () {
        function sidenavController($mdSidenav, config, loginService) {
            this.$mdSidenav = $mdSidenav;
            this.config = config;
            this.loginService = loginService;
            this.mostrarSidenav = false;
            var claims = this.config.claims;
            this.mostrarSidenav = this.loginService.autorizar([claims.roles.Gerente, claims.roles.Tecnico]);
        }
        sidenavController.prototype.toggleSideNav = function () {
            this.$mdSidenav('left').toggle();
        };
        return sidenavController;
    }());
    sidenavController.$inject = ['$mdSidenav', 'config', 'loginService'];
    apArea.sidenavController = sidenavController;
})(apArea || (apArea = {}));
//# sourceMappingURL=sidenavController.js.map