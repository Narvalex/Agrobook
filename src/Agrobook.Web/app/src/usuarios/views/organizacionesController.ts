/// <reference path="../../_all.ts" />

module usuariosArea {
    export class organizacionesController {
        static $inject = ['usuariosService', 'usuariosQueryService', 'toasterLite', '$routeParams'];

        constructor(
            private usuariosService: usuariosService,
            private usuariosQueryService: usuariosQueryService,
            private toasterLite: common.toasterLite,
            private $routeParams: ng.route.IRouteParamsService
        ) {
            this.idUsuario = this.$routeParams['idUsuario'];
            this.obtenerOrganizaciones();
        }

        loaded: boolean;

        idUsuario: string;

        // Nueva organizacion
        orgNombre: string;

        // lista de organizaciones
        organizaciones : organizacionDto[] = [];

        crearNuevaOrganizacion() {
            this.usuariosService.crearNuevaOrganizacion(this.orgNombre,
                value => this.toasterLite.success("La organización " + this.orgNombre + " fue creada exitosamente"),
                reason => this.toasterLite.error('Ocurrió un error inesperado al intentar crear la organización ' + this.orgNombre)
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
            this.usuariosQueryService.obtenerOrganizaciones(
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