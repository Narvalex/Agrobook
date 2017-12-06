/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var serviciosTabPrescripcionesController = /** @class */ (function () {
        function serviciosTabPrescripcionesController(config, $routeParams) {
            this.config = config;
            this.$routeParams = $routeParams;
            var idServicio = this.$routeParams['idServicio'];
            this.idColeccion = this.config.categoriaDeArchivos.servicioPrescripciones + "-" + idServicio;
        }
        serviciosTabPrescripcionesController.$inject = ['config', '$routeParams'];
        return serviciosTabPrescripcionesController;
    }());
    apArea.serviciosTabPrescripcionesController = serviciosTabPrescripcionesController;
})(apArea || (apArea = {}));
//# sourceMappingURL=serviciosTabPrescripcionesController.js.map