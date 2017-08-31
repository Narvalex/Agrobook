/// <reference path="../../../_all.ts" />

module apArea {
    export class prodTabServiciosController {
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

            this.obtenerServicios();
        }
        // helpers
        momentInstance = moment;

        // Estados
        loadingServicios: boolean = false;
        ocultarEliminados: boolean = true;

        // Objetos
        idProd: string;

        // Listas
        servicios : servicioDto[];

        // Api
        nuevoServicio() {
            window.location.replace(`#!/servicios/${this.idProd}/new?tab=resumen&action=new`);
        }

        irAServicio(servicio: servicioDto) {
            window.location.replace(`#!/servicios/${this.idProd}/${servicio.id}`);
        }

        toggleMostrarEliminados() {
            this.ocultarEliminados = !this.ocultarEliminados;
        }

        // Privados
        private obtenerServicios() {
            this.loadingServicios = true;
            this.apQueryService.getServiciosPorProd(this.idProd,
                new common.callbackLite<servicioDto[]>(
                    value => {
                        this.servicios = value.data;
                        this.loadingServicios = false;
                    },
                    reason => {
                        this.loadingServicios = false;
                    })
            );
        }
    }
}