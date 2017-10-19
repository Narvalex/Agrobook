/// <reference path="../../_all.ts" />

module apArea {
    export class mainContentController {
        static $inject = ['apQueryService', 'toasterLite', 'loginService', 'loginQueryService', 'config']

        constructor(
            private apQueryService: apQueryService,
            private toasterLite: common.toasterLite,
            private loginService: login.loginService,
            private loginQueryService: login.loginQueryService,
            private config: common.config
        ) {
            var claims = config.claims;
            var puedeVerElMainDeAp = this.loginService.autorizar([claims.roles.Gerente, claims.roles.Tecnico]);
            if (puedeVerElMainDeAp)
                this.getServicios();
            else {
                window.location.href = `./index.html#!/prod/${this.loginQueryService.tryGetLocalLoginInfo().usuario}`;
            }
        }

        // Estado
        loading: boolean;
        sinServicios: boolean;

        // Model
        momentJs = moment;
        servicios: servicioParaDashboardDto[];

        irAlServicio(servicio: servicioParaDashboardDto) {
            window.location.href = `./index.html#!/servicios/${servicio.idProd}/${servicio.id}?tab=resumen&action=view`;
        }

        getServicios() {
            this.loading = true;
            this.apQueryService.getUltimosServicios(30, new common.callbackLite<servicioParaDashboardDto[]>(
                value => {
                    if (value.data.length === 0) {
                        this.sinServicios = true;
                    }
                    else {
                        this.servicios = value.data;
                    }
                    this.loading = false;
                },
                reason => {
                    this.loading = false;
                    this.toasterLite.error('Hubo un error al obtener los últimos servicios', this.toasterLite.delayForever);
                })
            );
        }
    }
}