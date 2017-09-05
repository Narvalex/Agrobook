/// <reference path="../../../_all.ts" />

module apArea {
    export class serviciosTabParcelaController {
        static $inject = ['config', 'apService', 'apQueryService', 'toasterLite', '$routeParams', '$rootScope', '$scope', 'awService'];

        constructor(
            private config: common.config,
            private apService: apService,
            private apQueryService: apQueryService,
            private toasterLite: common.toasterLite,
            private $routeParams: angular.route.IRouteParamsService,
            private $rootScope: angular.IRootScopeService,
            private $scope: angular.IScope,
            private awService: common.filesWidgetService
        ) {
            this.idProd = this.$routeParams['idProd'];
            this.idServicio = this.$routeParams['idServicio'];

            this.$scope.$on('$routeUpdate', (scope, next, current) => {
                this.cargarDatosSegunEstado();
            });

            this.cargarDatosSegunEstado();
            this.awInit();
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
            if (this.servicio === undefined)
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
                            this.apQueryService.getParcelasDelProd
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
                        this.submitting = false;
                        window.location.replace(`#!/servicios/${this.idProd}/${this.idServicio}?tab=parcela&action=view`);
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
                        this.submitting = false;
                        window.location.replace(`#!/servicios/${this.idProd}/${this.idServicio}?tab=parcela&action=view`);
                    },
                    reason => {
                        this.submitting = false;
                    }
                ));
        }

        //-----------------------------
        // Archivos implementation
        //-----------------------------
        awTitle: string;
        awUploadLink: string;
        awFileUnits: common.fileUnit[] = [];
        awAllowUpload: boolean;
        awInit() {
            this.awTitle = 'Mapas de límites y puntos georeferenciados';
            this.awAllowUpload = true;
            this.awUploadLink = 'Levantar archivo...';
        }
        awPrepareFiles(element: HTMLInputElement) {
            this.awService.resetFileInput();

            var vm = (angular.element(this)[0] as any) as serviciosTabParcelaController;
            vm.$scope.$apply(scope => {
                vm.awFileUnits = vm.awService.prepareFiles(element.files, vm.awFileUnits);
            });
        }
    }
}