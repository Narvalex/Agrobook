/// <reference path="../_all.ts" />

module apArea {
    export class sidenavController {
        static $inject = ['$mdSidenav', 'config', 'loginService'];

        constructor(
            private $mdSidenav: angular.material.ISidenavService,
            private config: common.config,
            private loginService: login.loginService
        ) {
            var claims = this.config.claims;
            this.mostrarSidenav = this.loginService.autorizar([claims.roles.Gerente, claims.roles.Tecnico]);
        }

        mostrarSidenav: boolean = false;

        toggleSideNav(): void {
            this.$mdSidenav('left').toggle();
        }
    }
}