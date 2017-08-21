/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var orgTabContratosController = (function () {
        function orgTabContratosController($routeParams, apQueryService) {
            this.$routeParams = $routeParams;
            this.apQueryService = apQueryService;
            this.idOrg = this.$routeParams['idOrg'];
            this.recuperarContratos();
        }
        //--------------------------
        // Private
        //--------------------------
        orgTabContratosController.prototype.recuperarContratos = function () {
            var _this = this;
            this.apQueryService.getContratos(this.idOrg, new common.callbackLite(function (value) {
                _this.contratos = value.data;
            }, function (reason) { }));
        };
        return orgTabContratosController;
    }());
    orgTabContratosController.$inject = ['$routeParams', 'apQueryService'];
    apArea.orgTabContratosController = orgTabContratosController;
})(apArea || (apArea = {}));
//# sourceMappingURL=orgTabContratosController.js.map