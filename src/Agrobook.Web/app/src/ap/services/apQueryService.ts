/// <reference path="../../_all.ts" />

module apArea {
    export class apQueryService extends common.httpLite {
        static $inject = ['$http', 'fakeDb', '$timeout'];

        constructor(
            private $http: ng.IHttpService,
            private fakeDb: fakeDb,
            private $timeout: angular.ITimeoutService
        ) {
            super($http, 'ap/query');
        }

        getClientes(
            filtro: string,
            callback: common.callbackLite<cliente[]>
        ) {
            var filteredList: cliente[];

            if (filtro === "todos")
                filteredList = this.fakeDb.clientes;
            else if (filtro === "prod")
                filteredList = this.fakeDb.clientes.filter(x => x.tipo === "prod");
            else if (filtro === "org")
                filteredList = this.fakeDb.clientes.filter(x => x.tipo === "org");

            callback.onSuccess({
                data: filteredList});
        }

        getOrg(
            id: string,
            callback: common.callbackLite<orgDto>
        ) {
            var dto: orgDto;
            for (var i = 0; i < this.fakeDb.orgs.length; i++) {
                if (this.fakeDb.orgs[i].id === id) {
                    var x = this.fakeDb.orgs[i];
                    dto = new orgDto(x.id, x.display, x.avatarUrl);
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
            var dto: prodDto;
            for (var i = 0; i < this.fakeDb.prods.length; i++) {
                if (this.fakeDb.prods[i].id === id) {
                    var x = this.fakeDb.prods[i];
                    dto = new prodDto(x.id, x.display,x.avatarUrl, x.orgs);
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
            var list = this.fakeDb.servicios.filter(x => x.idOrg === idOrg);

            this.$timeout(() => callback.onSuccess({ data: list }), 750);
        }

        getServiciosPorProd(idProd: string, callback: common.callbackLite<servicioDto[]>) {
            var lista = this.fakeDb.servicios.filter(x => x.idProd === idProd);

            this.$timeout(() => callback.onSuccess({ data: lista }), 750);
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

        getOrgsConContratos(idProd: string, callback: common.callbackLite<orgConContratos[]>) {
            let orgs: orgDto[];
            for (var i = 0; i < this.fakeDb.prods.length; i++) {
                var prod = this.fakeDb.prods[i];
                if (prod.id === idProd) {
                    orgs = prod.orgs;
                    break;
                }
            }

            var list = orgs.map(o => new orgConContratos(o, this.fakeDb.contratos.filter(c => c.idOrg === o.id)));

            callback.onSuccess({ data: list });
        }
    }
}

