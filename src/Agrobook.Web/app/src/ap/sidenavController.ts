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

            this.recuperarListaDeClientes();
        }

        mostrarSidenav: boolean = false;
        loaded: boolean;

        // Objetos
        clientes: cliente[];

        toggleSideNav(): void {
            this.$mdSidenav('left').toggle();
        }

        mostrarFiltros($event) {

        }

        //--------------------
        // PRIVATE
        //--------------------

        private recuperarListaDeClientes() {
            this.clientes = [
                new cliente("coopchorti", "Cooperativa Chortizer", "Loma Plata", "org", "./assets/img/avatar/6.png"),
                new cliente("ccuu", "Cooperativa Colonias Unidas", "Itapua", "org", "./assets/img/avatar/7.png"),
                new cliente("davidelias", "David Elias", "Productor de Chortizer", "prod", "./assets/img/avatar/8.png"),
                new cliente("kazuoyama", "Kazuo Yamazuki", "Productor de Pirapo", "prod", "./assets/img/avatar/9.png")
            ];
            this.loaded = true
        }
    }

    export class cliente {
        constructor(
            public id: string, // coopchorti / davidelias
            public nombre: string, // Cooperativa Chortizer / David Elias
            public desc: string, // Loma Plata / Productor de Chooperativa Chortizer y Colonias Unidas
            public tipo: string, // org / prod
            public avatarUrl: string
        ) {
        }
    }
}