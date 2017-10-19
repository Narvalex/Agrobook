/// <reference path="../../../_all.ts" />

module apArea {
    export class prodMainContentController {
        static $inject = ['$routeParams', '$scope', 'apQueryService', 'config', 'loginService', 'toasterLite']

        constructor(
            private $routeParams: angular.route.IRouteParamsService,
            private $scope: ng.IScope,
            private apQueryService: apQueryService,
            private config: common.config,
            private loginService: login.loginService,
            private toasterLite: common.toasterLite
        ) {
            this.idProd = this.$routeParams['idProd'];

            this.recuperarProd(this.idProd);

            this.puedeIrAOrg = this.loginService.autorizar([config.claims.roles.Gerente, config.claims.roles.Tecnico]);

            this.abrirTabCorrespondiente();
            this.$scope.$on('$routeUpdate', (scope, next, current) => {
                this.abrirTabCorrespondiente();
            });
        }

        // Estados
        tabIndex: number;
        puedeIrAOrg: boolean;

        // Objetos seleccionados
        idProd: string;
        prod: prodDto;

        // API
        irAOrg(org: orgDto) {
            if (this.puedeIrAOrg)
                window.location.replace(`#!/org/${org.id}`);
            else
                this.toasterLite.info('Esta es la cooperativa a la cual pertence: ' + org.display);
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

            window.location.replace(`#!/prod/${this.idProd}?tab=${tabId}`);
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