/// <reference path="../_all.ts" />

module archivosArea {
    export class toolbarHeaderController {
        static $inject = ['$mdSidenav', 'config', 'loginService'];

        constructor(
            private $mdSidenav: angular.material.ISidenavService,
            private config: common.config,
            private loginService: login.loginService
        ) {
            // Auth
            var roles = this.config.claims.roles;
            this.showUsuarios = this.loginService.autorizar([roles.Gerente, roles.Tecnico]);
        }

        //Auth
        showUsuarios: boolean = false;

        goTo(location: string) {
            window.location.href = location;
        }

        toggleSideNav(): void {
            this.$mdSidenav('left').toggle();
        }
    }
}
