/// <reference path="../../_all.ts" />

module apArea {
    export class apService extends common.httpLite {
        static $inject = ['$http', 'fakeDb'];

        constructor(
            private $http: ng.IHttpService,
            private fakeDb: fakeDb
        ) {
            super($http, 'ap');
        }

        registrarNuevaParcela(
            dto: edicionParcelaDto,
            callback: common.callbackLite<parcelaDto>
        ) {
            var data = new parcelaDto(dto.idProd + '_' + dto.display.trim(), dto.idProd, dto.display, dto.hectareas);

            for (var i = 0; i < this.fakeDb.parcelas.length; i++) {
                if (this.fakeDb.parcelas[i].id === data.id) {
                    callback.onError(null);
                    return;
                }
            }

            this.fakeDb.parcelas.push(data);
                 
            callback.onSuccess({
                data: data
            });
        }

        editarParcela(
            dto: edicionParcelaDto,
            callback: common.callbackLite<{}>
        ) {
            for (var i = 0; i < this.fakeDb.parcelas.length; i++) {
                if (this.fakeDb.parcelas[i].id === dto.idParcela) {
                    this.fakeDb.parcelas[i].display = dto.display;
                    this.fakeDb.parcelas[i].hectareas = dto.hectareas;
                    break;
                }
            }

            callback.onSuccess({
                data: {}
            });
        }

        eliminar(
            idParcela: string,
            callback: common.callbackLite<{}>
        ) {
            for (var i = 0; i < this.fakeDb.parcelas.length; i++) {
                if (this.fakeDb.parcelas[i].id === idParcela) {
                    this.fakeDb.parcelas[i].eliminado = true;
                    break;
                }
            }

            callback.onSuccess({
                data: {}
            });
        }

        restaurar(
            idParcela: string,
            callback: common.callbackLite<{}>
        ) {
            for (var i = 0; i < this.fakeDb.parcelas.length; i++) {
                if (this.fakeDb.parcelas[i].id === idParcela) {
                    this.fakeDb.parcelas[i].eliminado = false;
                    break;
                }
            }

            callback.onSuccess({
                data: {}
            });
        }
    }
}
