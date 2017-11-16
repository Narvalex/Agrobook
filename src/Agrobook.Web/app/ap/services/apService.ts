/// <reference path="../../_all.ts" />

module apArea {
    export class apService extends common.httpLite {
        static $inject = ['$http', 'fakeDb', '$timeout'];

        constructor(
            private $http: ng.IHttpService,
            private fakeDb: fakeDb,
            private timer: angular.ITimeoutService
        ) {
            super($http, '../ap');
        }

        // Parcelas
        registrarNuevaParcela(idProductor: string, nombreDeLaParcela: string, hectareas: number, callback: common.callbackLite<string>) {
            let cmd = {
                idProductor: idProductor,
                nombreDeLaParcela: nombreDeLaParcela,
                hectareas: hectareas
            };

            super.postWithCallback('registrar-parcela', cmd, callback);
        }

        editarParcela(idProductor: string, idParcela: string, nombreDeLaParcela: string, hectareas: number, callback: common.callbackLite<{}>) {
            var cmd = {
                idProductor: idProductor,
                idParcela: idParcela,
                nombre: nombreDeLaParcela,
                hectareas: hectareas
            };

            super.postWithCallback('editar-parcela', cmd, callback);
        }

        eliminarParcela(idProductor: string, idParcela: string, callback: common.callbackLite<{}>) {
            var cmd = {
                idProductor: idProductor,
                idParcela: idParcela
            };

            super.postWithCallback('eliminar-parcela', cmd, callback);
        }

        restaurarParcela(idProductor: string, idParcela: string, callback: common.callbackLite<{}>) {
            var cmd = {
                idProductor: idProductor,
                idParcela: idParcela
            };

            super.postWithCallback('restaurar-parcela', cmd, callback);
        }

        // Contratos
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

        // Servicios
        registrarNuevoServicio(servicio: servicioDto, callback: common.callbackLite<string>) {
            let cmd = {
                idProd: servicio.idProd,
                idOrg: servicio.idOrg,
                idContrato: servicio.idContrato,
                esAdenda: servicio.esAdenda,
                idContratoDeLaAdenda: servicio.idContratoDeLaAdenda,
                fecha: servicio.fecha,
                observaciones: servicio.observaciones
            };
            super.postWithCallback('nuevo-servicio', cmd, callback);
        }

        editarDatosBasicosDelServicio(servicio: servicioDto, callback: common.callbackLite<any>) {
            let cmd = {
                idServicio: servicio.id,
                idOrg: servicio.idOrg,
                idContrato: servicio.idContrato,
                esAdenda: servicio.esAdenda,
                idContratoDeLaAdenda: servicio.idContratoDeLaAdenda,
                fecha: servicio.fecha,
                observaciones: servicio.observaciones
            };
            super.postWithCallback('editar-datos-basicos-del-servicio', cmd, callback);
        }

        eliminarServicio(idServicio: string, callback: common.callbackLite<any>) {
            let cmd = { idServicio: idServicio };
            super.postWithCallback('eliminar-servicio', cmd, callback);
        }

        restaurarServicio(idServicio: string, callback: common.callbackLite<any>) {
            let cmd = { idServicio: idServicio };
            super.postWithCallback('restaurar-servicio', cmd, callback);
        }

        especificarParcelaDelServicio(idServicio: string, parcela: parcelaDto, callback: common.callbackLite<any>) {
            let cmd = {
                idServicio: idServicio,
                idParcela: parcela.id
            };
            super.postWithCallback('especificar-parcela-del-servicio', cmd, callback); 
        }

        cambiarParcelaDelServicio(idServicio: string, parcela: parcelaDto, callback: common.callbackLite<any>) {
            let cmd = {
                idServicio: idServicio,
                idParcela: parcela.id
            };
            super.postWithCallback('cambiar-parcela-del-servicio', cmd, callback);
        }

        fijarPrecio(idServicio: string, precio: number, callback: common.callbackLite<any>) {
            let cmd = {
                idServicio: idServicio,
                precio: precio
            };
            super.postWithCallback('fijar-precio-al-servicio', cmd, callback);
        }

        ajustarPrecio(idServicio: string, precio: number, callback: common.callbackLite<any>) {
            let cmd = {
                idServicio: idServicio,
                precio: precio
            };
            super.postWithCallback('ajustar-precio-del-servicio', cmd, callback);
        }
    }
}