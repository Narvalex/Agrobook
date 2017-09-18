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
            var _this = _super.call(this, $http, 'ap/query') || this;
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
            var list = this.fakeDb.servicios.filter(function (x) { return x.idOrg === idOrg; });
            this.$timeout(function () { return callback.onSuccess({ data: list }); }, 750);
        };
        apQueryService.prototype.getServiciosPorProd = function (idProd, callback) {
            var lista = this.fakeDb.servicios.filter(function (x) { return x.idProd === idProd; });
            this.$timeout(function () { return callback.onSuccess({ data: lista }); }, 750);
        };
        apQueryService.prototype.getServicio = function (idServicio, callback) {
            var servicio;
            for (var i = 0; i < this.fakeDb.servicios.length; i++) {
                servicio = this.fakeDb.servicios[i];
                if (servicio.id === idServicio)
                    break;
            }
            this.$timeout(function () { return callback.onSuccess({ data: servicio }); }, 500);
        };
        apQueryService.prototype.getParcelasDelProd = function (idProd, callback) {
            var list = this.fakeDb.parcelas.filter(function (x) { return x.idProd === idProd; });
            callback.onSuccess({
                data: list
            });
        };
        apQueryService.prototype.getParcela = function (idParcela, callback) {
            var dto = this.fakeDb.parcelas.filter(function (x) { return x.id === idParcela; });
            this.$timeout(function () {
                return callback.onSuccess({
                    data: dto[0]
                });
            }, 500);
        };
        apQueryService.prototype.getContratos = function (idOrg, callback) {
            _super.prototype.getWithCallback.call(this, 'contratos/' + idOrg, callback);
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
            this.$timeout(function () { return callback.onSuccess({ data: list }); }, 500);
        };
        return apQueryService;
    }(common.httpLite));
    apQueryService.$inject = ['$http', 'fakeDb', '$timeout'];
    apArea.apQueryService = apQueryService;
})(apArea || (apArea = {}));
//# sourceMappingURL=apQueryService.js.map