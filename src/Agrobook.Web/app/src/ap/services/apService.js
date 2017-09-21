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
        function apService($http, fakeDb, timer) {
            var _this = _super.call(this, $http, 'ap') || this;
            _this.$http = $http;
            _this.fakeDb = fakeDb;
            _this.timer = timer;
            return _this;
        }
        apService.prototype.registrarNuevaParcela = function (dto, callback) {
            var cmd = {
                idProductor: dto.idProd,
                nombreDeLaParcela: dto.display,
                hectareas: dto.hectareas
            };
            _super.prototype.postWithCallback.call(this, 'registrar-parcela', cmd, callback);
        };
        apService.prototype.editarParcela = function (dto, callback) {
            var cmd = {
                idProductor: dto.idProd,
                idParcela: dto.idParcela,
                nombre: dto.display,
                hectareas: dto.hectareas
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
        apService.prototype.registrarNuevoServicio = function (servicio, callback) {
            var serviciosDelProductor = this.fakeDb.servicios.filter(function (x) { return x.idProd === servicio.idProd; });
            var idQueLeSigue = serviciosDelProductor.length + 1;
            servicio.id = servicio.idProd + "_servicio" + idQueLeSigue;
            this.fakeDb.servicios.push(servicio);
            setTimeout(function () {
                callback.onSuccess({ data: servicio.id });
            }, 2000);
        };
        apService.prototype.actualizarServicio = function (servicio, callback) {
            for (var i = 0; i < this.fakeDb.servicios.length; i++) {
                var recuperado = this.fakeDb.servicios[i];
                if (recuperado.id === servicio.id) {
                    this.fakeDb.servicios.splice(i, 1);
                    this.fakeDb.servicios.push(servicio);
                    break;
                }
            }
            this.timer(function () { return callback.onSuccess({}); }, 500);
        };
        apService.prototype.eliminarServicio = function (idServicio, callback) {
            for (var i = 0; i < this.fakeDb.servicios.length; i++) {
                if (this.fakeDb.servicios[i].id === idServicio) {
                    this.fakeDb.servicios[i].eliminado = true;
                    break;
                }
            }
            this.timer(function () { return callback.onSuccess({ data: {} }); }, 500);
        };
        apService.prototype.restaurarServicio = function (idServicio, callback) {
            for (var i = 0; i < this.fakeDb.servicios.length; i++) {
                if (this.fakeDb.servicios[i].id === idServicio) {
                    this.fakeDb.servicios[i].eliminado = false;
                    break;
                }
            }
            this.timer(function () { return callback.onSuccess({ data: {} }); }, 500);
        };
        apService.prototype.especificarParcelaDelServicio = function (idServicio, parcela, callback) {
            for (var i = 0; i < this.fakeDb.servicios.length; i++) {
                var servicio = this.fakeDb.servicios[i];
                if (servicio.id === idServicio) {
                    this.fakeDb.servicios[i].parcelaId = parcela.id;
                    this.fakeDb.servicios[i].parcelaDisplay = parcela.display;
                    break;
                }
            }
            this.timer(function () { return callback.onSuccess({}); }, 500);
        };
        apService.prototype.cambiarParcelaDelServicio = function (idServicio, parcela, callback) {
            for (var i = 0; i < this.fakeDb.servicios.length; i++) {
                var servicio = this.fakeDb.servicios[i];
                if (servicio.id === idServicio) {
                    this.fakeDb.servicios[i].parcelaId = parcela.id;
                    this.fakeDb.servicios[i].parcelaDisplay = parcela.display;
                    break;
                }
            }
            this.timer(function () { return callback.onSuccess({}); }, 500);
        };
        return apService;
    }(common.httpLite));
    apService.$inject = ['$http', 'fakeDb', '$timeout'];
    apArea.apService = apService;
})(apArea || (apArea = {}));
//# sourceMappingURL=apService.js.map