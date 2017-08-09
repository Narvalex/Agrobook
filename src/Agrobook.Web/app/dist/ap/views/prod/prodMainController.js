/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var prodMainContentController = (function () {
        function prodMainContentController($routeParams, apQueryService) {
            this.$routeParams = $routeParams;
            this.apQueryService = apQueryService;
            var idProd = this.$routeParams['idProd'];
            this.recuperarProd(idProd);
        }
        //--------------------------
        // Private
        //--------------------------
        prodMainContentController.prototype.recuperarProd = function (id) {
            var _this = this;
            this.apQueryService.getOrg(id, new common.callbackLite(function (value) {
                _this.prod = value.data;
            }, function (reason) { }));
        };
        return prodMainContentController;
    }());
    prodMainContentController.$inject = ['$routeParams', 'apQueryService'];
    apArea.prodMainContentController = prodMainContentController;
})(apArea || (apArea = {}));
//# sourceMappingURL=prodMainController.js.map