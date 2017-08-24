/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var orgMainContentController = (function () {
        function orgMainContentController($routeParams, $scope, apQueryService) {
            var _this = this;
            this.$routeParams = $routeParams;
            this.$scope = $scope;
            this.apQueryService = apQueryService;
            var idOrg = this.$routeParams['idOrg'];
            this.recuperarOrg(idOrg);
            this.abrirTabCorrespondiente();
            this.$scope.$on('$routeUpdate', function (scope, next, current) {
                _this.abrirTabCorrespondiente();
            });
        }
        //--------------------------
        // Private
        //--------------------------
        orgMainContentController.prototype.onTabSelected = function (tabIndex) {
            var tabId;
            switch (tabIndex) {
                case 0:
                    tabId = "servicios";
                    break;
                case 1:
                    tabId = "contratos";
                    break;
                default:
                    tabId = "servicios";
                    break;
            }
            window.location.replace("#!/org/" + this.org.id + "?tab=" + tabId);
        };
        orgMainContentController.prototype.abrirTabCorrespondiente = function () {
            var tabId = this.$routeParams['tab'];
            switch (tabId) {
                case 'servicios':
                    this.tabIndex = 0;
                    break;
                case 'contratos':
                    this.tabIndex = 1;
                    break;
                default:
                    this.tabIndex = 0;
                    break;
            }
        };
        orgMainContentController.prototype.recuperarOrg = function (id) {
            var _this = this;
            this.apQueryService.getOrg(id, new common.callbackLite(function (value) {
                _this.org = value.data;
            }, function (reason) { }));
        };
        return orgMainContentController;
    }());
    orgMainContentController.$inject = ['$routeParams', '$scope', 'apQueryService'];
    apArea.orgMainContentController = orgMainContentController;
})(apArea || (apArea = {}));
//# sourceMappingURL=orgMainContentController.js.map