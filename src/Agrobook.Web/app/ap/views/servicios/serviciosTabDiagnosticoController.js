/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var serviciosTabDiagnosticoController = /** @class */ (function () {
        function serviciosTabDiagnosticoController(config, $routeParams) {
            this.config = config;
            this.$routeParams = $routeParams;
            var idServicio = this.$routeParams['idServicio'];
            this.idColeccion = this.config.categoriaDeArchivos.servicioDiagnostico + "-" + idServicio;
        }
        serviciosTabDiagnosticoController.$inject = ['config', '$routeParams'];
        return serviciosTabDiagnosticoController;
    }());
    apArea.serviciosTabDiagnosticoController = serviciosTabDiagnosticoController;
})(apArea || (apArea = {}));
//# sourceMappingURL=serviciosTabDiagnosticoController.js.map