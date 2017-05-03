/// <reference path="../../_all.ts" />

module usuariosArea {
    export class organizacionesController {
        static $inject = ['usuariosService', 'toasterLite'];

        constructor(
            private usuariosService: usuariosService,
            private toasterLite: common.toasterLite
        ) {
           //...
        }

        orgNombre: string;

        crearNuevaOrganizacion() {
            this.usuariosService.crearNuevaOrganizacion(this.orgNombre,
                value => this.toasterLite.success("La organización " + this.orgNombre + " fue creada exitosamente"),
                reason => this.toasterLite.error('Ocurrió un error inesperado al intentar crear la organización ' + this.orgNombre)
            );
        }
    }
}