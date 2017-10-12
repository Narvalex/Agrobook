/// <reference path="../../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var organizacionesController = (function () {
        function organizacionesController(usuariosService, usuariosQueryService, toasterLite, $routeParams, loginQueryService, $rootScope, config) {
            this.usuariosService = usuariosService;
            this.usuariosQueryService = usuariosQueryService;
            this.toasterLite = toasterLite;
            this.$routeParams = $routeParams;
            this.loginQueryService = loginQueryService;
            this.$rootScope = $rootScope;
            this.config = config;
            // lista de organizaciones
            this.organizaciones = [];
            this.idUsuario = this.$routeParams['idUsuario'];
            if (this.idUsuario === undefined)
                this.idUsuario = this.loginQueryService.tryGetLocalLoginInfo().usuario;
            this.obtenerOrganizaciones();
        }
        organizacionesController.prototype.crearNuevaOrganizacion = function () {
            var _this = this;
            this.creandoOrg = true;
            this.usuariosService.crearNuevaOrganizacion(this.orgNombre, function (value) {
                _this.organizaciones.push(value.data);
                _this.toasterLite.success("La organización " + _this.orgNombre + " fue creada exitosamente");
                _this.creandoOrg = false;
            }, function (reason) {
                _this.toasterLite.error('Ocurrió un error inesperado al intentar crear la organización ' + _this.orgNombre);
                _this.creandoOrg = false;
            });
        };
        organizacionesController.prototype.agregarAOrganizacion = function ($event, org) {
            var _this = this;
            this.agregandoUsuario = true;
            this.usuariosService.agregarUsuarioALaOrganizacion(this.idUsuario, org.id, function (value) {
                // Actualizar la interfaz
                for (var i = 0; i < _this.organizaciones.length; i++) {
                    if (_this.organizaciones[i].id === org.id) {
                        _this.organizaciones[i].usuarioEsMiembro = true;
                        break;
                    }
                }
                _this.$rootScope.$broadcast(_this.config.eventIndex.usuarios.usuarioAgregadoAOrganizacion, {
                    idUsuario: _this.idUsuario,
                    org: org
                });
                _this.toasterLite.success("Usuario agregado exitosamente a " + org.display);
                _this.agregandoUsuario = false;
            }, function (reason) {
                _this.toasterLite.error('Hubo un error al incorporar el usuario a la organizacion', _this.toasterLite.delayForever);
                _this.agregandoUsuario = false;
            });
        };
        organizacionesController.prototype.removerDeLaOrganizacion = function ($event, org) {
            var _this = this;
            this.removiendoUsuario = true;
            this.usuariosService.removerUsuarioDeOrganizacion(this.idUsuario, org.id, new common.callbackLite(function (value) {
                // Actualizar la interfaz
                for (var i = 0; i < _this.organizaciones.length; i++) {
                    if (_this.organizaciones[i].id === org.id) {
                        _this.organizaciones[i].usuarioEsMiembro = false;
                        break;
                    }
                }
                _this.removiendoUsuario = false;
                _this.toasterLite.default('Usuario removido de la organización');
            }, function (reason) {
                _this.removiendoUsuario = false;
                _this.toasterLite.error('Hubo un error al intentar remover usuario de la organización');
            }));
        };
        //-------------------
        // INTERNAL
        //-------------------
        organizacionesController.prototype.obtenerOrganizaciones = function () {
            var _this = this;
            this.usuariosQueryService.obtenerOrganizacionesMarcadasDelUsuario(this.idUsuario, function (value) {
                _this.organizaciones = value.data;
                _this.loaded = true;
            }, function (reason) { return _this.toasterLite.error('Ocurrió un error al recuperar lista de organizaciones', _this.toasterLite.delayForever); });
        };
        return organizacionesController;
    }());
    organizacionesController.$inject = ['usuariosService', 'usuariosQueryService', 'toasterLite', '$routeParams', 'loginQueryService', '$rootScope',
        'config'];
    usuariosArea.organizacionesController = organizacionesController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=organizacionesController.js.map