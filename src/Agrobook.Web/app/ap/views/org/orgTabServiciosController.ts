/// <reference path="../../../_all.ts" />

module apArea {
    export class orgTabServiciosController {
        static $inject = ['$routeParams', 'apQueryService', '$mdSidenav', 'toasterLite']

        constructor(
            private $routeParams: angular.route.IRouteParamsService,
            private apQueryService: apQueryService,
            private $mdSidenav: angular.material.ISidenavService,
            private toasterLite: common.toasterLite
        ) {
            let idOrg = this.$routeParams['idOrg'];

            this.recuperarServiciosPorOrg(idOrg);
        }

        // Objetos seleccionados
        loadingServicios: boolean;
        ocultarEliminados: boolean = true;
        orderByDesc: boolean = true;

        // Listas
        contratos: contratoConServicios[];

        // objetos
        momentInstance: moment.MomentStatic = moment;


        //--------------------------
        // Api
        //--------------------------
        toogleOrder() {
            this.orderByDesc = !this.orderByDesc;
        }

        toogleColapsado(contrato: contratoConServicios) {
            for (var i = 0; i < this.contratos.length; i++) {
                var c = this.contratos[i];
                if (c.id === contrato.id) {
                    c.colapsado = !c.colapsado;
                }
            }
        }

        nuevoServicio() {
            this.$mdSidenav('left').open();
            this.toasterLite.default('Seleccione un productor para poder registrar un servicio', 7000, true, 'top left');
        }

        toggleMostrarEliminados() {
            this.ocultarEliminados = !this.ocultarEliminados;
        }

        irAServicio(servicio: servicioSlim) {
            window.location.href = `#!/servicios/${servicio.idProd}/${servicio.id}`;
        }

        //--------------------------
        // Private
        //--------------------------

        private recuperarServiciosPorOrg(idOrg: string) {
            this.loadingServicios = true;
            this.apQueryService.getServiciosPorOrgAgrupadosPorContrato(idOrg,
                new common.callbackLite<contratoConServicios[]>(
                    value => {
                        this.contratos = value.data;
                        this.loadingServicios = false;
                    },
                    reason => { })
            );
        }
    }
}