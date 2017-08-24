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
            public display: string
        ) {
        }
    }

    export class servicioDto {
        constructor(
            public productorDisplay: string,
            public fecha: string,
            public contrato: string
        ) {
        }
    }

    export class prodDto {
        constructor(
            public id: string,
            public display: string
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

    //-----------------------------------------
    // FakeDb
    //-----------------------------------------
    export class fakeDb {
        static $inject = [];

        constructor(
        ) {    
        }

        public fakeClientesList = [
            new cliente("coopchorti", "Cooperativa Chortizer", "Loma Plata", "org", "./assets/img/avatar/org-icon.png"),
            new cliente("ccuu", "Cooperativa Colonias Unidas", "Itapua", "org", "./assets/img/avatar/org-icon.png"),
            new cliente("davidelias", "David Elias", "Cooperativa Chortizer", "prod", "./assets/img/avatar/8.png"),
            new cliente("kazuoyama", "Kazuo Yamazuki", "Cooperativa Pirapo", "prod", "./assets/img/avatar/9.png"),
            new cliente("adair", "Adair Acosta", "Cooperativa Raul Peña", "prod", "./assets/img/avatar/7.png")
        ];

        public fakeServiciosList = [
            new servicioDto("David Elias", "20/12/2018", "Contrato Chorti"),
            new servicioDto("Kazuo Yamazuki", "20/12/2017", "Contrato Pirapo")
        ];

        // El id de la parcela se puede hacer de la combinacion [prod]_[nombreParcela] debido a que el prod es 
        // igual al usuario, que es unico
        public parcelas: parcelaDto[] = [
            new parcelaDto('davidelias_DeLaSeñora', 'davidelias', 'De la Señora', '31,66'),
            new parcelaDto('davidelias_Apepu', 'davidelias', 'Apepu', '72,18'),
            new parcelaDto('kazuoyama_Mariscal', 'kazuoyama', 'Mariscal', '73,18'),
            new parcelaDto('kazuoyama_Feliciano', 'kazuoyama', 'Feliciano', '75,18')
        ];

        // Id contrato: [idOrg]_[nombre_contrato]
        // Id adenda: [idContrato]_[nombre_adenda]
        public contratos: contratoDto[] = [
            new contratoDto('coopchorti_Contrato Chorti', 'coopchorti', 'Contrato Chorti', false, false, null, new Date(2017, 1, 17)),
            new contratoDto('coopchorti_Contrato Chorti_Adenda I', 'coopchorti', 'Adenda I', true, false, 'Contrato_Chorti', new Date(2017, 2, 20)) 
        ];
    }
}