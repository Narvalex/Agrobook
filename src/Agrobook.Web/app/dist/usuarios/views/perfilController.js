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
        perfilController.prototype.actualizarPerfil = function () {
            if (!this.perfilEstaEditado)
                this.toasterLite.info('No hay nada para actualizar');
            if (!this.intentarValidarEdicionDePerfil())
                return;
        };
        perfilController.prototype.resetearPassword = function () {
            console.log('password reseteado');
        };
        perfilController.prototype.inicializarEdicionDeInfoBasica = function (usuarioRecuperado) {
            this.usuarioRecuperado = usuarioRecuperado;
            this.usuarioEditado = new usuariosArea.usuarioInfoBasica(usuarioRecuperado.nombre, usuarioRecuperado.nombreParaMostrar, usuarioRecuperado.avatarUrl);
        };
        Object.defineProperty(perfilController.prototype, "perfilEstaEditado", {
            get: function () {
                if (this.usuarioRecuperado.avatarUrl !== this.usuarioEditado.avatarUrl)
                    return true;
                if (this.usuarioRecuperado.nombreParaMostrar !== this.usuarioEditado.nombreParaMostrar)
                    return true;
                if (this.seQuiereActualizarPassword)
                    return true;
                return false;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(perfilController.prototype, "seQuiereActualizarPassword", {
            get: function () {
                if (this.nuevoPassword !== undefined
                    && this.nuevoPassword !== null
                    && this.nuevoPassword !== '')
                    return true;
                else
                    return false;
            },
            enumerable: true,
            configurable: true
        });
        perfilController.prototype.intentarValidarEdicionDePerfil = function () {
            if (this.seQuiereActualizarPassword) {
                if (this.passwordActual === undefined
                    || this.passwordActual === null
                    || this.passwordActual === '') {
                    this.toasterLite.error('Debe ingresar el password actual para actualizarlo.');
                    return false;
                }
                if (this.nuevoPassword !== this.nuevoPasswordConfirmacion)
                    this.toasterLite.error('El password ingresado no coincide con la confirmación');
                return false;
            }
            return true;
        };
        return perfilController;
    }());
    perfilController.$inject = ['$routeParams', 'loginQueryService', 'usuariosQueryService', 'toasterLite', 'config'];
    usuariosArea.perfilController = perfilController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=perfilController.js.map