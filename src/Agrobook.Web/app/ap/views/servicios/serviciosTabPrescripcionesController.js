/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var serviciosTabPrescripcionesController = (function () {
        function serviciosTabPrescripcionesController(config, $routeParams) {
            this.config = config;
            this.$routeParams = $routeParams;
            var idServicio = this.$routeParams['idServicio'];
            this.idColeccion = this.config.categoriaDeArchivos.servicioPrescripciones + "-" + idServicio;
        }
        return serviciosTabPrescripcionesController;
    }());
    serviciosTabPrescripcionesController.$inject = ['config', '$routeParams'];
    apArea.serviciosTabPrescripcionesController = serviciosTabPrescripcionesController;
})(apArea || (apArea = {}));
//# sourceMappingURL=serviciosTabPrescripcionesController.js.map