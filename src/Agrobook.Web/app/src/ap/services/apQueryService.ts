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
            var list = [
                new cliente("coopchorti", "Cooperativa Chortizer", "Loma Plata", "org", "./assets/img/avatar/6.png"),
                new cliente("ccuu", "Cooperativa Colonias Unidas", "Itapua", "org", "./assets/img/avatar/7.png"),
                new cliente("davidelias", "David Elias", "Cooperativa Chortizer", "prod", "./assets/img/avatar/8.png"),
                new cliente("kazuoyama", "Kazuo Yamazuki", "Cooperativa Pirapo", "prod", "./assets/img/avatar/9.png")
            ];

            if (filtro === "todos")
                filteredList = list;
            else if (filtro === "prod")
                filteredList = list.filter(x => x.tipo === "prod");
            else if (filtro === "org")
                filteredList = list.filter(x => x.tipo === "org");

            callback.onSuccess({
                data: filteredList});
        }
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
}

