/// <reference path="../../../_all.ts" />

module apArea {
    export class serviciosTabResumenController {
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
            this.esNuevo = true;
        }

        // Estados
        esNuevo: boolean;

        // Objetos
        idProd: string;

        // Listas


        // Api
        cancelar() {
            window.location.replace(`#!/prod/${this.idProd}`);
        }

        // Privados

    }
}