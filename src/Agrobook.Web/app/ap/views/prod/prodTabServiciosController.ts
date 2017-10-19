/// <reference path="../../../_all.ts" />

module apArea {
    export class prodTabServiciosController {
        static $inject = ['config', 'apService', 'apQueryService', 'toasterLite', '$routeParams', '$mdPanel', 'loginService'];

        constructor(
            private config: common.config,
            private apService: apService,
            private apQueryService: apQueryService,
            private toasterLite: common.toasterLite,
            private $routeParams: angular.route.IRouteParamsService,
            private $mdPanel: angular.material.IPanelService,
            private loginService: login.loginService
        ) {
            this.idProd = this.$routeParams['idProd'];

            var roles = this.config.claims.roles;
            this.puedeCrearServicios = this.loginService.autorizar([roles.Gerente, roles.Tecnico]);

            this.obtenerServicios();
        }
        // helpers
        momentInstance = moment;

        // Estados
        loadingServicios: boolean = false;
        ocultarEliminados: boolean = true;
        puedeCrearServicios: boolean;

        // Objetos
        idProd: string;

        // Listas
        servicios : servicioDto[];

        // Api
        nuevoServicio() {
            window.location.href = `#!/servicios/${this.idProd}/new?tab=resumen&action=new`;
        }

        irAServicio(servicio: servicioDto) {
            window.location.href = `#!/servicios/${this.idProd}/${servicio.id}`;
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