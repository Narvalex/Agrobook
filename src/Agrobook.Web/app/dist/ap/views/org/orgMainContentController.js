/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var orgMainContentController = (function () {
        function orgMainContentController($routeParams, apQueryService) {
            this.$routeParams = $routeParams;
            this.apQueryService = apQueryService;
            var idOrg = this.$routeParams['idOrg'];
            this.recuperarOrg(idOrg);
        }
        //--------------------------
        // Private
        //--------------------------
        orgMainContentController.prototype.recuperarOrg = function (id) {
            var _this = this;
            this.apQueryService.getOrg(id, new common.callbackLite(function (value) {
                _this.org = value.data;
            }, function (reason) { }));
        };
        return orgMainContentController;
    }());
    orgMainContentController.$inject = ['$routeParams', 'apQueryService'];
    apArea.orgMainContentController = orgMainContentController;
})(apArea || (apArea = {}));
//# sourceMappingURL=orgMainContentController.js.map