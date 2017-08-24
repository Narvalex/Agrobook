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

            // Set validation check
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

            callback.onSuccess({});
        }

        eliminarParcela(
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

        restaurarParcela(
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

        registrarNuevoContrato(contrato: contratoDto, callback: common.callbackLite<contratoDto>) {
            let id: string;
            if (contrato.esAdenda) {
                id = `${contrato.idContratoDeLaAdenda}_${contrato.display.trim()}`;
            }
            else {
                id = `${contrato.idOrg}_${contrato.display.trim()}`;
            }

            // Primary key check
            for (var i = 0; i < this.fakeDb.contratos.length; i++) {
                if (this.fakeDb.contratos[i].id === id) {
                    callback.onError(null);
                    return;
                }
            }

            contrato.id = id;
            this.fakeDb.contratos.push(contrato);

            callback.onSuccess({
                data: contrato
            });
        }

        editarContrato(contrato: contratoDto, callback: common.callbackLite<{}>) {
            for (var i = 0; i < this.fakeDb.contratos.length; i++) {
                if (this.fakeDb.contratos[i].id === contrato.id) {
                    this.fakeDb.contratos.splice(i, 1);
                    this.fakeDb.contratos.push(contrato);
                    break;
                }
            }

            callback.onSuccess({});
        }

        eliminarContrato(idContrato: string, callback: common.callbackLite<{}>) {
            for (var i = 0; i < this.fakeDb.contratos.length; i++) {
                if (this.fakeDb.contratos[i].id === idContrato) {
                    this.fakeDb.contratos[i].eliminado = true;
                    break;
                }
            }

            callback.onSuccess({
                data: {}
            });
        }

        restaurarContrato(idContrato: string, callback: common.callbackLite<{}>) {
            for (var i = 0; i < this.fakeDb.contratos.length; i++) {
                if (this.fakeDb.contratos[i].id === idContrato) {
                    this.fakeDb.contratos[i].eliminado = false;
                    break;
                }
            }

            callback.onSuccess({
                data: {}
            });
        }
    }
}
