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
                filteredList = this.fakeDb.clientes;
            else if (filtro === "prod")
                filteredList = this.fakeDb.clientes.filter(function (x) { return x.tipo === "prod"; });
            else if (filtro === "org")
                filteredList = this.fakeDb.clientes.filter(function (x) { return x.tipo === "org"; });
            callback.onSuccess({
                data: filteredList
            });
        };
        apQueryService.prototype.getOrg = function (id, callback) {
            var dto;
            for (var i = 0; i < this.fakeDb.orgs.length; i++) {
                if (this.fakeDb.orgs[i].id === id) {
                    var x = this.fakeDb.orgs[i];
                    dto = new apArea.orgDto(x.id, x.display, x.avatarUrl);
                    break;
                }
            }
            callback.onSuccess({
                data: dto
            });
        };
        apQueryService.prototype.getProd = function (id, callback) {
            var dto;
            for (var i = 0; i < this.fakeDb.prods.length; i++) {
                if (this.fakeDb.prods[i].id === id) {
                    var x = this.fakeDb.prods[i];
                    dto = new apArea.prodDto(x.id, x.display, x.avatarUrl, x.orgs);
                    break;
                }
            }
            callback.onSuccess({
                data: dto
            });
        };
        apQueryService.prototype.getServiciosPorOrg = function (idOrg, callback) {
            callback.onSuccess({
                data: []
            });
        };
        apQueryService.prototype.getParcelasDelProd = function (idProd, callback) {
            var list = this.fakeDb.parcelas.filter(function (x) { return x.idProd === idProd; });
            callback.onSuccess({
                data: list
            });
        };
        apQueryService.prototype.getContratos = function (idOrg, callback) {
            var list = this.fakeDb.contratos.filter(function (x) { return x.idOrg === idOrg; });
            callback.onSuccess({
                data: list
            });
        };
        apQueryService.prototype.getOrgsConContratos = function (idProd, callback) {
            var _this = this;
            var orgs;
            for (var i = 0; i < this.fakeDb.prods.length; i++) {
                var prod = this.fakeDb.prods[i];
                if (prod.id === idProd) {
                    orgs = prod.orgs;
                    break;
                }
            }
            var list = orgs.map(function (o) { return new apArea.orgConContratos(o, _this.fakeDb.contratos.filter(function (c) { return c.idOrg === o.id; })); });
            callback.onSuccess({ data: list });
        };
        return apQueryService;
    }(common.httpLite));
    apQueryService.$inject = ['$http', 'fakeDb'];
    apArea.apQueryService = apQueryService;
})(apArea || (apArea = {}));
//# sourceMappingURL=apQueryService.js.map