/// <reference path="../../_all.ts" />

module apArea {
    export class apService extends common.httpLite {
        static $inject = ['$http', 'fakeDb', '$timeout'];

        constructor(
            private $http: ng.IHttpService,
            private fakeDb: fakeDb,
            private timer: angular.ITimeoutService
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

        registrarNuevoServicio(servicio: servicioDto, callback: common.callbackLite<string>) {
            var serviciosDelProductor = this.fakeDb.servicios.filter(x => x.idProd === servicio.idProd);
            var idQueLeSigue = serviciosDelProductor.length + 1;
            servicio.id = `${servicio.idProd}_servicio${idQueLeSigue}`;

            this.fakeDb.servicios.push(servicio);

            setTimeout(() => {
                callback.onSuccess({ data: servicio.id });
            }, 2000);
        }

        actualizarServicio(servicio: servicioDto, callback: common.callbackLite<any>) {
            for (var i = 0; i < this.fakeDb.servicios.length; i++) {
                let recuperado = this.fakeDb.servicios[i];
                if (recuperado.id === servicio.id) {
                    this.fakeDb.servicios.splice(i, 1);
                    this.fakeDb.servicios.push(servicio);
                    break;
                }
            }

            this.timer(() => callback.onSuccess({}), 500);
        }

        eliminarServicio(idServicio: string, callback: common.callbackLite<any>) {
            for (var i = 0; i < this.fakeDb.servicios.length; i++) {
                if (this.fakeDb.servicios[i].id === idServicio) {
                    this.fakeDb.servicios[i].eliminado = true;
                    break;
                }
            }

            this.timer(() => callback.onSuccess({ data: {} }), 500);
        }

        restaurarServicio(idServicio: string, callback: common.callbackLite<any>) {
            for (var i = 0; i < this.fakeDb.servicios.length; i++) {
                if (this.fakeDb.servicios[i].id === idServicio) {
                    this.fakeDb.servicios[i].eliminado = false;
                    break;
                }
            }

            this.timer(() => callback.onSuccess({ data: {} }), 500);
        }
    }
}
