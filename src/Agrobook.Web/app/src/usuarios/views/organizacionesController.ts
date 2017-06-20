/// <reference path="../../_all.ts" />

module usuariosArea {
    export class organizacionesController {
        static $inject = ['usuariosService', 'usuariosQueryService', 'toasterLite', '$routeParams', 'loginQueryService'];

        constructor(
            private usuariosService: usuariosService,
            private usuariosQueryService: usuariosQueryService,
            private toasterLite: common.toasterLite,
            private $routeParams: ng.route.IRouteParamsService,
            private loginQueryService: login.loginQueryService
        ) {
            this.idUsuario = this.$routeParams['idUsuario'];
            if (this.idUsuario === undefined)
                this.idUsuario = this.loginQueryService.tryGetLocalLoginInfo().usuario;

            this.obtenerOrganizaciones();
        }

        loaded: boolean;
        creandoOrg: boolean;

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
            this.usuariosService.agregarUsuarioALaOrganizacion(this.idUsuario, org.id,
                value => {
                    this.toasterLite.success(`Usuario agregado exitosamente a ${org.display}`);
                },
                reason => this.toasterLite.error('Hubo un error al incorporar el usuario a la organizacion', this.toasterLite.delayForever)
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