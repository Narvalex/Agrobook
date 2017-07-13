/// <reference path="../../_all.ts" />

module archivosArea {
    export class mainContentController {
        static $inject = ['$mdSidenav', '$scope', '$rootScope', '$routeParams', 'config', 'toasterLite', 'archivosQueryService',
            'loginService', 'loginQueryService'];

        constructor(
            private $mdSidenav: angular.material.ISidenavService,
            private $scope: angular.IScope,
            private $rooteScope: angular.IRootScopeService,
            private $routeParams: ng.route.IRouteParamsService,
            private config: common.config,
            private toasterLite: common.toasterLite,
            private archivosQueryService: archivosQueryService,
            private loginService: login.loginService,
            private loginQueryService: login.loginQueryService
        ) {
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

            this.$scope.$on(this.config.eventIndex.archivos.filtrar, (e, args) => {
                var filtro = args as common.TipoDeArchivo;
                this.filtroActivo = filtro;
                this.aplicarFiltro();
            });
        }

        puedeCargarArchivos: boolean = false;
        puedeCambiarProductores: boolean = false;

        idProductor: string;
        archivosOriginales: archivoDto[];
        archivoSeleccionado: archivoDto = null;

        // filtraje
        filtroActivo: common.TipoDeArchivo = this.config.tiposDeArchivos.todos;
        archivosFiltrados: archivoDto[] = [];

        toggleSideNav(): void {
            this.$mdSidenav('right').toggle();
        }

        isSideNavOpen(): boolean {
            var open = this.$mdSidenav('right').isOpen();
            return open;
        }

        publicarElIdProductorActual() {
            /*
            Esta es la unica forma de hacer, por que capturando el evento routeChanged solo se puede hacer dentro de los
            controles ng-view
            */
            this.$rooteScope.$broadcast(this.config.eventIndex.archivos.productorSeleccionado, this.idProductor);
        }

        abrirCuadroDeCargaDeArchivos() {
            this.$rooteScope.$broadcast(this.config.eventIndex.archivos.abrirCuadroDeCargaDeArchivos, this.idProductor);
        }

        cargarArchivosDelproductor() {
            this.archivosQueryService.obtenerArchivosDelProductor(this.idProductor,
                value => {
                    this.archivosOriginales = value.data;
                    this.aplicarFiltro();
                },
                reason => {
                }
            );
        }

        seleccionarArchivo(archivo: archivoDto) {
            this.archivoSeleccionado = archivo;
        }

        download(archivo: archivoDto) {
            this.archivosQueryService.download(this.idProductor, archivo.nombre, archivo.extension);
        }

        obtenerTemaDePreview(): string {
            if (this.archivoSeleccionado === null || this.archivoSeleccionado === undefined)
                return 'default';
            var ext = this.archivoSeleccionado.extension;
            if (this.archivosQueryService.esFoto(ext))
                return 'dark-grey';
            else
                return 'default';
        }

        private aplicarFiltro() {
            this.archivosFiltrados = [];
            var tipos = this.config.tiposDeArchivos;
            var filtrado = this.filtroActivo.display;

            if (filtrado === tipos.todos.display) {
                this.archivosFiltrados = this.archivosOriginales;
            }
            else if (filtrado === tipos.fotos.display) {
                this.archivosOriginales.forEach(x => {
                    if (this.archivosQueryService.esFoto(x.extension))
                        this.archivosFiltrados.push(x);
                });
            }
            else if (filtrado === tipos.pdf.display) {
                this.archivosOriginales.forEach(x => {
                    if (this.archivosQueryService.esPdf(x.extension))
                        this.archivosFiltrados.push(x);
                });
            }
            else if (filtrado === tipos.mapas.display) {
                this.archivosOriginales.forEach(x => {
                    if (this.archivosQueryService.esMapa(x.extension))
                        this.archivosFiltrados.push(x);
                });
            }
            else if (filtrado === tipos.excel.display) {
                this.archivosOriginales.forEach(x => {
                    if (this.archivosQueryService.esExcel(x.extension))
                        this.archivosFiltrados.push(x);
                });
            }
            else if (filtrado === tipos.word.display) {
                this.archivosOriginales.forEach(x => {
                    if (this.archivosQueryService.esWord(x.extension))
                        this.archivosFiltrados.push(x);
                });
            }
            else if (filtrado === tipos.powerPoint.display) {
                this.archivosOriginales.forEach(x => {
                    if (this.archivosQueryService.esPowerPoint(x.extension))
                        this.archivosFiltrados.push(x);
                });
            }
        }


    }
}