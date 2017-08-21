/// <reference path="../../_all.ts" />

module apArea {
    export class apQueryService extends common.httpLite {
        static $inject = ['$http', 'fakeDb'];

        constructor(
            private $http: ng.IHttpService,
            private fakeDb: fakeDb
        ) {
            super($http, 'ap/query');
        }

        getClientes(
            filtro: string,
            callback: common.callbackLite<cliente[]>
        ) {
            var filteredList: cliente[];

            if (filtro === "todos")
                filteredList = this.fakeDb.fakeClientesList;
            else if (filtro === "prod")
                filteredList = this.fakeDb.fakeClientesList.filter(x => x.tipo === "prod");
            else if (filtro === "org")
                filteredList = this.fakeDb.fakeClientesList.filter(x => x.tipo === "org");

            callback.onSuccess({
                data: filteredList});
        }

        getOrg(
            id: string,
            callback: common.callbackLite<orgDto>
        ) {
            var dto: orgDto;
            for (var i = 0; i < this.fakeDb.fakeClientesList.length; i++) {
                if (this.fakeDb.fakeClientesList[i].id === id) {
                    var x = this.fakeDb.fakeClientesList[i];
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
            for (var i = 0; i < this.fakeDb.fakeClientesList.length; i++) {
                if (this.fakeDb.fakeClientesList[i].id === id) {
                    var x = this.fakeDb.fakeClientesList[i];
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
                data: this.fakeDb.fakeServiciosList
            });
        }

        getParcelasDelProd(
            idProd: string,
            callback: common.callbackLite<parcelaDto[]>
        ) {
            var list = this.fakeDb.parcelas.filter(x => x.idProd === idProd);

            callback.onSuccess({
                data: list
            });
        }

        getContratos(
            idOrg: string,
            callback: common.callbackLite<contratoDto[]>
        ) {
            var list = this.fakeDb.contratos.filter(x => x.idOrg === idOrg);

            callback.onSuccess({
                data: list
            });
        }
    }
}

