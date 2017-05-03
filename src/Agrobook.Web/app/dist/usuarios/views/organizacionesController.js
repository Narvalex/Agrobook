/// <reference path="../../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var organizacionesController = (function () {
        function organizacionesController(usuariosService, toasterLite) {
            this.usuariosService = usuariosService;
            this.toasterLite = toasterLite;
            //...
        }
        organizacionesController.prototype.crearNuevaOrganizacion = function () {
            var _this = this;
            this.usuariosService.crearNuevaOrganizacion(this.orgNombre, function (value) { return _this.toasterLite.success("La organización " + _this.orgNombre + " fue creada exitosamente"); }, function (reason) { return _this.toasterLite.error('Ocurrió un error inesperado al intentar crear la organización ' + _this.orgNombre); });
        };
        return organizacionesController;
    }());
    organizacionesController.$inject = ['usuariosService', 'toasterLite'];
    usuariosArea.organizacionesController = organizacionesController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=organizacionesController.js.map