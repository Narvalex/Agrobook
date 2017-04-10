/// <reference path="../../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var usuarioController = (function () {
        function usuarioController($routeParams) {
            this.$routeParams = $routeParams;
            var idUsuario = this.$routeParams['idUsuario'];
            this.nombreCompleto = idUsuario;
        }
        return usuarioController;
    }());
    usuarioController.$inject = ['$routeParams'];
    usuariosArea.usuarioController = usuarioController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=usuarioController.js.map