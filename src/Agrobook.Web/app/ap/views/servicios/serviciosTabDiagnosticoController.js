/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var serviciosTabDiagnosticoController = (function () {
        function serviciosTabDiagnosticoController(config, $routeParams) {
            this.config = config;
            this.$routeParams = $routeParams;
            var idServicio = this.$routeParams['idServicio'];
            this.idColeccion = this.config.categoriaDeArchivos.servicioDiagnostico + "-" + idServicio;
        }
        return serviciosTabDiagnosticoController;
    }());
    serviciosTabDiagnosticoController.$inject = ['config', '$routeParams'];
    apArea.serviciosTabDiagnosticoController = serviciosTabDiagnosticoController;
})(apArea || (apArea = {}));
//# sourceMappingURL=serviciosTabDiagnosticoController.js.map