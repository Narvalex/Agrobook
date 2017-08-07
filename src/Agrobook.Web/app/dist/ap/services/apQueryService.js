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
        function apQueryService($http, $q) {
            var _this = _super.call(this, $http, 'ap/query') || this;
            _this.$http = $http;
            _this.$q = $q;
            return _this;
        }
        apQueryService.prototype.getClientes = function (filtro, callback) {
            var filteredList;
            var list = [
                new cliente("coopchorti", "Cooperativa Chortizer", "Loma Plata", "org", "./assets/img/avatar/6.png"),
                new cliente("ccuu", "Cooperativa Colonias Unidas", "Itapua", "org", "./assets/img/avatar/7.png"),
                new cliente("davidelias", "David Elias", "Cooperativa Chortizer", "prod", "./assets/img/avatar/8.png"),
                new cliente("kazuoyama", "Kazuo Yamazuki", "Cooperativa Pirapo", "prod", "./assets/img/avatar/9.png")
            ];
            if (filtro === "todos")
                filteredList = list;
            else if (filtro === "prod")
                filteredList = list.filter(function (x) { return x.tipo === "prod"; });
            else if (filtro === "org")
                filteredList = list.filter(function (x) { return x.tipo === "org"; });
            callback.onSuccess({
                data: filteredList
            });
        };
        return apQueryService;
    }(common.httpLite));
    apQueryService.$inject = ['$http'];
    apArea.apQueryService = apQueryService;
    //---------------------
    // DTOS
    //---------------------
    var cliente = (function () {
        function cliente(id, // coopchorti / davidelias
            nombre, // Cooperativa Chortizer / David Elias
            desc, // Loma Plata / Productor de Chooperativa Chortizer y Colonias Unidas
            tipo, // org / prod
            avatarUrl) {
            this.id = id;
            this.nombre = nombre;
            this.desc = desc;
            this.tipo = tipo;
            this.avatarUrl = avatarUrl;
        }
        return cliente;
    }());
    apArea.cliente = cliente;
})(apArea || (apArea = {}));
//# sourceMappingURL=apQueryService.js.map