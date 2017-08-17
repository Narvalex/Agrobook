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
        function apService($http, fakeDb) {
            var _this = _super.call(this, $http, 'ap') || this;
            _this.$http = $http;
            _this.fakeDb = fakeDb;
            return _this;
        }
        apService.prototype.registrarNuevaParcela = function (dto, callback) {
            var data = new apArea.parcelaDto(dto.idProd + '_' + dto.display.trim(), dto.idProd, dto.display, dto.hectareas);
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
            callback.onSuccess({
                data: {}
            });
        };
        apService.prototype.eliminar = function (idParcela, callback) {
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
        apService.prototype.restaurar = function (idParcela, callback) {
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
        return apService;
    }(common.httpLite));
    apService.$inject = ['$http', 'fakeDb'];
    apArea.apService = apService;
})(apArea || (apArea = {}));
//# sourceMappingURL=apService.js.map