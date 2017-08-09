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
            //--------------------------------
            // Fakes
            //--------------------------------
            _this.fakeClientesList = [
                new cliente("coopchorti", "Cooperativa Chortizer", "Loma Plata", "org", "./assets/img/avatar/org-icon.png"),
                new cliente("ccuu", "Cooperativa Colonias Unidas", "Itapua", "org", "./assets/img/avatar/org-icon.png"),
                new cliente("davidelias", "David Elias", "Cooperativa Chortizer", "prod", "./assets/img/avatar/8.png"),
                new cliente("kazuoyama", "Kazuo Yamazuki", "Cooperativa Pirapo", "prod", "./assets/img/avatar/9.png")
            ];
            _this.fakeServiciosList = [
                new servicioDto("David Elias", "20/12/2018", "Contrato Chorti"),
                new servicioDto("Kazuo Yamazuki", "20/12/2017", "Contrato Pirapo")
            ];
            return _this;
        }
        apQueryService.prototype.getClientes = function (filtro, callback) {
            var filteredList;
            if (filtro === "todos")
                filteredList = this.fakeClientesList;
            else if (filtro === "prod")
                filteredList = this.fakeClientesList.filter(function (x) { return x.tipo === "prod"; });
            else if (filtro === "org")
                filteredList = this.fakeClientesList.filter(function (x) { return x.tipo === "org"; });
            callback.onSuccess({
                data: filteredList
            });
        };
        apQueryService.prototype.getOrg = function (id, callback) {
            var dto;
            for (var i = 0; i < this.fakeClientesList.length; i++) {
                if (this.fakeClientesList[i].id === id) {
                    var x = this.fakeClientesList[i];
                    dto = new orgDto(x.id, x.nombre);
                    break;
                }
            }
            callback.onSuccess({
                data: dto
            });
        };
        apQueryService.prototype.getProd = function (id, callback) {
            var dto;
            for (var i = 0; i < this.fakeClientesList.length; i++) {
                if (this.fakeClientesList[i].id === id) {
                    var x = this.fakeClientesList[i];
                    dto = new prodDto(x.id, x.nombre);
                    break;
                }
            }
            callback.onSuccess({
                data: dto
            });
        };
        apQueryService.prototype.getServiciosPorOrg = function (idOrg, callback) {
            callback.onSuccess({
                data: this.fakeServiciosList
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
    var orgDto = (function () {
        function orgDto(id, display) {
            this.id = id;
            this.display = display;
        }
        return orgDto;
    }());
    apArea.orgDto = orgDto;
    var servicioDto = (function () {
        function servicioDto(productorDisplay, fecha, contrato) {
            this.productorDisplay = productorDisplay;
            this.fecha = fecha;
            this.contrato = contrato;
        }
        return servicioDto;
    }());
    apArea.servicioDto = servicioDto;
    var prodDto = (function () {
        function prodDto(id, display) {
            this.id = id;
            this.display = display;
        }
        return prodDto;
    }());
    apArea.prodDto = prodDto;
})(apArea || (apArea = {}));
//# sourceMappingURL=apQueryService.js.map