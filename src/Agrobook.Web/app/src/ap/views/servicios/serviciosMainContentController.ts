/// <reference path="../../../_all.ts" />

module apArea {
    export class serviciosMainContentController {
        static $inject = ['$routeParams', '$scope', 'apQueryService', 'config']

        constructor(
            private $routeParams: angular.route.IRouteParamsService,
            private $scope: angular.IScope,
            private apQueryService: apQueryService,
            private config: common.config
        ) {
            this.idProd = this.$routeParams['idProd'];
            this.idServicio = this.$routeParams['idServicio'];

            let action = this.$routeParams['action'];
            this.action = action === undefined ? 'view' : action;
            this.esNuevo = this.action === 'new';

            this.recuperarProductorYResolverTitulo();

            this.abrirTabCorrespondiente();
            this.$scope.$on('$routeUpdate', (scope, next, current) => {
                this.abrirTabCorrespondiente();
            });
        }

        // Estados
        tabIndex: number;
        action: string;
        esNuevo: boolean;
        title: string;

        // Objetos seleccionados
        idProd: string;
        idServicio: string;
        productor: prodDto;

        //--------------------------
        // Private
        //--------------------------

        private onTabSelected(tabIndex: number) {
            let tabId: string;
            switch (tabIndex) {
                case 0: tabId = "resumen"; break;
                case 1: tabId = "parcela"; break;
                case 2: tabId = "diagnostico"; break;
                case 3: tabId = "prescripciones"; break;
                default: tabId = "resumen"; break;
            }

            window.location.replace(`#!/servicios/${this.idProd}/${this.idServicio}?tab=${tabId}&action=${this.action}`);
        }

        private abrirTabCorrespondiente() {
            let tabId = this.$routeParams['tab'];
            switch (tabId) {
                case 'resumen': this.tabIndex = 0; break;
                case 'parcela': this.tabIndex = 1; break;
                case 'diagnostico': this.tabIndex = 2; break;
                case 'prescripciones': this.tabIndex = 3; break;
                default: this.tabIndex = 0; break;
            }
        }

        private recuperarProductorYResolverTitulo() {
            this.apQueryService.getProd(this.idProd,
                new common.callbackLite<prodDto>(
                    value => {
                        this.productor = value.data;
                        this.resolverTitulo();
                    },
                    reason => { }
                ));
        }

        private resolverTitulo() {
            switch (this.action) {
                case 'new':
                    this.title = ` Nuevo servicio para ${this.productor.display}`;
                    break;
                case 'edit':
                    break;
                case 'view':
                    this.title = `Servicio ${'place servicio here'}`;
                    break;
            }                
        }
    }
}