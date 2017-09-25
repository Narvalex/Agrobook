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
            callback: common.callbackLite<string>
        ) {
            let cmd = {
                idProductor: dto.idProd,
                nombreDeLaParcela: dto.display,
                hectareas: dto.hectareas
            };

            super.postWithCallback('registrar-parcela', cmd, callback);
        }

        editarParcela(
            dto: edicionParcelaDto,
            callback: common.callbackLite<{}>
        ) {
            var cmd = {
                idProductor: dto.idProd,
                idParcela: dto.idParcela,
                nombre: dto.display,
                hectareas: dto.hectareas
            };

            super.postWithCallback('editar-parcela', cmd, callback);
        }

        eliminarParcela(
            idProductor: string,
            idParcela: string,
            callback: common.callbackLite<{}>
        ) {
            var cmd = {
                idProductor: idProductor,
                idParcela: idParcela
            };

            super.postWithCallback('eliminar-parcela', cmd, callback);
        }

        restaurarParcela(
            idProductor: string,
            idParcela: string,
            callback: common.callbackLite<{}>
        ) {
            var cmd = {
                idProductor: idProductor,
                idParcela: idParcela
            };

            super.postWithCallback('restaurar-parcela', cmd, callback);
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
            let cmd = {
                idProd: servicio.idProd,
                idOrg: servicio.idOrg,
                idContrato: servicio.idContrato,
                fecha: servicio.fecha
            };
            super.postWithCallback('nuevo-servicio', cmd, callback);
        }

        actualizarServicio(servicio: servicioDto, callback: common.callbackLite<any>) {
            let cmd = {
                idServicio: servicio.id,
                idOrg: servicio.idOrg,
                idContrato: servicio.idContrato,
                fecha: servicio.fecha,
            };
            super.postWithCallback('editar-servicio', cmd, callback);
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
