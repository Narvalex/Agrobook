/// <reference path="../../../_all.ts" />

module apArea {
    export class serviciosTabParcelaController {
        static $inject = ['config', 'apService', 'apQueryService', 'toasterLite', '$routeParams', '$rootScope', '$scope'];

        constructor(
            private config: common.config,
            private apService: apService,
            private apQueryService: apQueryService,
            private toasterLite: common.toasterLite,
            private $routeParams: angular.route.IRouteParamsService,
            private $rootScope: angular.IRootScopeService,
            private $scope: angular.IScope,
        ) {
            this.idProd = this.$routeParams['idProd'];
            this.idColeccion = `${this.config.categoriaDeArchivos.servicioParcelas}-${this.idProd}`;
            this.idServicio = this.$routeParams['idServicio'];

            this.$scope.$on('$routeUpdate', (scope, next, current) => {
                this.cargarDatosSegunEstado();
            });

            this.cargarDatosSegunEstado();
        }

        // Estados--------------------------------------
        loading: boolean = false;
        action: string;
        tieneParcela: boolean = false;
        submitting: boolean = false;

        // Objetos---------------------------------------
        idServicio: string;
        idProd: string;
        servicio: servicioDto;
        parcela: parcelaDto;
        idColeccion: string;
        // Edit/New
        parcelaSeleccionada: parcelaDto;

        // Listas-----------------------------------------
        // Edit/New
        parcelas: parcelaDto[];


        // Api
        actualizar() {
            window.location.replace(`#!/servicios/${this.idProd}/${this.idServicio}?tab=parcela&action=edit`);
        }

        cancelar() {
            this.parcelaSeleccionada = undefined;
            window.location.replace(`#!/servicios/${this.idProd}/${this.idServicio}?tab=parcela&action=view`);
        }

        submit() {
            this.submitting = true;
            if (this.parcelaSeleccionada === undefined) {
                this.toasterLite.error('Debe seleccionar una parcela');
                this.submitting = false;
                return;
            }
            else {
                if (this.parcelaSeleccionada.id === this.servicio.parcelaId) {
                    this.toasterLite.info('La parcela es la misma que antes. No hay cambios');
                    this.submitting = false;
                    this.cancelar();
                    return;
                }
            }

            if (this.tieneParcela)
                this.cambiarParcela();
            else
                this.especificarParcela();
        }

        // Privados
        cargarDatosSegunEstado() {
            this.action = this.$routeParams['action'] === undefined ? 'view' : this.$routeParams['action'];
            if (this.servicio === undefined && this.idServicio.toLowerCase() !== 'new')
                this.recuperarServicioYLaParcelaSiTiene();

            switch (this.action) {
                case 'edit':
                    this.recuperarParcelas();
                    break;

                case 'view':
                    break;
            }
        }

        private recuperarServicioYLaParcelaSiTiene() {
            this.loading = true;
            this.apQueryService.getServicio(this.idServicio,
                new common.callbackLite<servicioDto>(
                    value => {
                        this.servicio = value.data;
                        this.tieneParcela = this.servicio.parcelaId !== undefined && this.servicio.parcelaId !== null;
                        if (this.tieneParcela) {
                            // esta logica no tiene sentido... solamente esta por las hectareas
                            this.apQueryService.getParcela(this.servicio.parcelaId,
                                new common.callbackLite<parcelaDto>(
                                    value => {
                                        this.parcela = value.data;
                                        this.loading = false;
                                    },
                                    reason => {
                                        this.loading = false;
                                    })
                            );
                        }
                        else 
                            this.loading = false;
                    },
                    reason => {
                        this.loading = false;
                    })
            );
        }

        private recuperarParcelas() {
            this.loading = true;
            this.apQueryService.getParcelasDelProd(this.idProd,
                new common.callbackLite<parcelaDto[]>(
                    value => {
                        this.parcelas = value.data.filter(x => !x.eliminado);
                        if (this.tieneParcela) {
                            for (var i = 0; i < this.parcelas.length; i++) {
                                let parcela = this.parcelas[i];
                                if (parcela.id === this.servicio.parcelaId) {
                                    this.parcelaSeleccionada = parcela;
                                    break;
                                }
                            }
                        }
                        this.loading = false;
                    },
                    reason => {
                        this.loading = false;
                    }
            ));
        }

        private especificarParcela() {
            let parcela = this.parcelaSeleccionada;
            this.apService.especificarParcelaDelServicio(this.idServicio, parcela,
                new common.callbackLite<any>(
                    value => {
                        this.servicio.parcelaId = parcela.id;
                        this.servicio.parcelaDisplay = parcela.display;
                        this.parcela = parcela;
                        this.toasterLite.success('Parcela especificada correctamente');
                        this.tieneParcela = true;
                        this.$rootScope.$broadcast(this.config.eventIndex.ap_servicios.cambioDeParcelaEnServicio, parcela.display);
                        window.location.replace(`#!/servicios/${this.idProd}/${this.idServicio}?tab=parcela&action=view`);
                        this.submitting = false;
                    },
                    reason => {
                        this.submitting = false;
                    }
                ));
        }

        private cambiarParcela() {
            let parcela = this.parcelaSeleccionada;
            this.apService.cambiarParcelaDelServicio(this.idServicio, parcela,
                new common.callbackLite<any>(
                    value => {
                        this.servicio.parcelaId = parcela.id;
                        this.servicio.parcelaDisplay = parcela.display;
                        this.parcela = parcela;
                        this.toasterLite.success('La parcela ha sido cambiada');
                        this.$rootScope.$broadcast(this.config.eventIndex.ap_servicios.cambioDeParcelaEnServicio, parcela.display);
                        window.location.replace(`#!/servicios/${this.idProd}/${this.idServicio}?tab=parcela&action=view`);
                        this.submitting = false;
                    },
                    reason => {
                        this.submitting = false;
                    }
                ));
        }
    }
}