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
            super.getWithCallback('clientes?filtro=' + filtro, callback);
        }

        getOrg(
            id: string,
            callback: common.callbackLite<orgDto>
        ) {
            super.getWithCallback('org/' + id, callback);
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

        getServicio(idServicio: string, callback: common.callbackLite<servicioDto>) {
            let servicio: servicioDto;
            for (var i = 0; i < this.fakeDb.servicios.length; i++) {
                servicio = this.fakeDb.servicios[i];
                if (servicio.id === idServicio)
                    break;
            }

            this.$timeout(() => callback.onSuccess({ data: servicio }), 500);
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

        getParcela(
            idParcela: string,
            callback: common.callbackLite<parcelaDto>
        ) {
            var dto = this.fakeDb.parcelas.filter(x => x.id === idParcela);

            this.$timeout(() =>
            callback.onSuccess({
                data: dto[0]
            }), 500);
        }

        getContratos(
            idOrg: string,
            callback: common.callbackLite<contratoDto[]>
        ) {
            super.getWithCallback('contratos/' + idOrg, callback);
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

            this.$timeout(() => callback.onSuccess({ data: list }), 500);
        }
    }
}

