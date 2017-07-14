/// <reference path="../_all.ts" />

module usuariosArea {
    export class toolbarHeaderController {
        static $inject = ['$mdSidenav', 'loginService', 'config'];

        constructor(
            private $mdSidenav: angular.material.ISidenavService,
            private loginService: login.loginService,
            private config: common.config
        ) {
            var claims = this.config.claims;
            var esTecnicoOSuperior = this.loginService.autorizar([claims.roles.Gerente, claims.roles.Tecnico]); 
            this.mostrarSidenav = esTecnicoOSuperior;

            if (esTecnicoOSuperior) {
                this.titulo = 'Usuarios';
            }
            else {
                this.titulo = 'Mi Perfil'
            }
        }

        mostrarSidenav: boolean = false;
        titulo: string;

        goTo(location: string) {
            window.location.href = location;
        }

        toggleSideNav(): void {
            this.$mdSidenav('left').toggle();
        }
    }
}