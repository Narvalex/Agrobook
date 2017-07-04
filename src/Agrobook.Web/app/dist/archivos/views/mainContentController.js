/// <reference path="../../_all.ts" />
var archivosArea;
(function (archivosArea) {
    var mainContentController = (function () {
        function mainContentController($mdSidenav, $rooteScope, $routeParams, config, toasterLite, archivosQueryService) {
            this.$mdSidenav = $mdSidenav;
            this.$rooteScope = $rooteScope;
            this.$routeParams = $routeParams;
            this.config = config;
            this.toasterLite = toasterLite;
            this.archivosQueryService = archivosQueryService;
            this.idProductor = this.$routeParams['idProductor'];
            if (this.idProductor === undefined)
                // No existe productor seleccionado, deberia elegir uno
                // no op, for now
                return;
            else {
                // todo aqui sucede si existe productor seleccionado                
                this.cargarArchivosDelproductor();
                // No entiendo muy bien porque hice esto asi...
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
        mainContentController.prototype.cargarArchivosDelproductor = function () {
            var _this = this;
            this.archivosQueryService.obtenerArchivosDelProductor(this.idProductor, function (value) {
                _this.archivos = value.data;
            }, function (reason) {
            });
        };
        return mainContentController;
    }());
    mainContentController.$inject = ['$mdSidenav', '$rootScope', '$routeParams', 'config', 'toasterLite', 'archivosQueryService'];
    archivosArea.mainContentController = mainContentController;
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=mainContentController.js.map