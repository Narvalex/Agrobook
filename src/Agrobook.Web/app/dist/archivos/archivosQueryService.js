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
        function archivosQueryService($http, loginQueryService, config) {
            var _this = _super.call(this, $http, 'archivos/query') || this;
            _this.$http = $http;
            _this.loginQueryService = loginQueryService;
            _this.config = config;
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
        archivosQueryService.prototype.resolverIcono = function (ext) {
            var tipos = this.config.tiposDeArchivos;
            if (this.esPdf(ext)) {
                return tipos.pdf.icon;
            }
            else if (this.esMapa(ext)) {
                return tipos.mapas.icon;
            }
            else if (this.esFoto(ext)) {
                return tipos.fotos.icon;
            }
            else if (this.esExcel(ext)) {
                return tipos.excel.icon;
            }
            else if (this.esWord(ext)) {
                return tipos.word.icon;
            }
            else if (this.esPowerPoint(ext)) {
                return tipos.powerPoint.icon;
            }
            else
                return tipos.generico.icon;
        };
        // Es algo section
        archivosQueryService.prototype.esPdf = function (extension) {
            if (extension === undefined)
                return false;
            extension = extension.toLowerCase();
            return extension === 'pdf';
        };
        archivosQueryService.prototype.esFoto = function (extension) {
            if (extension === undefined)
                return false;
            extension = extension.toLowerCase();
            return extension === 'jpg'
                || extension === 'jpeg';
        };
        archivosQueryService.prototype.esMapa = function (extension) {
            if (extension === undefined)
                return false;
            extension = extension.toLowerCase();
            return extension === 'kmz'
                || extension === 'kml';
        };
        archivosQueryService.prototype.esExcel = function (extension) {
            if (extension === undefined)
                return false;
            extension = extension.toLowerCase();
            return extension === 'xls'
                || extension === 'xlsx';
        };
        archivosQueryService.prototype.esWord = function (extension) {
            if (extension === undefined)
                return false;
            extension = extension.toLowerCase();
            return extension === 'doc'
                || extension === 'docx'
                || extension === 'text'
                || extension === 'rtf';
        };
        archivosQueryService.prototype.esPowerPoint = function (extension) {
            if (extension === undefined)
                return false;
            extension = extension.toLowerCase();
            return extension === 'ppt'
                || extension === 'pptx';
        };
        return archivosQueryService;
    }(common.httpLite));
    archivosQueryService.$inject = ['$http', 'loginQueryService', 'config'];
    archivosArea.archivosQueryService = archivosQueryService;
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=archivosQueryService.js.map