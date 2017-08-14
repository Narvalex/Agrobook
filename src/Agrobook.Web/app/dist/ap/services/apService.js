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
        function apService($http, $q) {
            var _this = _super.call(this, $http, 'ap') || this;
            _this.$http = $http;
            _this.$q = $q;
            return _this;
        }
        apService.prototype.registrarNuevaParcela = function (nombre, callback) {
            var dto = null; //new parcelaDto(nombre.trim(), nombre);
            callback.onSuccess({
                data: dto
            });
        };
        return apService;
    }(common.httpLite));
    apService.$inject = ['$http'];
    apArea.apService = apService;
})(apArea || (apArea = {}));
//# sourceMappingURL=apService.js.map