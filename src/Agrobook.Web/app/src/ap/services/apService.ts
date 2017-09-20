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

        registrarNuevoContrato(contrato: contratoDto, callback: common.callbackLite<string>) {
            if (contrato.esAdenda) {
                super.postWithCallback('registrar-adenda', { IdContrato: contrato.idContratoDeLaAdenda, NombreDeLaAdenda: contrato.display, Fecha: contrato.fecha }, callback);
            }
            else {
                super.postWithCallback('registrar-contrato', { IdOrganizacion: contrato.idOrg, NombreDelContrato: contrato.display, fecha: contrato.fecha }, callback);
            }
        }

        editarContrato(contrato: contratoDto, callback: common.callbackLite<{}>) {
            if (contrato.esAdenda) {
                let cmd = {
                    idContrato: contrato.idContratoDeLaAdenda,
                    idAdenda: contrato.id,
                    nombreDeLaAdenda: contrato.display,
                    fecha: contrato.fecha
                };
                super.postWithCallback('editar-adenda', cmd, callback);
            }
            else {
                let cmd = {
                    idContrato: contrato.id,
                    nombreDelContrato: contrato.display,
                    fecha: contrato.fecha
                };
                super.postWithCallback('editar-contrato', cmd, callback);
            }
        }

        eliminarContrato(idContrato: string, callback: common.callbackLite<{}>) {
            super.postWithCallback('eliminar-contrato/' + idContrato, {}, callback); 
        }

        eliminarAdenda(idContrato: string, idAdenda: string, callback: common.callbackLite<{}>) {
            super.postWithCallback('eliminar-adenda?idContrato=' + idContrato + '&idAdenda=' + idAdenda, {}, callback);
        }

        restaurarAdenda(idContrato: string, idAdenda: string, callback: common.callbackLite<{}>) {
            super.postWithCallback('restaurar-adenda?idContrato=' + idContrato + '&idAdenda=' + idAdenda, {}, callback);
        }

        restaurarContrato(idContrato: string, callback: common.callbackLite<{}>) {
            super.postWithCallback('restaurar-contrato/' + idContrato, {}, callback); 
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

        especificarParcelaDelServicio(idServicio: string, parcela: parcelaDto, callback: common.callbackLite<any>) {
            for (var i = 0; i < this.fakeDb.servicios.length; i++) {
                let servicio = this.fakeDb.servicios[i];
                if (servicio.id === idServicio) {
                    this.fakeDb.servicios[i].parcelaId = parcela.id;
                    this.fakeDb.servicios[i].parcelaDisplay = parcela.display;
                    break;
                }
            }

            this.timer(() => callback.onSuccess({}), 500);
        }

        cambiarParcelaDelServicio(idServicio: string, parcela: parcelaDto, callback: common.callbackLite<any>) {
            for (var i = 0; i < this.fakeDb.servicios.length; i++) {
                let servicio = this.fakeDb.servicios[i];
                if (servicio.id === idServicio) {
                    this.fakeDb.servicios[i].parcelaId = parcela.id;
                    this.fakeDb.servicios[i].parcelaDisplay = parcela.display;
                    break;
                }
            }

            this.timer(() => callback.onSuccess({}), 500);
        }
    }
}
