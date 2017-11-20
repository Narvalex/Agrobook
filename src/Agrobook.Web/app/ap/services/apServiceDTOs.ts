/// <reference path="../../_all.ts" />

module apArea {
    //---------------------
    // DTOS
    //---------------------
    export class cliente {
        constructor(
            public id: string, // coopchorti / davidelias
            public nombre: string, // Cooperativa Chortizer / David Elias
            public desc: string, // Loma Plata / Productor de Chooperativa Chortizer y Colonias Unidas
            public tipo: string, // org / prod
            public avatarUrl: string
        ) {
        }
    }

    export class orgDto {
        constructor(
            public id: string,
            public display: string,
            public avatarUrl: string
        ) {
        }
    }

    export class servicioDto {
        constructor(
            public id: string,
            public idContrato: string,
            public esAdenda: boolean,
            public idContratoDeLaAdenda: string,
            public contratoDisplay: string,
            public idOrg: string,
            public orgDisplay: string,
            public idProd: string,
            public prodDislplay: string,
            public fecha: Date,
            public observaciones: string,
            // With Defaults
            public eliminado: boolean = false,
            // Parcela
            public parcelaId: string = null,
            public parcelaDisplay: string = null,
            public hectareas: string = null,
            // Precio
            public tienePrecio: boolean = false,
            public precioTotal: string = null,
            public precioPorHectarea: string = null
        ) {
        }
    }

    export class prodDto {
        constructor(
            public id: string,
            public display: string,
            public avatarUrl: string,
            public orgs: orgDto[]
        ) {
        }
    }

    export class orgConContratos {
        constructor(
            public org: orgDto,
            public contratos: contratoDto[]
        ) {
        }
    }

    export class servicioParaDashboardDto {
        constructor(
            public id: string,
            public idProd: string,
            public prodDisplay: string,
            public prodAvatarUrl: string,
            public orgDisplay: string,
            public parcelaDisplay: string,
            public fecha: string
        ) {
        }
    }

    /************************
     * Parcela
     ************************/

    export class edicionParcelaDto {
        public idProd: string;
        public display: string;
        public hectareas: string;
        public idParcela: string; // solo en edicion tiene sentido
    }

    export class parcelaDto {
        constructor(
            public id: string,
            public idProd: string,
            public display: string,
            public hectareas: string,
            public idDepartamento: string,
            public departamentoDisplay: string,
            public idDistrito: string, 
            public distritoDisplay: string,
            public eliminado: boolean = false
        ) {
        }
    }

    /************************
    * Contrato
    ************************/

    export class contratoDto {
        constructor(
            public id: string,
            public idOrg: string,
            public display: string,
            public esAdenda: boolean,
            public eliminado: boolean,
            public idContratoDeLaAdenda: string,
            public fecha: Date
        ) {
        }
    }

    /************************
    * Distritos
    ************************/

    export class departamento {
        constructor(
            public id: string,
            public display: string,
            public distritos: distrito[]
        ) {
        }
    }

    export class distrito {
        constructor(
            public id: string,
            public display: string
        ) {
        }
    }
}