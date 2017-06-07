/// <reference path="../../_all.ts" />
var archivosArea;
(function (archivosArea) {
    var mainContentController = (function () {
        function mainContentController($mdSidenav, $rooteScope, $routeParams, config, toasterLite) {
            this.$mdSidenav = $mdSidenav;
            this.$rooteScope = $rooteScope;
            this.$routeParams = $routeParams;
            this.config = config;
            this.toasterLite = toasterLite;
            this.idProductor = this.$routeParams['idProductor'];
            if (this.idProductor === undefined)
                // No existe productor seleccionado, deberia elegir uno
                this.pedirQueElUsuarioSeleccioneUnProductor();
            else {
                if (location.hash.slice(3, 9) === 'upload')
                    this.abrirCuadroDeCargaDeArchivos();
                else
                    this.publicarElIdProductorActual();
            }
        }
        mainContentController.prototype.toggleSideNav = function () {
            this.$mdSidenav('right').toggle();
        };
        mainContentController.prototype.isSideNavOpen = function () {
            var open = this.$mdSidenav('right').isOpen();
            return open;
        };
        mainContentController.prototype.pedirQueElUsuarioSeleccioneUnProductor = function () {
            // El side nav no esta disponible. Mejor le enviamos un mensaje al usuario
            //this.toasterLite.info('Seleccione un productor por favor...');
            // this.toggleSideNav();
        };
        mainContentController.prototype.publicarElIdProductorActual = function () {
            /*
            Esta es la unica forma de hacer, por que capturando el evento routeChanged solo se puede hacer dentro de los
            controles ng-view
            */
            this.$rooteScope.$broadcast(this.config.eventIndex.archivos.productorSeleccionado, this.idProductor);
        };
        mainContentController.prototype.abrirCuadroDeCargaDeArchivos = function () {
            this.$rooteScope.$broadcast(this.config.eventIndex.archivos.abrirCuadroDeCargaDeArchivos, this.idProductor);
        };
        return mainContentController;
    }());
    mainContentController.$inject = ['$mdSidenav', '$rootScope', '$routeParams', 'config', 'toasterLite'];
    archivosArea.mainContentController = mainContentController;
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=mainContentController.js.map