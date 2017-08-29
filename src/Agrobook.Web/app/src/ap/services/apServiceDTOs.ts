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
            public productorDisplay: string,
            public fecha: string,
            public contrato: string
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
            // Cliente preload
            this.clientes = this.orgs.map(x => new cliente(x.id, x.display, 'Organización', 'org', x.avatarUrl))
                .concat(this.prods
                    .map(p => new cliente(p.id, p.display,
                        p.orgs.reduce<string>((des: string, org: orgDto, index: number, array: orgDto[]) => {
                            if (des === '')
                                des = org.display;
                            else
                                des += (', ' + org.display);
                            return des;
                        }, ''),
                        'prod', p.avatarUrl)));
        }

        public clientes: cliente[];
        
        // Se genera en otro Bounded Context
        public orgs = [
            new orgDto('coopchorti', 'Cooperativa Chortizer', './assets/img/avatar/org-icon.png'),
            new orgDto('ccuu', 'Cooperativa Colonias Unidas', './assets/img/avatar/org-icon.png')
        ];

        // Se genera en otro Bounded Context
        public prods = [
            new prodDto('davidelias', 'David Elías', './assets/img/avatar/8.png', [
                new orgDto(this.orgs[0].id, this.orgs[0].display, this.orgs[0].avatarUrl)
            ]),
            new prodDto('kazuoyama', 'Kazuo Yamazuki', './assets/img/avatar/9.png', [
                new orgDto(this.orgs[1].id, this.orgs[1].display, this.orgs[1].avatarUrl)
            ]),
            new prodDto('adair', 'Adair Acosta', './assets/img/avatar/7.png', [
                new orgDto(this.orgs[0].id, this.orgs[0].display, this.orgs[0].avatarUrl),
                new orgDto(this.orgs[1].id, this.orgs[1].display, this.orgs[1].avatarUrl)
            ])
        ];

        //public servicios = [
        //    new servicioDto("David Elias", "20/12/2018", "Contrato Chorti"),
        //    new servicioDto("Kazuo Yamazuki", "20/12/2017", "Contrato Pirapo")
        //];

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