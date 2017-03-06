/// <reference path="../_all.ts" />
var UsuariosArea;
(function (UsuariosArea) {
    var usuariosWriteService = (function () {
        function usuariosWriteService($http) {
            this.$http = $http;
        }
        usuariosWriteService.prototype.crearNuevoUsuario = function (usuario) {
            return this.$http.post('', usuario);
        };
        return usuariosWriteService;
    }());
    usuariosWriteService.$inject = ['$http'];
    UsuariosArea.usuariosWriteService = usuariosWriteService;
})(UsuariosArea || (UsuariosArea = {}));
//# sourceMappingURL=usuariosWriteService.js.map