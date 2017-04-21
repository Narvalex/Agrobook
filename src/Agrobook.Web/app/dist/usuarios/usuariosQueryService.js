/// <reference path="../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var usuariosQueryService = (function () {
        function usuariosQueryService(httpLite) {
            this.httpLite = httpLite;
            this.httpLite.prefix = 'usuarios/query';
        }
        usuariosQueryService.prototype.obtenerListaDeTodosLosUsuarios = function (onSuccess, onError) {
            this.httpLite.get('todos', onSuccess, onError);
        };
        usuariosQueryService.prototype.obtenerInfoBasicaDeUsuario = function (usuario, onSuccess, onError) {
            this.httpLite.get('info-basica/' + usuario, onSuccess, onError);
        };
        return usuariosQueryService;
    }());
    usuariosQueryService.$inject = ['httpLite'];
    usuariosArea.usuariosQueryService = usuariosQueryService;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=usuariosQueryService.js.map