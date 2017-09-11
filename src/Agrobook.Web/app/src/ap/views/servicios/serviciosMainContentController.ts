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
            this.servicio = new servicioDto(this.$routeParams['idServicio'], null, null, null, null, null, null);
            
            this.resolveAction();

            this.recuperarProductorYResolverTitulo();

            this.abrirTabCorrespondiente();
            this.$scope.$on('$routeUpdate', (scope, next, current) => {
                this.resolveAction();
                this.abrirTabCorrespondiente();
            });
            this.$scope.$on(this.config.eventIndex.ap_servicios.nuevoServicioCreado, (e, args: servicioDto) => {
                this.esNuevo = false;
                this.action = 'view';
                this.servicio = args;
            });
            this.$scope.$on(this.config.eventIndex.ap_servicios.cambioDeParcelaEnServicio, (e, parcelaDisplay: string) => {
                this.servicio.parcelaDisplay = parcelaDisplay;
            });
        }

        // Estados
        loading: boolean;
        tabIndex: number;
        action: string;
        esNuevo: boolean;
        title: string;

        // Objetos seleccionados
        idProd: string;
        servicio: servicioDto;
        productor: prodDto;

        //--------------------------
        // Private
        //--------------------------

        private resolveAction() {
            let action = this.$routeParams['action'];
            this.action = action === undefined ? 'view' : action;
            this.esNuevo = this.action === 'new';
        }

        private onTabSelected(tabIndex: number) {
            let tabId: string;
            switch (tabIndex) {
                case 0: tabId = "resumen"; break;
                case 1: tabId = "parcela"; break;
                case 2: tabId = "diagnostico"; break;
                case 3: tabId = "prescripciones"; break;
                default: tabId = "resumen"; break;
            }

            window.location.replace(`#!/servicios/${this.idProd}/${this.servicio.id}?tab=${tabId}&action=${this.action}`);
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
            this.loading = true;
            this.apQueryService.getProd(this.idProd,
                new common.callbackLite<prodDto>(
                    value => {
                        this.productor = value.data;
                        this.apQueryService.getServicio(this.servicio.id,
                            new common.callbackLite<servicioDto>(
                                value => {
                                    this.servicio = value.data;
                                    this.loading = false;
                                },
                                reason => {
                                    this.loading = false;
                                })
                        );
                    },
                    reason => {
                        this.loading = false;
                    }
                ));
        }
    }
}