/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var serviciosTabPrescripcionesController = (function () {
        function serviciosTabPrescripcionesController(config, $routeParams) {
            this.config = config;
            this.$routeParams = $routeParams;
            var idProd = this.$routeParams['idProd'];
            this.idColeccion = this.config.categoriaDeArchivos.servicioPrescripciones + "-" + idProd;
        }
        return serviciosTabPrescripcionesController;
    }());
    serviciosTabPrescripcionesController.$inject = ['config', '$routeParams'];
    apArea.serviciosTabPrescripcionesController = serviciosTabPrescripcionesController;
})(apArea || (apArea = {}));
//# sourceMappingURL=serviciosTabPrescripcionesController.js.map