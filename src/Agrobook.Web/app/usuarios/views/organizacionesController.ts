/// <reference path="../../_all.ts" />

module usuariosArea {
    export class organizacionesController {
        static $inject = ['usuariosService', 'usuariosQueryService', 'toasterLite', '$routeParams', 'loginQueryService', '$rootScope',
            'config', '$mdPanel'];

        constructor(
            private usuariosService: usuariosService,
            private usuariosQueryService: usuariosQueryService,
            private toasterLite: common.toasterLite,
            private $routeParams: ng.route.IRouteParamsService,
            private loginQueryService: login.loginQueryService,
            private $rootScope: ng.IRootScopeService,
            private config: common.config,
            private $mdPanel: angular.material.IPanelService
        ) {
            this.idUsuario = this.$routeParams['idUsuario'];
            if (this.idUsuario === undefined)
                this.idUsuario = this.loginQueryService.tryGetLocalLoginInfo().usuario;

            this.obtenerOrganizaciones();
        }

        showDeleted: boolean;
        loaded: boolean;
        creandoOrg: boolean;
        agregandoUsuario: boolean;
        removiendoUsuario: boolean;

        idUsuario: string;

        // Nueva organizacion
        orgNombre: string;

        // lista de organizaciones
        organizaciones: organizacionDto[] = [];

        toggleShowDeleted() {
            this.showDeleted = !this.showDeleted;
        }

        crearNuevaOrganizacion() {
            this.creandoOrg = true;
            this.usuariosService.crearNuevaOrganizacion(this.orgNombre,
                value => {
                    this.organizaciones.push(value.data);
                    this.toasterLite.success("La organización " + this.orgNombre + " fue creada exitosamente");
                    this.creandoOrg = false;

                },
                reason => {
                    this.toasterLite.error('Ocurrió un error inesperado al intentar crear la organización ' + this.orgNombre);
                    this.creandoOrg = false;
                }
            );
        }

        eliminarOrganizacion(org: organizacionDto) {
            this.creandoOrg = true;
            this.usuariosService.eliminarOrganizacion(org, new common.callbackLite(
                value => {
                    for (var i = 0; i < this.organizaciones.length; i++) {
                        let o = this.organizaciones[i];
                        if (o.id === org.id) {
                            this.organizaciones[i].deleted = true;
                            break;
                        }
                    }
                    this.creandoOrg = false;
                    this.toasterLite.default('La organización fue eliminada');
                },
                reason => {
                    this.creandoOrg = false;
                    this.toasterLite.error('Ocurrió un error inesperado al intentar eliminar la organización');
                })
            );
        }

        restaurarOrganizacion(org: organizacionDto) {
            this.creandoOrg = true;
            this.usuariosService.restaurarOrganizacion(org, new common.callbackLite(
                value => {
                    for (var i = 0; i < this.organizaciones.length; i++) {
                        let o = this.organizaciones[i];
                        if (o.id === org.id) {
                            this.organizaciones[i].deleted = false;
                            break;
                        }
                    }
                    this.creandoOrg = false;
                    this.toasterLite.success('La organización fue restaurada');
                },
                reason => {
                    this.creandoOrg = false;
                    this.toasterLite.error('Ocurrió un error inesperado al intentar restaurar la organización');
                })
            );
        }

        agregarAOrganizacion($event, org: organizacionDto) {
            this.agregandoUsuario = true;
            this.usuariosService.agregarUsuarioALaOrganizacion(this.idUsuario, org.id,
                value => {
                    // Actualizar la interfaz
                    for (var i = 0; i < this.organizaciones.length; i++) {
                        if (this.organizaciones[i].id === org.id) {
                            this.organizaciones[i].usuarioEsMiembro = true;
                            break;
                        }
                    }
                    this.$rootScope.$broadcast(this.config.eventIndex.usuarios.usuarioAgregadoAOrganizacion,
                        {
                            idUsuario: this.idUsuario,
                            org: org
                        });

                    this.toasterLite.success(`Usuario agregado exitosamente a ${org.display}`);
                    this.agregandoUsuario = false;
                },
                reason => {
                    this.toasterLite.error('Hubo un error al incorporar el usuario a la organizacion', this.toasterLite.delayForever);
                    this.agregandoUsuario = false;
                }
            );
        }

        removerDeLaOrganizacion($event, org: organizacionDto) {
            this.removiendoUsuario = true;
            this.usuariosService.removerUsuarioDeOrganizacion(this.idUsuario, org.id,
                new common.callbackLite<any>(
                    value => {
                        // Actualizar la interfaz
                        for (var i = 0; i < this.organizaciones.length; i++) {
                            if (this.organizaciones[i].id === org.id) {
                                this.organizaciones[i].usuarioEsMiembro = false;
                                break;
                            }
                        }

                        this.removiendoUsuario = false;
                        this.toasterLite.default('Usuario removido de la organización');
                    },
                    reason => {
                        this.removiendoUsuario = false;
                        this.toasterLite.error('Hubo un error al intentar remover usuario de la organización');
                    })
            );
        }

        mostrarOpciones($event: Event, org: organizacionDto) {
            let position = this.$mdPanel.newPanelPosition()
                .relativeTo($event.srcElement)
                .addPanelPosition(
                this.$mdPanel.xPosition.ALIGN_START,
                this.$mdPanel.yPosition.BELOW);

            let config: angular.material.IPanelConfig = {
                attachTo: angular.element(document.body),
                controller: panelMenuController,
                controllerAs: 'vm',
                hasBackdrop: true,
                templateUrl: './views/organizaciones-menu-panel.html',
                position: position,
                trapFocus: true,
                locals: {
                    'org': org,
                    'parent': this
                },
                panelClass: 'menu-panel-container',
                openFrom: $event,
                focusOnOpen: true,
                zIndex: 150,
                disableParentScroll: true,
                clickOutsideToClose: true,
                escapeToClose: true,
            };

            this.$mdPanel.open(config);
        }

        //-------------------
        // INTERNAL
        //-------------------

        private obtenerOrganizaciones() {
            this.usuariosQueryService.obtenerOrganizacionesMarcadasDelUsuario(this.idUsuario,
                value => {
                    this.organizaciones = value.data;
                    this.loaded = true;
                },
                reason => this.toasterLite.error('Ocurrió un error al recuperar lista de organizaciones', this.toasterLite.delayForever)
            );
        }
    }

    class panelMenuController {
        static $inject = ['mdPanelRef'];

        constructor(
            private mdPanelRef: angular.material.IPanelRef
        ) {
        }

        org: organizacionDto;
        parent: organizacionesController;

        removerUsuario() {
            this.mdPanelRef.close().then(
                value => {
                    this.parent.removerDeLaOrganizacion(null, this.org);
                })
                .finally(() => this.mdPanelRef.destroy());
        }

        agregarUsuario() {
            this.mdPanelRef.close().then(
                value => {
                    this.parent.agregarAOrganizacion(null, this.org);
                })
                .finally(() => this.mdPanelRef.destroy());
        }

        eliminarOrg() {
            this.mdPanelRef.close().then(
                value => {
                    this.parent.eliminarOrganizacion(this.org);
                })
                .finally(() => this.mdPanelRef.destroy());
        }

        restaurarOrg() {
            this.mdPanelRef.close().then(
                value => {
                    this.parent.restaurarOrganizacion(this.org);
                })
                .finally(() => this.mdPanelRef.destroy());
        }

        cancelar() {
            this.mdPanelRef.close().finally(() => this.mdPanelRef.destroy());
        }
    }
}