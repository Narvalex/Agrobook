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
var usuariosArea;
(function (usuariosArea) {
    var usuariosService = (function (_super) {
        __extends(usuariosService, _super);
        function usuariosService($http) {
            var _this = _super.call(this, $http, 'usuarios') || this;
            _this.$http = $http;
            return _this;
        }
        usuariosService.prototype.crearNuevoUsuario = function (usuario, onSuccess, onError) {
            this.post('crear-nuevo-usuario', usuario, onSuccess, onError);
        };
        usuariosService.prototype.actualizarPerfil = function (usuario, onSuccess, onError) {
            this.post('actualizar-perfil', usuario, onSuccess, onError);
        };
        usuariosService.prototype.resetearPassword = function (usuario, onSuccess, onError) {
            this.post('resetear-password/' + usuario, {}, onSuccess, onError);
        };
        usuariosService.prototype.crearNuevaOrganizacion = function (nombreOrg, onSuccess, onError) {
            this.post('crear-nueva-organizacion/' + nombreOrg, {}, onSuccess, onError);
        };
        return usuariosService;
    }(common.httpLite));
    usuariosService.$inject = ['$http'];
    usuariosArea.usuariosService = usuariosService;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=usuariosService.js.map