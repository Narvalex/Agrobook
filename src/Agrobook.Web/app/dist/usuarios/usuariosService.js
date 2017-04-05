/// <reference path="../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var usuariosService = (function () {
        function usuariosService(httpLite) {
            this.httpLite = httpLite;
            this.httpLite.prefix = 'usuarios';
        }
        usuariosService.prototype.crearNuevoUsuario = function (usuario, onSuccess, onError) {
            this.httpLite.post('crear-nuevo-usuario', usuario, onSuccess, onError);
        };
        return usuariosService;
    }());
    usuariosService.$inject = ['httpLite'];
    usuariosArea.usuariosService = usuariosService;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=usuariosService.js.map