/// <reference path="../../_all.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var apArea;
(function (apArea) {
    var apService = (function (_super) {
        __extends(apService, _super);
        function apService($http) {
            var _this = _super.call(this, $http, '../ap') || this;
            _this.$http = $http;
            return _this;
        }
        // Parcelas
        apService.prototype.registrarNuevaParcela = function (idProductor, nombreDeLaParcela, hectareas, callback) {
            var cmd = {
                idProductor: idProductor,
                nombreDeLaParcela: nombreDeLaParcela,
                hectareas: hectareas
            };
            _super.prototype.postWithCallback.call(this, 'registrar-parcela', cmd, callback);
        };
        apService.prototype.editarParcela = function (idProductor, idParcela, nombreDeLaParcela, hectareas, callback) {
            var cmd = {
                idProductor: idProductor,
                idParcela: idParcela,
                nombre: nombreDeLaParcela,
                hectareas: hectareas
            };
            _super.prototype.postWithCallback.call(this, 'editar-parcela', cmd, callback);
        };
        apService.prototype.eliminarParcela = function (idProductor, idParcela, callback) {
            var cmd = {
                idProductor: idProductor,
                idParcela: idParcela
            };
            _super.prototype.postWithCallback.call(this, 'eliminar-parcela', cmd, callback);
        };
        apService.prototype.restaurarParcela = function (idProductor, idParcela, callback) {
            var cmd = {
                idProductor: idProductor,
                idParcela: idParcela
            };
            _super.prototype.postWithCallback.call(this, 'restaurar-parcela', cmd, callback);
        };
        // Contratos
        apService.prototype.registrarNuevoContrato = function (contrato, callback) {
            if (contrato.esAdenda) {
                _super.prototype.postWithCallback.call(this, 'registrar-adenda', { IdContrato: contrato.idContratoDeLaAdenda, NombreDeLaAdenda: contrato.display, Fecha: contrato.fecha }, callback);
            }
            else {
                _super.prototype.postWithCallback.call(this, 'registrar-contrato', { IdOrganizacion: contrato.idOrg, NombreDelContrato: contrato.display, fecha: contrato.fecha }, callback);
            }
        };
        apService.prototype.editarContrato = function (contrato, callback) {
            if (contrato.esAdenda) {
                var cmd = {
                    idContrato: contrato.idContratoDeLaAdenda,
                    idAdenda: contrato.id,
                    nombreDeLaAdenda: contrato.display,
                    fecha: contrato.fecha
                };
                _super.prototype.postWithCallback.call(this, 'editar-adenda', cmd, callback);
            }
            else {
                var cmd = {
                    idContrato: contrato.id,
                    nombreDelContrato: contrato.display,
                    fecha: contrato.fecha
                };
                _super.prototype.postWithCallback.call(this, 'editar-contrato', cmd, callback);
            }
        };
        apService.prototype.eliminarContrato = function (idContrato, callback) {
            _super.prototype.postWithCallback.call(this, 'eliminar-contrato/' + idContrato, {}, callback);
        };
        apService.prototype.eliminarAdenda = function (idContrato, idAdenda, callback) {
            _super.prototype.postWithCallback.call(this, 'eliminar-adenda?idContrato=' + idContrato + '&idAdenda=' + idAdenda, {}, callback);
        };
        apService.prototype.restaurarAdenda = function (idContrato, idAdenda, callback) {
            _super.prototype.postWithCallback.call(this, 'restaurar-adenda?idContrato=' + idContrato + '&idAdenda=' + idAdenda, {}, callback);
        };
        apService.prototype.restaurarContrato = function (idContrato, callback) {
            _super.prototype.postWithCallback.call(this, 'restaurar-contrato/' + idContrato, {}, callback);
        };
        // Servicios
        apService.prototype.registrarNuevoServicio = function (servicio, callback) {
            var cmd = {
                idProd: servicio.idProd,
                idOrg: servicio.idOrg,
                idContrato: servicio.idContrato,
                esAdenda: servicio.esAdenda,
                idContratoDeLaAdenda: servicio.idContratoDeLaAdenda,
                fecha: servicio.fecha,
                observaciones: servicio.observaciones
            };
            _super.prototype.postWithCallback.call(this, 'nuevo-servicio', cmd, callback);
        };
        apService.prototype.editarDatosBasicosDelServicio = function (servicio, callback) {
            var cmd = {
                idServicio: servicio.id,
                idOrg: servicio.idOrg,
                idContrato: servicio.idContrato,
                esAdenda: servicio.esAdenda,
                idContratoDeLaAdenda: servicio.idContratoDeLaAdenda,
                fecha: servicio.fecha,
                observaciones: servicio.observaciones
            };
            _super.prototype.postWithCallback.call(this, 'editar-datos-basicos-del-servicio', cmd, callback);
        };
        apService.prototype.eliminarServicio = function (idServicio, callback) {
            var cmd = { idServicio: idServicio };
            _super.prototype.postWithCallback.call(this, 'eliminar-servicio', cmd, callback);
        };
        apService.prototype.restaurarServicio = function (idServicio, callback) {
            var cmd = { idServicio: idServicio };
            _super.prototype.postWithCallback.call(this, 'restaurar-servicio', cmd, callback);
        };
        apService.prototype.especificarParcelaDelServicio = function (idServicio, parcela, callback) {
            var cmd = {
                idServicio: idServicio,
                idParcela: parcela.id
            };
            _super.prototype.postWithCallback.call(this, 'especificar-parcela-del-servicio', cmd, callback);
        };
        apService.prototype.cambiarParcelaDelServicio = function (idServicio, parcela, callback) {
            var cmd = {
                idServicio: idServicio,
                idParcela: parcela.id
            };
            _super.prototype.postWithCallback.call(this, 'cambiar-parcela-del-servicio', cmd, callback);
        };
        apService.prototype.fijarPrecio = function (idServicio, precio, callback) {
            var cmd = {
                idServicio: idServicio,
                precio: precio
            };
            _super.prototype.postWithCallback.call(this, 'fijar-precio-al-servicio', cmd, callback);
        };
        apService.prototype.ajustarPrecio = function (idServicio, precio, callback) {
            var cmd = {
                idServicio: idServicio,
                precio: precio
            };
            _super.prototype.postWithCallback.call(this, 'ajustar-precio-del-servicio', cmd, callback);
        };
        return apService;
    }(common.httpLite));
    apService.$inject = ['$http'];
    apArea.apService = apService;
})(apArea || (apArea = {}));
//# sourceMappingURL=apService.js.map