/// <reference path="../../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var organizacionesController = (function () {
        function organizacionesController(usuariosService, usuariosQueryService, toasterLite) {
            this.usuariosService = usuariosService;
            this.usuariosQueryService = usuariosQueryService;
            this.toasterLite = toasterLite;
            // lista de organizaciones
            this.organizaciones = [];
            this.obtenerOrganizaciones();
        }
        organizacionesController.prototype.crearNuevaOrganizacion = function () {
            var _this = this;
            this.usuariosService.crearNuevaOrganizacion(this.orgNombre, function (value) { return _this.toasterLite.success("La organización " + _this.orgNombre + " fue creada exitosamente"); }, function (reason) { return _this.toasterLite.error('Ocurrió un error inesperado al intentar crear la organización ' + _this.orgNombre); });
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
    organizacionesController.$inject = ['usuariosService', 'usuariosQueryService', 'toasterLite'];
    usuariosArea.organizacionesController = organizacionesController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=organizacionesController.js.map