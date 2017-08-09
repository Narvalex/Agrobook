/// <reference path="../../_all.ts" />

module apArea {
    export class apQueryService extends common.httpLite {
        static $inject = ['$http'];

        constructor(
            private $http: ng.IHttpService,
            private $q: ng.IQService
        ) {
            super($http, 'ap/query');
        }

        getClientes(
            filtro: string,
            callback: common.callbackLite<cliente[]>
        ) {
            var filteredList: cliente[];

            if (filtro === "todos")
                filteredList = this.fakeClientesList;
            else if (filtro === "prod")
                filteredList = this.fakeClientesList.filter(x => x.tipo === "prod");
            else if (filtro === "org")
                filteredList = this.fakeClientesList.filter(x => x.tipo === "org");

            callback.onSuccess({
                data: filteredList});
        }

        getOrg(
            id: string,
            callback: common.callbackLite<orgDto>
        ) {
            var dto: orgDto;
            for (var i = 0; i < this.fakeClientesList.length; i++) {
                if (this.fakeClientesList[i].id === id) {
                    var x = this.fakeClientesList[i];
                    dto = new orgDto(x.id, x.nombre);
                    break;
                }
            }

            callback.onSuccess({
                data: dto
            });
        }

        getProd(
            id: string,
            callback: common.callbackLite<prodDto>
        ) {
            var dto: orgDto;
            for (var i = 0; i < this.fakeClientesList.length; i++) {
                if (this.fakeClientesList[i].id === id) {
                    var x = this.fakeClientesList[i];
                    dto = new prodDto(x.id, x.nombre);
                    break;
                }
            }

            callback.onSuccess({
                data: dto
            });
        }

        getServiciosPorOrg(
            idOrg: string,
            callback: common.callbackLite<servicioDto[]>
        ) {
            callback.onSuccess({
                data: this.fakeServiciosList
            });
        }

        //--------------------------------
        // Fakes
        //--------------------------------

        private fakeClientesList = [
            new cliente("coopchorti", "Cooperativa Chortizer", "Loma Plata", "org", "./assets/img/avatar/org-icon.png"),
            new cliente("ccuu", "Cooperativa Colonias Unidas", "Itapua", "org", "./assets/img/avatar/org-icon.png"),
            new cliente("davidelias", "David Elias", "Cooperativa Chortizer", "prod", "./assets/img/avatar/8.png"),
            new cliente("kazuoyama", "Kazuo Yamazuki", "Cooperativa Pirapo", "prod", "./assets/img/avatar/9.png")
        ];

        private fakeServiciosList = [
            new servicioDto("David Elias", "20/12/2018", "Contrato Chorti"),
            new servicioDto("Kazuo Yamazuki", "20/12/2017", "Contrato Pirapo")
        ];
    }

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
}

