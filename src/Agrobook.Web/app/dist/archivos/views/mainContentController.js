/// <reference path="../../_all.ts" />
var archivosArea;
(function (archivosArea) {
    var mainContentController = (function () {
        function mainContentController($mdSidenav, $rooteScope, $routeParams, config, toasterLite, archivosQueryService, loginService, loginQueryService) {
            this.$mdSidenav = $mdSidenav;
            this.$rooteScope = $rooteScope;
            this.$routeParams = $routeParams;
            this.config = config;
            this.toasterLite = toasterLite;
            this.archivosQueryService = archivosQueryService;
            this.loginService = loginService;
            this.loginQueryService = loginQueryService;
            this.puedeCargarArchivos = false;
            this.puedeCambiarProductores = false;
            this.archivoSeleccionado = null;
            // Auth
            var roles = this.config.claims.roles;
            this.puedeCargarArchivos = this.loginService.autorizar([roles.Gerente, roles.Tecnico]);
            this.puedeCambiarProductores = this.loginService.autorizar([roles.Gerente, roles.Tecnico]);
            if (this.puedeCambiarProductores) {
                this.idProductor = this.$routeParams['idProductor'];
                if (this.idProductor === undefined) {
                    // No existe productor seleccionado, deberia elegir uno
                    // no op, for now
                    this.toggleSideNav();
                    return;
                }
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
            else {
                var usuario = this.loginQueryService.tryGetLocalLoginInfo();
                this.idProductor = usuario.usuario;
                this.cargarArchivosDelproductor();
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
        mainContentController.prototype.seleccionarArchivo = function (archivo) {
            this.archivoSeleccionado = archivo;
        };
        mainContentController.prototype.download = function (archivo) {
            this.archivosQueryService.download(this.idProductor, archivo.nombre, archivo.extension);
        };
        return mainContentController;
    }());
    mainContentController.$inject = ['$mdSidenav', '$rootScope', '$routeParams', 'config', 'toasterLite', 'archivosQueryService',
        'loginService', 'loginQueryService'];
    archivosArea.mainContentController = mainContentController;
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=mainContentController.js.map