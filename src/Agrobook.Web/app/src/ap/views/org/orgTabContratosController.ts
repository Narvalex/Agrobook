/// <reference path="../../../_all.ts" />

module apArea {
    export class orgTabContratosController {
        static $inject = ['$routeParams', 'apQueryService']

        constructor(
            private $routeParams: angular.route.IRouteParamsService,
            private apQueryService: apQueryService
        ) {
            this.idOrg = this.$routeParams['idOrg'];

            this.recuperarContratos();
        }

        idOrg: string;

        // listas
        contratos: contratoDto[];
        

        //--------------------------
        // Private
        //--------------------------
        private recuperarContratos() {
            this.apQueryService.getContratos(this.idOrg,
                new common.callbackLite<contratoDto[]>(
                    value => {
                        this.contratos = value.data;
                    },
                    reason => { }
                ));
        }
    }
}