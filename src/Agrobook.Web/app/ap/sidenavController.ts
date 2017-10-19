/// <reference path="../_all.ts" />

module apArea {
    export class sidenavController {
        static $inject = ['$mdSidenav', 'config', 'loginService', 'apQueryService'];

        constructor(
            private $mdSidenav: angular.material.ISidenavService,
            private config: common.config,
            private loginService: login.loginService,
            private apQueryService: apQueryService
        ) {
            var claims = this.config.claims;
            this.mostrarSidenav = this.loginService.autorizar([claims.roles.Gerente, claims.roles.Tecnico]);
            if (!this.mostrarSidenav) return;

            this.resolverFiltros();
            this.establecerFiltro('todos');
            this.recuperarListaDeClientes();
        }

        // Estados
        mostrarSidenav: boolean = false;
        loaded: boolean;

        // Seleccioandos
        filtroSeleccionado: filtro;
        clienteSeleccionado: cliente;

        // Listas
        filtros: filtro[];
        clientes: cliente[];

        toggleSideNav(): void {
            this.$mdSidenav('left').toggle();
        }

        cambiarFiltro($event) {
            switch (this.filtroSeleccionado.id) {
                case "todos":
                    this.establecerFiltro('prod');
                    break;
                case "prod":
                    this.establecerFiltro('org');
                    break;
                case "org":
                    this.establecerFiltro('todos');
                    break;
            }
        }

        seleccionarCliente(cliente: cliente) {
            this.clienteSeleccionado = cliente;
            this.toggleSideNav();
            window.location.replace(`#!/${cliente.tipo}/${cliente.id}`);
        }

        //--------------------
        // PRIVATE
        //--------------------

        private resolverFiltros() {
            this.filtros = [
                new filtro("todos", "Buscar entre todos", "filter_list"),
                new filtro("prod", "Buscar en productores", "people"),
                new filtro("org", "Buscar en organizaciones", "business")
            ];
        }

        private establecerFiltro(filtroId: string) {
            for (var i = 0; i < this.filtros.length; i++) {
                if (this.filtros[i].id === filtroId) {
                    this.filtroSeleccionado = this.filtros[i];
                    this.recuperarListaDeClientes();
                    return;
                }
            };
            throw "El filtro: " + filtroId + "es inválido";
        }

        private recuperarListaDeClientes() {
            this.apQueryService.getClientes(
                this.filtroSeleccionado.id,
                new common.callbackLite<cliente[]>(
                    value => {
                        this.clientes = value.data;
                        this.loaded = true;
                    },
                    reason => { })
            );
        }
    }

    export class filtro {

        constructor(
            public id: string,
            public placeholder: string,
            public icon: string
        ) {
        }
    }
}