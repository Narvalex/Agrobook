/// <reference path="../_all.ts" />
var apArea;
(function (apArea) {
    var sidenavController = (function () {
        function sidenavController($mdSidenav, config, loginService, apQueryService) {
            this.$mdSidenav = $mdSidenav;
            this.config = config;
            this.loginService = loginService;
            this.apQueryService = apQueryService;
            // Estados
            this.mostrarSidenav = false;
            var claims = this.config.claims;
            this.mostrarSidenav = this.loginService.autorizar([claims.roles.Gerente, claims.roles.Tecnico]);
            this.resolverFiltros();
            this.establecerFiltro('todos');
            this.recuperarListaDeClientes();
        }
        sidenavController.prototype.toggleSideNav = function () {
            this.$mdSidenav('left').toggle();
        };
        sidenavController.prototype.cambiarFiltro = function ($event) {
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
        };
        //--------------------
        // PRIVATE
        //--------------------
        sidenavController.prototype.resolverFiltros = function () {
            this.filtros = [
                new filtro("todos", "Buscar entre todos", "search"),
                new filtro("prod", "Buscar en productores", "people"),
                new filtro("org", "Buscar en organizaciones", "business")
            ];
        };
        sidenavController.prototype.establecerFiltro = function (filtroId) {
            for (var i = 0; i < this.filtros.length; i++) {
                if (this.filtros[i].id === filtroId) {
                    this.filtroSeleccionado = this.filtros[i];
                    this.recuperarListaDeClientes();
                    return;
                }
            }
            ;
            throw "El filtro: " + filtroId + "es inválido";
        };
        sidenavController.prototype.recuperarListaDeClientes = function () {
            var _this = this;
            this.apQueryService.getClientes(this.filtroSeleccionado.id, new common.callbackLite(function (value) {
                _this.clientes = value.data;
                _this.loaded = true;
            }, function (reason) { }));
        };
        return sidenavController;
    }());
    sidenavController.$inject = ['$mdSidenav', 'config', 'loginService', 'apQueryService'];
    apArea.sidenavController = sidenavController;
    var filtro = (function () {
        function filtro(id, placeholder, icon) {
            this.id = id;
            this.placeholder = placeholder;
            this.icon = icon;
        }
        return filtro;
    }());
    apArea.filtro = filtro;
})(apArea || (apArea = {}));
//# sourceMappingURL=sidenavController.js.map