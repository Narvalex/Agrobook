/// <reference path="../../../_all.ts" />

module apArea {
    export class serviciosTabResumenController {
        static $inject = ['config', 'apService', 'apQueryService', 'toasterLite', '$routeParams', '$rootScope', '$scope', 'loginService'];

        constructor(
            private config: common.config,
            private apService: apService,
            private apQueryService: apQueryService,
            private toasterLite: common.toasterLite,
            private $routeParams: angular.route.IRouteParamsService,
            private $rootScope: angular.IRootScopeService,
            private $scope: angular.IScope,
            private loginService: login.loginService
        ) {
            this.idProd = this.$routeParams['idProd'];
            this.idServicio = this.$routeParams['idServicio'];
            this.idColeccion = `${this.config.categoriaDeArchivos.servicioDatosBasicos}-${this.idServicio}`;

            let roles = config.claims.roles;
            this.tienePermiso = this.loginService.autorizar([roles.Gerente, roles.Tecnico]);

            this.$scope.$on('$routeUpdate', (scope, next, current) => {
                this.cargarDatosSegunEstado();
            });
            this.$scope.$on(this.config.eventIndex.ap_servicios.cambioDeParcelaEnServicio, (e, parcelaDisplay: string) => {
                this.servicio.parcelaDisplay = parcelaDisplay;
            });

            this.cargarDatosSegunEstado();
        }

        // Estados--------------------------------------
        action: string;
        submitting: boolean = false;
        eliminando: boolean = false;
        restaurando: boolean = false;
        loading: boolean = false;
        tienePermiso: boolean = false;

        // Objetos---------------------------------------
        momentInstance = moment;
        idServicio: string;
        idProd: string;
        idColeccion: string;
        // Edit/New
        orgConContratosSeleccionada: orgConContratos;
        contratoSeleccionado: contratoDto;
        fechaSeleccionada: Date
        // View
        servicio: servicioDto;

        // Listas-----------------------------------------
        orgsConContratos: orgConContratos[];

        // Api
        goToOrg() {
            if (!this.tienePermiso) {
                this.toasterLite.info('Usted pertenece a ' + this.servicio.orgDisplay);
                return;
            }

            window.location.href = `#!/org/${this.servicio.idOrg}`;
        }

        enableEditMode() {
            window.location.href = `#!/servicios/${this.idProd}/${this.idServicio}?tab=resumen&action=edit`;
        }

        cancelar() {
            if (this.action === 'new')
                window.location.href = `#!/prod/${this.idProd}`;
            else if (this.action === 'edit')
                window.location.href = `#!/servicios/${this.idProd}/${this.idServicio}?tab=resumen&action=view`;
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
                        this.toasterLite.error("No se pudo eliminar. Lo sentimos");
                    })
            );
        }

        restaurar() {
            this.restaurando = true;
            this.apService.restaurarServicio(this.idServicio,
                new common.callbackLite<any>(
                    value => {
                        this.servicio.eliminado = false;
                        this.restaurando = false;
                    },
                    reason => {
                        this.restaurando = false;
                        this.toasterLite.error("No se pudo restaurar. Lo sentimos!");
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
                this.idServicio,
                this.contratoSeleccionado.id,
                this.contratoSeleccionado.esAdenda,
                this.contratoSeleccionado.idContratoDeLaAdenda,
                this.contratoSeleccionado.display,
                this.orgConContratosSeleccionada.org.id,
                this.orgConContratosSeleccionada.org.display,
                this.idProd,
                null,
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
                // Esta logica esta aqui para que no se cargue el tab cada vez que se pasa por el 
                if (this.servicio === undefined || this.servicio === null)
                    this.recuperarServicio();
            }
        }

        private recuperarYEstablecerContratos() {
            this.loading = true;
            this.apQueryService.getOrgsConContratosDelProductor(this.idProd,
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
                        this.idServicio = servicio.id;
                        this.submitting = false;
                        this.$rootScope.$broadcast(this.config.eventIndex.ap_servicios.nuevoServicioCreado, servicio);
                        this.servicio = servicio;
                        window.location.href = `#!/servicios/${this.idProd}/${this.idServicio}?tab=resumen&action=view`;
                    },
                    reason => {
                        this.submitting = false;
                    })
            );
        }

        private actualizarDatosBasicos(servicio: servicioDto) {
            this.apService.editarDatosBasicosDelServicio(servicio,
                new common.callbackLite<any>(
                    value => {
                        this.toasterLite.success('Los datos básicos del servicio han sido actualizados');
                        this.action = 'view';
                        this.servicio = servicio;
                        this.submitting = false;
                        window.location.replace(`#!/servicios/${this.idProd}/${this.idServicio}?tab=resumen&action=view`);
                    },
                    reason => {
                        this.toasterLite.error('Hubo un error al intentar editar el servicio. Verifique los datos por favor.');
                        this.submitting = false;
                    })
            );
        }

        //-----------------------------
        // Archivos implementation
        //-----------------------------
        //awTitle: string;
        //awUploadLink: string;
        //awFileUnits: common.fileUnit[] = [];
        //awAllowUpload: boolean;
        //awInit() {
        //    this.awTitle = 'Documento del informe final.';
        //    this.awAllowUpload = true;
        //    this.awUploadLink = 'Levantar archivo...';
        //}
        //awPrepareFiles(element: HTMLInputElement) {
        //    this.awService.resetFileInput();

        //    var vm = (angular.element(this)[0] as any) as serviciosTabResumenController;
        //    vm.$scope.$apply(scope => {
        //        vm.awFileUnits = vm.awService.prepareFiles(element.files, vm.awFileUnits);
        //    });
        //}
    }
}