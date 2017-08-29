/// <reference path="../../../_all.ts" />

module apArea {
    export class prodTabServiciosController {
        static $inject = ['config', 'apService', 'apQueryService', 'toasterLite', '$routeParams', '$mdPanel'];

        constructor(
            private config: common.config,
            private apService: apService,
            private apQueryService: apQueryService,
            private toasterLite: common.toasterLite,
            private $routeParams: angular.route.IRouteParamsService,
            private $mdPanel: angular.material.IPanelService
        ) {
            this.idProd = this.$routeParams['idProd'];
        }

        // Estados


        // Objetos
        idProd: string;

        // Listas


        // Api
        nuevoServicio() {
            window.location.replace(`#!/servicios/${this.idProd}/new?tab=resumen&action=new`);
        }

        // Privados

    }
}