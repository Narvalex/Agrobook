/// <reference path="../../../_all.ts" />

module apArea {
    export class orgMainContentController {
        static $inject = ['$routeParams', '$scope', 'apQueryService']

        constructor(
            private $routeParams: angular.route.IRouteParamsService,
            private $scope: angular.IScope,
            private apQueryService: apQueryService,
        ) {
            let idOrg = this.$routeParams['idOrg'];

            this.recuperarOrg(idOrg);

            this.abrirTabCorrespondiente();
            this.$scope.$on('$routeUpdate', (scope, next, current) => {
                this.abrirTabCorrespondiente();
            });
        }

        // Estados
        tabIndex: number;

        // Objetos seleccionados
        org: orgDto;

        //--------------------------
        // Private
        //--------------------------

        private onTabSelected(tabIndex: number) {
            let tabId: string;
            switch (tabIndex) {
                case 0: tabId = "servicios"; break;
                case 1: tabId = "contratos"; break;
                case 2: tabId = "productores"; break;
                default: tabId = "servicios"; break;
            }

            window.location.replace(`#!/org/${this.org.id}?tab=${tabId}`);
        }

        private abrirTabCorrespondiente() {
            let tabId = this.$routeParams['tab'];
            switch (tabId) {
                case 'servicios': this.tabIndex = 0; break;
                case 'contratos': this.tabIndex = 1; break;
                case 'productores': this.tabIndex = 2; break;
                default: this.tabIndex = 0; break;
            }
        }

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