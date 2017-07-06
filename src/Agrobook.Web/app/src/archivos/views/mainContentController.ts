/// <reference path="../../_all.ts" />

module archivosArea {
    export class mainContentController {
        static $inject = ['$mdSidenav', '$rootScope', '$routeParams', 'config', 'toasterLite', 'archivosQueryService'];

        constructor(
            private $mdSidenav: angular.material.ISidenavService,
            private $rooteScope: angular.IRootScopeService,
            private $routeParams: ng.route.IRouteParamsService,
            private config: common.config,
            private toasterLite: common.toasterLite,
            private archivosQueryService: archivosQueryService
        ) {
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

        idProductor: string;
        archivos: archivoDto[];
        archivoSeleccionado: archivoDto;

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
                    this.archivos = value.data;
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
    }
}