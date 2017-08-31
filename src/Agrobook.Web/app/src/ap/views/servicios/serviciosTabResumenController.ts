/// <reference path="../../../_all.ts" />

module apArea {
    export class serviciosTabResumenController {
        static $inject = ['config', 'apService', 'apQueryService', 'toasterLite', '$routeParams', '$mdPanel', '$rootScope', '$scope'];

        constructor(
            private config: common.config,
            private apService: apService,
            private apQueryService: apQueryService,
            private toasterLite: common.toasterLite,
            private $routeParams: angular.route.IRouteParamsService,
            private $mdPanel: angular.material.IPanelService,
            private $rootScope: angular.IRootScopeService,
            private $scope: angular.IScope
        ) {
            this.idProd = this.$routeParams['idProd'];
            this.action = this.$routeParams['action'] === undefined ? 'view' : this.$routeParams['action'];

            this.$scope.$on('$routeUpdate', (scope, next, current) => {
                this.action =  this.$routeParams['action'];
            });

            this.recuperarContratos();
        }

        // Estados
        action: string;
        submitting: boolean = false;

        // Objetos
        idProd: string;
        orgConContratosSeleccionada: orgConContratos;
        contratoSeleccionado: contratoDto;
        fechaSeleccionada: Date

        // Listas
        orgsConContratos: orgConContratos[];

        // Api
        cancelar() {
            window.location.replace(`#!/prod/${this.idProd}`);
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

            this.submitting = true;
            switch (this.action) {
                case 'new':
                    this.registrarNuevoServicio();
                    break;
            }
        }

        // Privados
        private recuperarContratos() {
            this.apQueryService.getOrgsConContratos(this.idProd,
                new common.callbackLite<orgConContratos[]>(
                    value => {
                        this.orgsConContratos = value.data;
                    },
                    reason => {
                    })
            );
        }

        private registrarNuevoServicio() {
            let servicio = new servicioDto(null, this.contratoSeleccionado.id, this.orgConContratosSeleccionada.org.id, this.idProd, this.fechaSeleccionada);
            this.apService.registrarNuevoServicio(
                new servicioDto(null, this.contratoSeleccionado.id, this.orgConContratosSeleccionada.org.id, this.idProd, this.fechaSeleccionada),
                new common.callbackLite<string>(
                    value => {
                        this.toasterLite.success('El servicio se registró con el id ' + value.data);
                        this.action = 'view';
                        servicio.id = value.data;
                        this.submitting = false;
                        this.$rootScope.$broadcast(this.config.eventIndex.ap_servicios.nuevoServicioCreado, servicio);
                    },
                    reason => {
                        this.submitting = false;
                    })
            );
        }
    }
}