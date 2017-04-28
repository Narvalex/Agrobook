/// <reference path="../../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var perfilController = (function () {
        function perfilController($routeParams, loginQueryService, usuariosService, usuariosQueryService, toasterLite, config, $rootScope, $scope) {
            var _this = this;
            this.$routeParams = $routeParams;
            this.loginQueryService = loginQueryService;
            this.usuariosService = usuariosService;
            this.usuariosQueryService = usuariosQueryService;
            this.toasterLite = toasterLite;
            this.config = config;
            this.$rootScope = $rootScope;
            this.$scope = $scope;
            this.loaded = false;
            this.avatarUrls = [];
            this.avatarUrls = config.avatarUrls;
            var idUsuario = this.$routeParams['idUsuario'];
            var usuario;
            if (idUsuario === undefined)
                idUsuario = this.loginQueryService.tryGetLocalLoginInfo().usuario;
            this.usuariosQueryService.obtenerInfoBasicaDeUsuario(idUsuario, function (value) {
                _this.inicializarEdicionDeInfoBasica(value.data);
                _this.loaded = true;
            }, function (reason) {
                _this.toasterLite.error('Ocurrió un error al recuperar información del usuario ' + idUsuario, _this.toasterLite.delayForever);
            });
            this.$scope.$on(this.config.eventIndex.usuarios.perfilActualizado, function (e, args) {
                _this.inicializarEdicionDeInfoBasica(new usuariosArea.usuarioInfoBasica(args.usuario, args.nombreParaMostrar, args.avatarUrl));
            });
        }
        perfilController.prototype.actualizarPerfil = function () {
            var _this = this;
            if (!this.perfilEstaEditado)
                this.toasterLite.info('No hay nada para actualizar');
            if (!this.intentarValidarEdicionDePerfil())
                return;
            var dto = new usuariosArea.actualizarPerfilDto(this.usuarioRecuperado.nombre, this.usuarioEditado.avatarUrl, this.usuarioEditado.nombreParaMostrar, this.passwordActual, this.nuevoPassword);
            this.usuariosService.actualizarPerfil(dto, function (value) {
                _this.$rootScope.$broadcast(_this.config.eventIndex.usuarios.perfilActualizado, new common.perfilActualizado(dto.usuario, dto.avatarUrl, dto.nombreParaMostrar));
                _this.toasterLite.success('El perfil se ha actualizado exitosamente');
            }, function (reason) { return _this.toasterLite.error('Ocurrió un error al intentar actualizar el perfil'); });
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
                if (this.nuevoPassword !== this.nuevoPasswordConfirmacion) {
                    this.toasterLite.error('El password ingresado no coincide con la confirmación');
                    return false;
                }
            }
            return true;
        };
        return perfilController;
    }());
    perfilController.$inject = ['$routeParams', 'loginQueryService', 'usuariosService',
        'usuariosQueryService', 'toasterLite', 'config', '$rootScope', '$scope'];
    usuariosArea.perfilController = perfilController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=perfilController.js.map