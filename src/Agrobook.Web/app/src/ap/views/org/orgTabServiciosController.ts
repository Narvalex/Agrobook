/// <reference path="../../../_all.ts" />

module apArea {
    export class orgTabServiciosController {
        static $inject = ['$routeParams', 'apQueryService']

        constructor(
            private $routeParams: angular.route.IRouteParamsService,
            private apQueryService: apQueryService
        ) {
            let idOrg = this.$routeParams['idOrg'];

            this.recuperarServiciosPorOrg(idOrg);
        }

        // Objetos seleccionados
        servicios: servicioDto[];

        //--------------------------
        // Private
        //--------------------------

        private recuperarServiciosPorOrg(idOrg: string) {
            this.apQueryService.getServiciosPorOrg(idOrg,
                new common.callbackLite<servicioDto[]>(
                    value => {
                        this.servicios = value.data;
                    },
                    reason => { })
            );
        }
    }
}