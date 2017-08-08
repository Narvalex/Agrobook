/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var orgTabServiciosController = (function () {
        function orgTabServiciosController($routeParams, apQueryService) {
            this.$routeParams = $routeParams;
            this.apQueryService = apQueryService;
            var idOrg = this.$routeParams['idOrg'];
            this.recuperarServiciosPorOrg(idOrg);
        }
        //--------------------------
        // Private
        //--------------------------
        orgTabServiciosController.prototype.recuperarServiciosPorOrg = function (idOrg) {
            var _this = this;
            this.apQueryService.getServiciosPorOrg(idOrg, new common.callbackLite(function (value) {
                _this.servicios = value.data;
            }, function (reason) { }));
        };
        return orgTabServiciosController;
    }());
    orgTabServiciosController.$inject = ['$routeParams', 'apQueryService'];
    apArea.orgTabServiciosController = orgTabServiciosController;
})(apArea || (apArea = {}));
//# sourceMappingURL=orgTabServiciosController.js.map