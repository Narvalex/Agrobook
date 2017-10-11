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
        function apQueryService($http, fakeDb, $timeout) {
            var _this = _super.call(this, $http, '../../ap/query') || this;
            _this.$http = $http;
            _this.fakeDb = fakeDb;
            _this.$timeout = $timeout;
            return _this;
        }
        apQueryService.prototype.getClientes = function (filtro, callback) {
            _super.prototype.getWithCallback.call(this, 'clientes?filtro=' + filtro, callback);
        };
        apQueryService.prototype.getOrg = function (id, callback) {
            _super.prototype.getWithCallback.call(this, 'org/' + id, callback);
        };
        apQueryService.prototype.getProd = function (idProd, callback) {
            _super.prototype.getWithCallback.call(this, 'prod/' + idProd, callback);
        };
        apQueryService.prototype.getServiciosPorOrg = function (idOrg, callback) {
            _super.prototype.getWithCallback.call(this, 'servicios-por-org/' + idOrg, callback);
        };
        apQueryService.prototype.getServiciosPorProd = function (idProd, callback) {
            _super.prototype.getWithCallback.call(this, 'servicios-por-prod/' + idProd, callback);
        };
        apQueryService.prototype.getServicio = function (idServicio, callback) {
            _super.prototype.getWithCallback.call(this, 'servicio/' + idServicio, callback);
        };
        apQueryService.prototype.getParcelasDelProd = function (idProd, callback) {
            _super.prototype.getWithCallback.call(this, 'parcelas/' + idProd, callback);
        };
        apQueryService.prototype.getParcela = function (idParcela, callback) {
            _super.prototype.getWithCallback.call(this, 'parcela/' + idParcela, callback);
        };
        apQueryService.prototype.getContratos = function (idOrg, callback) {
            _super.prototype.getWithCallback.call(this, 'contratos/' + idOrg, callback);
        };
        apQueryService.prototype.getOrgsConContratosDelProductor = function (idProd, callback) {
            _super.prototype.getWithCallback.call(this, 'orgs-con-contratos-del-productor/' + idProd, callback);
        };
        return apQueryService;
    }(common.httpLite));
    apQueryService.$inject = ['$http', 'fakeDb', '$timeout'];
    apArea.apQueryService = apQueryService;
})(apArea || (apArea = {}));
//# sourceMappingURL=apQueryService.js.map