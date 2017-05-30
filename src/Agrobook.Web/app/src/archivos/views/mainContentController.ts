/// <reference path="../../_all.ts" />

module archivosArea {
    export class mainContentController {
        static $inject = ['$mdSidenav', '$rootScope', '$routeParams', 'config'];

        constructor(
            private $mdSidenav: angular.material.ISidenavService,
            private $rooteScope: angular.IRootScopeService,
            private $routeParams: ng.route.IRouteParamsService,
            private config: common.config
        ) {
            this.setearElProductorEnTodosLados();
        }

        idProductor: string;

        toggleSideNav(): void {
            this.$mdSidenav('right').toggle();
        }

        isSideNavOpen(): boolean {
            var open = this.$mdSidenav('right').isOpen();
            return open;
        }

        setearElProductorEnTodosLados() {
            /*
            Esta es la unica forma de hacer, por que capturando el evento routeChanged solo se puede hacer dentro de los
            controles ng-view
            */
            this.idProductor = this.$routeParams['idProductor'];
            this.$rooteScope.$broadcast(this.config.eventIndex.archivos.productorSeleccionado, this.idProductor);
        }
    }
}