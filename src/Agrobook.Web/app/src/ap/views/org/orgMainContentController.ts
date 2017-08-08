/// <reference path="../../../_all.ts" />

module apArea {
    export class orgMainContentController {
        static $inject = ['$routeParams', 'apQueryService']

        constructor(
            private $routeParams: angular.route.IRouteParamsService,
            private apQueryService: apQueryService
        ) {
            let idOrg = this.$routeParams['idOrg'];

            this.recuperarOrg(idOrg);
        }

        // Objetos seleccionados
        org: orgDto;

        //--------------------------
        // Private
        //--------------------------

        private recuperarOrg(id: string) {
            this.apQueryService.getOrg(id,
                new common.callbackLite<orgDto>(
                    value => {
                        this.org = value.data;
                    },
                    reason => { })
            );
        }
    }
}