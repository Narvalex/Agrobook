/// <reference path="../../_all.ts" />

module archivosArea {
    export class mainContentController {
        static $inject = ['$mdSidenav', '$rootScope', '$routeParams', 'config', 'toasterLite'];

        constructor(
            private $mdSidenav: angular.material.ISidenavService,
            private $rooteScope: angular.IRootScopeService,
            private $routeParams: ng.route.IRouteParamsService,
            private config: common.config,
            private toasterLite: common.toasterLite
        ) {
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

        idProductor: string;

        toggleSideNav(): void {
            this.$mdSidenav('right').toggle();
        }

        isSideNavOpen(): boolean {
            var open = this.$mdSidenav('right').isOpen();
            return open;
        }

        pedirQueElUsuarioSeleccioneUnProductor() {
            // El side nav no esta disponible. Mejor le enviamos un mensaje al usuario
            //this.toasterLite.info('Seleccione un productor por favor...');
            // this.toggleSideNav();
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
    }
}