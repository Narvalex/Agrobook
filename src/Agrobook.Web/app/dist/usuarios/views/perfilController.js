/// <reference path="../../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var perfilController = (function () {
        function perfilController($routeParams, loginQueryService, usuariosQueryService, toasterLite, config) {
            var _this = this;
            this.$routeParams = $routeParams;
            this.loginQueryService = loginQueryService;
            this.usuariosQueryService = usuariosQueryService;
            this.toasterLite = toasterLite;
            this.config = config;
            this.loaded = false;
            this.avatarUrls = [];
            this.avatarUrls = config.avatarUrls;
            var idUsuario = this.$routeParams['idUsuario'];
            if (idUsuario === undefined) {
                var usuario = this.loginQueryService.tryGetLocalLoginInfo();
                this.inicializarEdicionDeInfoBasica(new usuariosArea.usuarioInfoBasica(usuario.usuario, usuario.nombreParaMostrar, usuario.avatarUrl));
                this.loaded = true;
            }
            else {
                this.usuariosQueryService.obtenerInfoBasicaDeUsuario(idUsuario, function (value) {
                    _this.inicializarEdicionDeInfoBasica(value.data);
                    _this.loaded = true;
                }, function (reason) {
                    _this.toasterLite.error('Ocurrió un error al recuperar información del usuario ' + idUsuario, _this.toasterLite.delayForever);
                });
            }
        }
        perfilController.prototype.inicializarEdicionDeInfoBasica = function (usuarioRecuperado) {
            this.usuarioRecuperado = usuarioRecuperado;
            this.usuarioEditado = new usuariosArea.usuarioInfoBasica(usuarioRecuperado.nombre, usuarioRecuperado.nombreParaMostrar, usuarioRecuperado.avatarUrl);
        };
        return perfilController;
    }());
    perfilController.$inject = ['$routeParams', 'loginQueryService', 'usuariosQueryService', 'toasterLite', 'config'];
    usuariosArea.perfilController = perfilController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=perfilController.js.map