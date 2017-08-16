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
    var apQueryService = (function (_super) {
        __extends(apQueryService, _super);
        function apQueryService($http, fakeDb) {
            var _this = _super.call(this, $http, 'ap/query') || this;
            _this.$http = $http;
            _this.fakeDb = fakeDb;
            return _this;
        }
        apQueryService.prototype.getClientes = function (filtro, callback) {
            var filteredList;
            if (filtro === "todos")
                filteredList = this.fakeDb.fakeClientesList;
            else if (filtro === "prod")
                filteredList = this.fakeDb.fakeClientesList.filter(function (x) { return x.tipo === "prod"; });
            else if (filtro === "org")
                filteredList = this.fakeDb.fakeClientesList.filter(function (x) { return x.tipo === "org"; });
            callback.onSuccess({
                data: filteredList
            });
        };
        apQueryService.prototype.getOrg = function (id, callback) {
            var dto;
            for (var i = 0; i < this.fakeDb.fakeClientesList.length; i++) {
                if (this.fakeDb.fakeClientesList[i].id === id) {
                    var x = this.fakeDb.fakeClientesList[i];
                    dto = new apArea.orgDto(x.id, x.nombre);
                    break;
                }
            }
            callback.onSuccess({
                data: dto
            });
        };
        apQueryService.prototype.getProd = function (id, callback) {
            var dto;
            for (var i = 0; i < this.fakeDb.fakeClientesList.length; i++) {
                if (this.fakeDb.fakeClientesList[i].id === id) {
                    var x = this.fakeDb.fakeClientesList[i];
                    dto = new apArea.prodDto(x.id, x.nombre);
                    break;
                }
            }
            callback.onSuccess({
                data: dto
            });
        };
        apQueryService.prototype.getServiciosPorOrg = function (idOrg, callback) {
            callback.onSuccess({
                data: this.fakeDb.fakeServiciosList
            });
        };
        apQueryService.prototype.gerParcelasDelProd = function (idProd, callback) {
            var list = this.fakeDb.parcelas.filter(function (x) { return x.idProd === idProd; });
            callback.onSuccess({
                data: list
            });
        };
        return apQueryService;
    }(common.httpLite));
    apQueryService.$inject = ['$http', 'fakeDb'];
    apArea.apQueryService = apQueryService;
})(apArea || (apArea = {}));
//# sourceMappingURL=apQueryService.js.map