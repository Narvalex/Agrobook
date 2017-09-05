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
            var data = new apArea.parcelaDto(dto.idProd + '_' + dto.display.trim(), dto.idProd, dto.display, dto.hectareas);
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
        };
        apService.prototype.editarParcela = function (dto, callback) {
            for (var i = 0; i < this.fakeDb.parcelas.length; i++) {
                if (this.fakeDb.parcelas[i].id === dto.idParcela) {
                    this.fakeDb.parcelas[i].display = dto.display;
                    this.fakeDb.parcelas[i].hectareas = dto.hectareas;
                    break;
                }
            }
            callback.onSuccess({});
        };
        apService.prototype.eliminarParcela = function (idParcela, callback) {
            for (var i = 0; i < this.fakeDb.parcelas.length; i++) {
                if (this.fakeDb.parcelas[i].id === idParcela) {
                    this.fakeDb.parcelas[i].eliminado = true;
                    break;
                }
            }
            callback.onSuccess({
                data: {}
            });
        };
        apService.prototype.restaurarParcela = function (idParcela, callback) {
            for (var i = 0; i < this.fakeDb.parcelas.length; i++) {
                if (this.fakeDb.parcelas[i].id === idParcela) {
                    this.fakeDb.parcelas[i].eliminado = false;
                    break;
                }
            }
            callback.onSuccess({
                data: {}
            });
        };
        apService.prototype.registrarNuevoContrato = function (contrato, callback) {
            var id;
            if (contrato.esAdenda) {
                id = contrato.idContratoDeLaAdenda + "_" + contrato.display.trim();
            }
            else {
                id = contrato.idOrg + "_" + contrato.display.trim();
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
        };
        apService.prototype.editarContrato = function (contrato, callback) {
            for (var i = 0; i < this.fakeDb.contratos.length; i++) {
                if (this.fakeDb.contratos[i].id === contrato.id) {
                    this.fakeDb.contratos.splice(i, 1);
                    this.fakeDb.contratos.push(contrato);
                    break;
                }
            }
            callback.onSuccess({});
        };
        apService.prototype.eliminarContrato = function (idContrato, callback) {
            for (var i = 0; i < this.fakeDb.contratos.length; i++) {
                if (this.fakeDb.contratos[i].id === idContrato) {
                    this.fakeDb.contratos[i].eliminado = true;
                    break;
                }
            }
            callback.onSuccess({
                data: {}
            });
        };
        apService.prototype.restaurarContrato = function (idContrato, callback) {
            for (var i = 0; i < this.fakeDb.contratos.length; i++) {
                if (this.fakeDb.contratos[i].id === idContrato) {
                    this.fakeDb.contratos[i].eliminado = false;
                    break;
                }
            }
            callback.onSuccess({
                data: {}
            });
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