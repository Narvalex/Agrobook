/// <reference path="../../_all.ts" />

module usuariosArea {
    export class organizacionesController {
        static $inject = ['usuariosService', 'usuariosQueryService', 'toasterLite', '$routeParams', 'loginQueryService', '$rootScope',
        'config'];

        constructor(
            private usuariosService: usuariosService,
            private usuariosQueryService: usuariosQueryService,
            private toasterLite: common.toasterLite,
            private $routeParams: ng.route.IRouteParamsService,
            private loginQueryService: login.loginQueryService,
            private $rootScope: ng.IRootScopeService,
            private config: common.config
        ) {
            this.idUsuario = this.$routeParams['idUsuario'];
            if (this.idUsuario === undefined)
                this.idUsuario = this.loginQueryService.tryGetLocalLoginInfo().usuario;

            this.obtenerOrganizaciones();
        }

        loaded: boolean;
        creandoOrg: boolean;
        agregandoUsuario: boolean;

        idUsuario: string;

        // Nueva organizacion
        orgNombre: string;

        // lista de organizaciones
        organizaciones : organizacionDto[] = [];

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

        //-------------------
        // INTERNAL
        //-------------------

        private obtenerOrganizaciones() {
            this.usuariosQueryService.obtenerOrganizacionesMarcadasDelUsuario(this.idUsuario,
                value =>
                {
                    this.organizaciones = value.data;
                    this.loaded = true;
                },
                reason => this.toasterLite.error('Ocurrió un error al recuperar lista de organizaciones', this.toasterLite.delayForever)
            );
        }
    }
}