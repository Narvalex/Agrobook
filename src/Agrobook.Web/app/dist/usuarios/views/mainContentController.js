/// <reference path="../../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var mainContentController = (function () {
        function mainContentController($routeParams, loginQueryService) {
            this.$routeParams = $routeParams;
            this.loginQueryService = loginQueryService;
            var idUsuario = this.$routeParams['idUsuario'];
            if (idUsuario === undefined) {
                var usuario = this.loginQueryService.tryGetLocalLoginInfo();
                idUsuario = usuario.usuario;
            }
            this.nombreCompleto = idUsuario;
        }
        return mainContentController;
    }());
    mainContentController.$inject = ['$routeParams', 'loginQueryService'];
    usuariosArea.mainContentController = mainContentController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=mainContentController.js.map