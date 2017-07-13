/// <reference path="../../_all.ts" />
var archivosArea;
(function (archivosArea) {
    var mainContentController = (function () {
        function mainContentController($mdSidenav, $scope, $rooteScope, $routeParams, config, toasterLite, archivosQueryService, loginService, loginQueryService) {
            var _this = this;
            this.$mdSidenav = $mdSidenav;
            this.$scope = $scope;
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
            // filtraje
            this.filtroActivo = this.config.tiposDeArchivos.todos;
            this.archivosFiltrados = [];
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
            this.$scope.$on(this.config.eventIndex.archivos.filtrar, function (e, args) {
                var filtro = args;
                _this.filtroActivo = filtro;
                _this.aplicarFiltro();
            });
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
                _this.archivosOriginales = value.data;
                _this.aplicarFiltro();
            }, function (reason) {
            });
        };
        mainContentController.prototype.seleccionarArchivo = function (archivo) {
            this.archivoSeleccionado = archivo;
        };
        mainContentController.prototype.download = function (archivo) {
            this.archivosQueryService.download(this.idProductor, archivo.nombre, archivo.extension);
        };
        mainContentController.prototype.obtenerTemaDePreview = function () {
            if (this.archivoSeleccionado === null || this.archivoSeleccionado === undefined)
                return 'default';
            var ext = this.archivoSeleccionado.extension;
            if (this.archivosQueryService.esFoto(ext))
                return 'dark-grey';
            else
                return 'default';
        };
        mainContentController.prototype.aplicarFiltro = function () {
            var _this = this;
            this.archivosFiltrados = [];
            var tipos = this.config.tiposDeArchivos;
            var filtrado = this.filtroActivo.display;
            if (filtrado === tipos.todos.display) {
                this.archivosFiltrados = this.archivosOriginales;
            }
            else if (filtrado === tipos.fotos.display) {
                this.archivosOriginales.forEach(function (x) {
                    if (_this.archivosQueryService.esFoto(x.extension))
                        _this.archivosFiltrados.push(x);
                });
            }
            else if (filtrado === tipos.pdf.display) {
                this.archivosOriginales.forEach(function (x) {
                    if (_this.archivosQueryService.esPdf(x.extension))
                        _this.archivosFiltrados.push(x);
                });
            }
            else if (filtrado === tipos.mapas.display) {
                this.archivosOriginales.forEach(function (x) {
                    if (_this.archivosQueryService.esMapa(x.extension))
                        _this.archivosFiltrados.push(x);
                });
            }
            else if (filtrado === tipos.excel.display) {
                this.archivosOriginales.forEach(function (x) {
                    if (_this.archivosQueryService.esExcel(x.extension))
                        _this.archivosFiltrados.push(x);
                });
            }
            else if (filtrado === tipos.word.display) {
                this.archivosOriginales.forEach(function (x) {
                    if (_this.archivosQueryService.esWord(x.extension))
                        _this.archivosFiltrados.push(x);
                });
            }
            else if (filtrado === tipos.powerPoint.display) {
                this.archivosOriginales.forEach(function (x) {
                    if (_this.archivosQueryService.esPowerPoint(x.extension))
                        _this.archivosFiltrados.push(x);
                });
            }
        };
        return mainContentController;
    }());
    mainContentController.$inject = ['$mdSidenav', '$scope', '$rootScope', '$routeParams', 'config', 'toasterLite', 'archivosQueryService',
        'loginService', 'loginQueryService'];
    archivosArea.mainContentController = mainContentController;
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=mainContentController.js.map