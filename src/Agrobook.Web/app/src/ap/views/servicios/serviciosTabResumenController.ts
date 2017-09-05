﻿/// <reference path="../../../_all.ts" />

module apArea {
    export class serviciosTabResumenController {
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
        action: string;
        submitting: boolean = false;
        eliminando: boolean = false;
        restaurando: boolean = false;
        loading: boolean = false;

        // Objetos---------------------------------------
        momentInstance = moment;
        idServicio: string;
        idProd: string;
        // Edit/New
        orgConContratosSeleccionada: orgConContratos;
        contratoSeleccionado: contratoDto;
        fechaSeleccionada: Date
        // View
        servicio: servicioDto;

        // Listas-----------------------------------------
        orgsConContratos: orgConContratos[];

        // Api
        enableEditMode() {
            window.location.replace(`#!/servicios/${this.idProd}/${this.idServicio}?tab=resumen&action=edit`);
        }

        cancelar() {
            if (this.action === 'new')
                window.location.replace(`#!/prod/${this.idProd}`);
            else if (this.action === 'edit')
                window.location.replace(`#!/servicios/${this.idProd}/${this.idServicio}?tab=resumen&action=view`);
        }

        eliminar() {
            this.eliminando = true;
            this.apService.eliminarServicio(this.idServicio,
                new common.callbackLite<any>(
                    value => {
                        this.servicio.eliminado = true;
                        this.eliminando = false;
                    },
                    reason => {
                        this.eliminando = false;
                    })
            );
        }

        restaurar() {
            this.restaurando = true;
            this.apService.eliminarServicio(this.idServicio,
                new common.callbackLite<any>(
                    value => {
                        this.servicio.eliminado = false;
                        this.restaurando = false;
                    },
                    reason => {
                        this.restaurando = false;
                    })
            );
        }

        submit() {
            if (this.contratoSeleccionado === undefined || this.contratoSeleccionado.id === undefined) {
                this.toasterLite.error("Debe seleccionar un contrato");
                return;
            }
            if (this.fechaSeleccionada === undefined) {
                this.toasterLite.error("Debe seleccionar la fecha del contrato");
                return;
            }

            if (this.contratoSeleccionado.idOrg !== this.orgConContratosSeleccionada.org.id) {
                this.toasterLite.error('Debe seleccionar un contrato válido');
                return;
            }

            this.submitting = true;
            let servicio = new servicioDto(
                null,
                this.contratoSeleccionado.id,
                this.contratoSeleccionado.display,
                this.orgConContratosSeleccionada.org.id,
                this.orgConContratosSeleccionada.org.display,
                this.idProd,
                this.fechaSeleccionada);

            switch (this.action) {
                case 'new':
                    this.registrarNuevoServicio(servicio);
                    break;
                case 'edit':
                    this.actualizarDatosBasicos(servicio);
                    break;

            }
        }

        // Privados
        private cargarDatosSegunEstado() {
            this.action = this.$routeParams['action'] === undefined ? 'view' : this.$routeParams['action'];

            if (this.action === 'new') {
                this.recuperarYEstablecerContratos();
            }
            if (this.action === 'edit') {
                this.recuperarServicio(() => this.recuperarYEstablecerContratos());
            }
            else if (this.action === 'view') {
                    this.recuperarServicio();
            }
        }

        private recuperarYEstablecerContratos() {
            this.loading = true;
            this.apQueryService.getOrgsConContratos(this.idProd,
                new common.callbackLite<orgConContratos[]>(
                    value => {
                        this.orgsConContratos = value.data;
                        if (this.action == 'edit') {
                            // Setear en el combo la org actual;
                            for (var i = 0; i < this.orgsConContratos.length; i++) {
                                let orgConContratos = this.orgsConContratos[i];
                                if (orgConContratos.org.id === this.servicio.idOrg) {
                                    this.orgConContratosSeleccionada = orgConContratos;
                                    break;
                                }
                            }
                            // Setear en el combo el contrato actual
                            for (var i = 0; i < this.orgConContratosSeleccionada.contratos.length; i++) {
                                let contrato = this.orgConContratosSeleccionada.contratos[i];
                                if (contrato.id === this.servicio.idContrato) {
                                    this.contratoSeleccionado = contrato;
                                    break;
                                }
                            }

                            // Setear la fecha original del servicio
                            this.fechaSeleccionada = this.servicio.fecha;
                        }

                        this.loading = false;
                    },
                    reason => {
                        this.loading = false;
                    })
            );
        }

        private recuperarServicio(callback: () => any = () => { }) {
            this.loading = true;
            this.apQueryService.getServicio(this.idServicio,
                new common.callbackLite<servicioDto>(
                    value => {
                        this.servicio = value.data;
                        this.loading = false;
                        callback();
                    },
                    reason => {
                        this.loading = false;
                    })
            );
        }

        private registrarNuevoServicio(servicio: servicioDto) {
            this.apService.registrarNuevoServicio(servicio,
                new common.callbackLite<string>(
                    value => {
                        this.toasterLite.success('El servicio se registró con el id ' + value.data);
                        this.action = 'view';
                        servicio.id = value.data;
                        this.submitting = false;
                        this.$rootScope.$broadcast(this.config.eventIndex.ap_servicios.nuevoServicioCreado, servicio);
                        this.servicio = servicio;
                        window.location.replace(`#!/servicios/${this.idProd}/${this.idServicio}?tab=resumen&action=view`);
                    },
                    reason => {
                        this.submitting = false;
                    })
            );
        }

        private actualizarDatosBasicos(servicio: servicioDto) {
            this.apService.actualizarServicio(servicio,
                new common.callbackLite<any>(
                    value => {
                        this.toasterLite.success('Los datos básicos del servicio han sido actualizados');
                        this.action = 'view';
                        this.servicio = servicio;
                        this.submitting = false;
                        window.location.replace(`#!/servicios/${this.idProd}/${this.idServicio}?tab=resumen&action=view`);
                    },
                    reason => {
                        this.submitting = false;
                    })
            );
        }

        //-----------------------------
        // Archivos implementation
        //-----------------------------
        awTitle: string;
        awUploadLink: string;
        awFileUnits: common.fileUnit[] = [];
        awAllowUpload: boolean;
        awInit() {
            this.awTitle = 'Documento del informe final.';
            this.awAllowUpload = true;
            this.awUploadLink = 'Levantar archivo...';
        }
        awPrepareFiles(element: HTMLInputElement) {
            this.awService.resetFileInput();

            var vm = (angular.element(this)[0] as any) as serviciosTabResumenController;
            vm.$scope.$apply(scope => {
                vm.awFileUnits = vm.awService.prepareFiles(element.files, vm.awFileUnits);
            });
        }
    }
}