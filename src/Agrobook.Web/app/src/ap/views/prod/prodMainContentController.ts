/// <reference path="../../../_all.ts" />

module apArea {
    export class prodMainContentController {
        static $inject = ['$routeParams', 'apQueryService']

        constructor(
            private $routeParams: angular.route.IRouteParamsService,
            private apQueryService: apQueryService,
        ) {
            let idProd = this.$routeParams['idProd'];

            this.recuperarProd(idProd);
        }

        // Objetos seleccionados
        prod: prodDto;

        //--------------------------
        // Private
        //--------------------------

        private recuperarProd(id: string) {
            this.apQueryService.getProd(id,
                new common.callbackLite<prodDto>(
                    value => {
                        this.prod = value.data;
                    },
                    reason => { })
            );
        }
    }
}