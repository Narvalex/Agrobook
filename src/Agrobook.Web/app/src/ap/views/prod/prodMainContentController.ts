/// <reference path="../../../_all.ts" />

module apArea {
    export class prodMainContentController {
        static $inject = ['$routeParams', '$scope', 'apQueryService']

        constructor(
            private $routeParams: angular.route.IRouteParamsService,
            private $scope: ng.IScope,
            private apQueryService: apQueryService,
        ) {
            let idProd = this.$routeParams['idProd'];

            this.recuperarProd(idProd);

            this.abrirTabCorrespondiente();
            this.$scope.$on('$routeUpdate', (scope, next, current) => {
                this.abrirTabCorrespondiente();
            });
        }

        // Estados
        tabIndex: number;

        // Objetos seleccionados
        prod: prodDto;

        // API
        irAOrg(org: orgDto) {
            window.location.replace(`#!/org/${org.id}`);
        }

        //--------------------------
        // Private
        //--------------------------

        private onTabSelected(tabIndex: number) {
            let tabId: string;
            switch (tabIndex) {
                case 0: tabId = "servicios"; break;
                case 1: tabId = "parcelas"; break;
                default: tabId = "servicios"; break;
            }

            window.location.replace(`#!/prod/${this.prod.id}?tab=${tabId}`);
        }

        private abrirTabCorrespondiente() {
            let tabId = this.$routeParams['tab'];
            switch (tabId) {
                case 'servicios': this.tabIndex = 0; break;
                case 'parcelas': this.tabIndex = 1; break;
                default: this.tabIndex = 0; break;
            }
        }

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