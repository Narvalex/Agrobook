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
            this.recuperarListaDeClientes();
        }
        sidenavController.prototype.toggleSideNav = function () {
            this.$mdSidenav('left').toggle();
        };
        sidenavController.prototype.mostrarFiltros = function ($event) {
        };
        //--------------------
        // PRIVATE
        //--------------------
        sidenavController.prototype.recuperarListaDeClientes = function () {
            this.clientes = [
                new cliente("coopchorti", "Cooperativa Chortizer", "Loma Plata", "org", "./assets/img/avatar/6.png"),
                new cliente("ccuu", "Cooperativa Colonias Unidas", "Itapua", "org", "./assets/img/avatar/7.png"),
                new cliente("davidelias", "David Elias", "Productor de Chortizer", "prod", "./assets/img/avatar/8.png"),
                new cliente("kazuoyama", "Kazuo Yamazuki", "Productor de Pirapo", "prod", "./assets/img/avatar/9.png")
            ];
            this.loaded = true;
        };
        return sidenavController;
    }());
    sidenavController.$inject = ['$mdSidenav', 'config', 'loginService'];
    apArea.sidenavController = sidenavController;
    var cliente = (function () {
        function cliente(id, // coopchorti / davidelias
            nombre, // Cooperativa Chortizer / David Elias
            desc, // Loma Plata / Productor de Chooperativa Chortizer y Colonias Unidas
            tipo, // org / prod
            avatarUrl) {
            this.id = id;
            this.nombre = nombre;
            this.desc = desc;
            this.tipo = tipo;
            this.avatarUrl = avatarUrl;
        }
        return cliente;
    }());
    apArea.cliente = cliente;
})(apArea || (apArea = {}));
//# sourceMappingURL=sidenavController.js.map