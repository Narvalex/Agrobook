/// <reference path="../../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var organizacionesController = (function () {
        function organizacionesController(usuariosService, usuariosQueryService, toasterLite, $routeParams) {
            this.usuariosService = usuariosService;
            this.usuariosQueryService = usuariosQueryService;
            this.toasterLite = toasterLite;
            this.$routeParams = $routeParams;
            // lista de organizaciones
            this.organizaciones = [];
            this.idUsuario = this.$routeParams['idUsuario'];
            this.obtenerOrganizaciones();
        }
        organizacionesController.prototype.crearNuevaOrganizacion = function () {
            var _this = this;
            this.usuariosService.crearNuevaOrganizacion(this.orgNombre, function (value) { return _this.toasterLite.success("La organización " + _this.orgNombre + " fue creada exitosamente"); }, function (reason) { return _this.toasterLite.error('Ocurrió un error inesperado al intentar crear la organización ' + _this.orgNombre); });
        };
        organizacionesController.prototype.agregarAOrganizacion = function ($event, org) {
            var _this = this;
            this.usuariosService.agregarUsuarioALaOrganizacion(this.idUsuario, org.id, function (value) {
                _this.toasterLite.success("Usuario agregado exitosamente a " + org.display);
            }, function (reason) { return _this.toasterLite.error('Hubo un error al incorporar el usuario a la organizacion', _this.toasterLite.delayForever); });
        };
        //-------------------
        // INTERNAL
        //-------------------
        organizacionesController.prototype.obtenerOrganizaciones = function () {
            var _this = this;
            this.usuariosQueryService.obtenerOrganizaciones(function (value) {
                _this.organizaciones = value.data;
                _this.loaded = true;
            }, function (reason) { return _this.toasterLite.error('Ocurrió un error al recuperar lista de organizaciones', _this.toasterLite.delayForever); });
        };
        return organizacionesController;
    }());
    organizacionesController.$inject = ['usuariosService', 'usuariosQueryService', 'toasterLite', '$routeParams'];
    usuariosArea.organizacionesController = organizacionesController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=organizacionesController.js.map