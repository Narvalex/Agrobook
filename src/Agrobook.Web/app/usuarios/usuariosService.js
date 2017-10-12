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
            var _this = _super.call(this, $http, '../usuarios') || this;
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
        usuariosService.prototype.crearNuevoGrupo = function (orgId, displayGrupoName, onSuccess, onError) {
            this.post('crear-nuevo-grupo/' + orgId + '/' + displayGrupoName, {}, onSuccess, onError);
        };
        usuariosService.prototype.agregarUsuarioALaOrganizacion = function (idUsuario, idOrganizacion, onSuccess, onError) {
            this.post("agregar-usuario-a-la-organizacion/" + idUsuario + "/" + idOrganizacion, {}, onSuccess, onError);
        };
        usuariosService.prototype.removerUsuarioDeOrganizacion = function (idUsuario, idOrganizacion, callback) {
            var cmd = {
                idUsuario: idUsuario,
                idOrganizacion: idOrganizacion
            };
            this.postWithCallback('remover-usuario-de-organizacion', cmd, callback);
        };
        usuariosService.prototype.agregarUsuarioAGrupo = function (idUsuario, idOrganizacion, idGrupo, onSuccess, onError) {
            this.post("agregar-usuario-a-grupo/" + idUsuario + "/" + idOrganizacion + "/" + idGrupo, {}, onSuccess, onError);
        };
        usuariosService.prototype.removerUsuarioDeUnGrupo = function (idUsuario, idOrganizacion, idGrupo, onSuccess, onError) {
            this.post("remover-usuario-de-un-grupo/" + idUsuario + "/" + idOrganizacion + "/" + idGrupo, {}, onSuccess, onError);
        };
        usuariosService.prototype.otorgarPermiso = function (idUsuario, permiso, callback) {
            _super.prototype.postWithCallback.call(this, "otorgar-permiso?usuario=" + idUsuario + "&permiso=" + permiso, {}, callback);
        };
        usuariosService.prototype.retirarPermiso = function (idUsuario, permiso, callback) {
            _super.prototype.postWithCallback.call(this, "retirar-permiso?usuario=" + idUsuario + "&permiso=" + permiso, {}, callback);
        };
        return usuariosService;
    }(common.httpLite));
    usuariosService.$inject = ['$http'];
    usuariosArea.usuariosService = usuariosService;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=usuariosService.js.map