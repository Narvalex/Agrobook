/// <reference path="../../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var perfilController = /** @class */ (function () {
        function perfilController($routeParams, loginService, loginQueryService, usuariosService, usuariosQueryService, toasterLite, config, $rootScope, $scope) {
            var _this = this;
            this.$routeParams = $routeParams;
            this.loginService = loginService;
            this.loginQueryService = loginQueryService;
            this.usuariosService = usuariosService;
            this.usuariosQueryService = usuariosQueryService;
            this.toasterLite = toasterLite;
            this.config = config;
            this.$rootScope = $rootScope;
            this.$scope = $scope;
            this.infoBasicaLoaded = false;
            this.claimsLoaded = false;
            this.permisosOtorgadosLoaded = false;
            // Claim Adding / Removal
            this.mostrarEdicionDeClaims = false;
            this.permisosOtorgados = []; // parece que debe estar inicializado para que los chips aparezcan
            this.aplicandoPermisos = false;
            this.avatarUrls = [];
            // auth
            var roles = this.config.claims.roles;
            this.mostrarEdicionDeClaims = this.loginService.autorizar([roles.Gerente]);
            this.avatarUrls = config.avatarUrls;
            this.usuarioLogueado = this.loginQueryService.tryGetLocalLoginInfo();
            var idUsuario = this.$routeParams['idUsuario'];
            var usuario;
            if (idUsuario === undefined)
                idUsuario = this.usuarioLogueado.usuario;
            this.usuariosQueryService.obtenerInfoBasicaDeUsuario(idUsuario, function (value) {
                _this.inicializarEdicionDeInfoBasica(value.data);
                _this.infoBasicaLoaded = true;
            }, function (reason) {
                _this.toasterLite.error('Ocurrió un error al recuperar información del usuario ' + idUsuario, _this.toasterLite.delayForever);
            });
            this.usuariosQueryService.obtenerListaDeClaims(function (value) {
                _this.claims = value.data;
                _this.claimsLoaded = true;
            }, function (reason) {
                _this.toasterLite.error("Error! Ver logs");
            });
            this.usuariosQueryService.obtenerClaimsDelUsuario(idUsuario, new common.callbackLite(function (value) {
                value.data.forEach(function (x) {
                    _this.permisosOtorgados.push(x); // tenemos que pushear, no reemplazar
                });
                _this.permisosOtorgadosLoaded = true;
            }, function (reason) {
                _this.toasterLite.error("Error al recuperar claims");
            }));
            this.$scope.$on(this.config.eventIndex.usuarios.perfilActualizado, function (e, args) {
                _this.inicializarEdicionDeInfoBasica(new usuariosArea.usuarioInfoBasica(args.usuario, args.nombreParaMostrar, args.avatarUrl, args.telefono, args.email));
            });
        }
        perfilController.prototype.allLoaded = function () { return this.infoBasicaLoaded && this.claimsLoaded && this.permisosOtorgadosLoaded; };
        perfilController.prototype.actualizarPerfil = function () {
            var _this = this;
            if (!this.perfilEstaEditado) {
                this.toasterLite.info('No hay nada para actualizar');
                return;
            }
            if (!this.intentarValidarEdicionDePerfil())
                return;
            var dto = new usuariosArea.actualizarPerfilDto(this.usuarioRecuperado.nombre, this.usuarioEditado.avatarUrl, this.usuarioEditado.nombreParaMostrar, this.passwordActual, this.nuevoPassword, this.usuarioEditado.telefono, this.usuarioEditado.email);
            this.usuariosService.actualizarPerfil(dto, function (value) {
                _this.$rootScope.$broadcast(_this.config.eventIndex.usuarios.perfilActualizado, new common.perfilActualizado(dto.usuario, dto.avatarUrl, dto.nombreParaMostrar, dto.telefono, dto.email));
                _this.toasterLite.info('El perfil se ha actualizado exitosamente. Atención: si actualizó su contraseña entonces usted debe volver a iniciar sesión.');
            }, function (reason) { return _this.toasterLite.error('Ocurrió un error al intentar actualizar el perfil'); });
        };
        perfilController.prototype.resetearPassword = function () {
            var _this = this;
            this.usuariosService.resetearPassword(this.usuarioRecuperado.nombre, function (value) { return _this.toasterLite.success('Password reseteado exitosamente'); }, function (reason) { return _this.toasterLite.error('Ocurrió un error al intentar resetear el password'); });
        };
        Object.defineProperty(perfilController.prototype, "perfilEstaEditado", {
            get: function () {
                if (this.usuarioRecuperado === undefined)
                    return false;
                if (this.usuarioRecuperado.avatarUrl !== this.usuarioEditado.avatarUrl)
                    return true;
                if (this.usuarioRecuperado.nombreParaMostrar !== this.usuarioEditado.nombreParaMostrar)
                    return true;
                if (this.seQuiereActualizarPassword)
                    return true;
                if (this.usuarioRecuperado.telefono !== this.usuarioEditado.telefono)
                    return true;
                if (this.usuarioRecuperado.email !== this.usuarioEditado.email)
                    return true;
                return false;
            },
            enumerable: true,
            configurable: true
        });
        perfilController.prototype.otorgarPermiso = function (claim) {
            var _this = this;
            this.aplicandoPermisos = true;
            this.usuariosService.otorgarPermiso(this.usuarioRecuperado.nombre, claim.id, new common.callbackLite(function (value) {
                _this.permisosOtorgados.push(claim);
                _this.toasterLite.success('Permiso otorgado');
                _this.aplicandoPermisos = false;
            }, function (reason) {
                _this.toasterLite.error('No se pudo otorgar el permiso');
                _this.aplicandoPermisos = false;
            }));
        };
        perfilController.prototype.retirarPermiso = function (claim) {
            var _this = this;
            this.aplicandoPermisos = true;
            this.usuariosService.retirarPermiso(this.usuarioRecuperado.nombre, claim.id, new common.callbackLite(function (value) {
                for (var i = 0; i < _this.permisosOtorgados.length; i++) {
                    if (_this.permisosOtorgados[i].id === claim.id) {
                        _this.permisosOtorgados.splice(i, 1);
                        break;
                    }
                }
                if (_this.permisosOtorgados.length === 0) {
                    _this.permisosOtorgados.push(new usuariosArea.claimDto('rol-invitado', 'Invitado', 'Los invitados pueden ver los trabajos realizados a productores de su organización.'));
                }
                _this.toasterLite.success('Permiso retirado');
                _this.aplicandoPermisos = false;
            }, function (reason) {
                _this.toasterLite.error('No se pudo retirar el permiso');
                _this.aplicandoPermisos = false;
            }));
        };
        perfilController.prototype.inicializarEdicionDeInfoBasica = function (usuarioRecuperado) {
            this.usuarioRecuperado = usuarioRecuperado;
            this.usuarioEditado = new usuariosArea.usuarioInfoBasica(usuarioRecuperado.nombre, usuarioRecuperado.nombreParaMostrar, usuarioRecuperado.avatarUrl, usuarioRecuperado.telefono, usuarioRecuperado.email);
        };
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
        perfilController.$inject = ['$routeParams', 'loginService', 'loginQueryService', 'usuariosService',
            'usuariosQueryService', 'toasterLite', 'config', '$rootScope', '$scope'];
        return perfilController;
    }());
    usuariosArea.perfilController = perfilController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=perfilController.js.map