/// <reference path="../../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var mainContentController = (function () {
        function mainContentController($routeParams, loginQueryService, usuariosQueryService, toasterLite) {
            var _this = this;
            this.$routeParams = $routeParams;
            this.loginQueryService = loginQueryService;
            this.usuariosQueryService = usuariosQueryService;
            this.toasterLite = toasterLite;
            this.loaded = false;
            var idUsuario = this.$routeParams['idUsuario'];
            if (idUsuario === undefined) {
                var usuario = this.loginQueryService.tryGetLocalLoginInfo();
                this.usuario = new usuariosArea.usuarioInfoBasica(usuario.usuario, usuario.nombreParaMostrar, usuario.avatarUrl);
                this.loaded = true;
            }
            else {
                this.usuariosQueryService.obtenerInfoBasicaDeUsuario(idUsuario, function (value) {
                    _this.usuario = value.data;
                    _this.loaded = true;
                }, function (reason) {
                    _this.toasterLite.error('Ocurrió un error al recuperar información del usuario ' + idUsuario, _this.toasterLite.delayForever);
                });
            }
        }
        return mainContentController;
    }());
    mainContentController.$inject = ['$routeParams', 'loginQueryService', 'usuariosQueryService', 'toasterLite'];
    usuariosArea.mainContentController = mainContentController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=mainContentController.js.map