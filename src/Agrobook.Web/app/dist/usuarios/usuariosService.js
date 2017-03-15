/// <reference path="../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var usuariosService = (function () {
        function usuariosService($http) {
            this.$http = $http;
            this.prefix = 'usuarios/';
        }
        usuariosService.prototype.post = function (url, dto) {
            return this.$http.post(this.prefix + url, dto);
        };
        usuariosService.prototype.crearNuevoUsuario = function (usuario, s, e) {
            this.post('crear-nuevo-usuario', usuario).then(function (value) { s(value); }, function (reason) { e(reason); });
        };
        return usuariosService;
    }());
    usuariosService.$inject = ['$http'];
    usuariosArea.usuariosService = usuariosService;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=usuariosService.js.map