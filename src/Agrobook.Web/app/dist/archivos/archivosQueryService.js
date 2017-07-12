/// <reference path="../_all.ts" />
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
var archivosArea;
(function (archivosArea) {
    var archivosQueryService = (function (_super) {
        __extends(archivosQueryService, _super);
        function archivosQueryService($http, loginQueryService) {
            var _this = _super.call(this, $http, 'archivos/query') || this;
            _this.$http = $http;
            _this.loginQueryService = loginQueryService;
            return _this;
        }
        archivosQueryService.prototype.obtenerListaDeProductores = function (onSuccess, onError) {
            _super.prototype.get.call(this, 'productores', onSuccess, onError);
        };
        archivosQueryService.prototype.obtenerArchivosDelProductor = function (idProductor, onSuccess, onError) {
            _super.prototype.get.call(this, 'archivos-del-productor/' + idProductor, onSuccess, onError);
        };
        archivosQueryService.prototype.download = function (idProductor, nombre, extension) {
            // Could be improved here: https://stackoverflow.com/questions/24080018/download-file-from-an-asp-net-web-api-method-using-angularjs
            var userInfo = this.loginQueryService.tryGetLocalLoginInfo();
            var usuario = userInfo.usuario;
            window.open("./archivos/query/download/" + idProductor + "/" + nombre + "/" + extension + "/" + usuario, '_blank', '');
        };
        return archivosQueryService;
    }(common.httpLite));
    archivosQueryService.$inject = ['$http', 'loginQueryService'];
    archivosArea.archivosQueryService = archivosQueryService;
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=archivosQueryService.js.map