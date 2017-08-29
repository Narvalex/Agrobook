/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var prodTabServiciosController = (function () {
        function prodTabServiciosController(config, apService, apQueryService, toasterLite, $routeParams, $mdPanel) {
            this.config = config;
            this.apService = apService;
            this.apQueryService = apQueryService;
            this.toasterLite = toasterLite;
            this.$routeParams = $routeParams;
            this.$mdPanel = $mdPanel;
            this.idProd = this.$routeParams['idProd'];
        }
        // Listas
        // Api
        prodTabServiciosController.prototype.nuevoServicio = function () {
            window.location.replace("#!/servicios/" + this.idProd + "/new?tab=resumen&action=new");
        };
        return prodTabServiciosController;
    }());
    prodTabServiciosController.$inject = ['config', 'apService', 'apQueryService', 'toasterLite', '$routeParams', '$mdPanel'];
    apArea.prodTabServiciosController = prodTabServiciosController;
})(apArea || (apArea = {}));
//# sourceMappingURL=prodTabServiciosController.js.map