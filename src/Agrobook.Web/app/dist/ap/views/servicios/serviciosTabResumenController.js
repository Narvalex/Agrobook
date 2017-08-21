/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var serviciosTabResumenController = (function () {
        function serviciosTabResumenController(config, apService, apQueryService, toasterLite, $routeParams, $mdPanel) {
            this.config = config;
            this.apService = apService;
            this.apQueryService = apQueryService;
            this.toasterLite = toasterLite;
            this.$routeParams = $routeParams;
            this.$mdPanel = $mdPanel;
            this.idProd = this.$routeParams['idProd'];
            this.esNuevo = true;
        }
        // Listas
        // Api
        serviciosTabResumenController.prototype.cancelar = function () {
            window.location.replace("#!/prod/" + this.idProd);
        };
        return serviciosTabResumenController;
    }());
    serviciosTabResumenController.$inject = ['config', 'apService', 'apQueryService', 'toasterLite', '$routeParams', '$mdPanel'];
    apArea.serviciosTabResumenController = serviciosTabResumenController;
})(apArea || (apArea = {}));
//# sourceMappingURL=serviciosTabResumenController.js.map