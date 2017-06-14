/// <reference path="../../_all.ts" />

module usuariosArea {
    export class organizacionesController {
        static $inject = ['usuariosService', 'usuariosQueryService', 'toasterLite'];

        constructor(
            private usuariosService: usuariosService,
            private usuariosQueryService: usuariosQueryService,
            private toasterLite: common.toasterLite
        ) {
            this.obtenerOrganizaciones();
        }

        loaded: boolean;

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