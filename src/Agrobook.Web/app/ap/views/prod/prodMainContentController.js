/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var prodMainContentController = (function () {
        function prodMainContentController($routeParams, $scope, apQueryService) {
            var _this = this;
            this.$routeParams = $routeParams;
            this.$scope = $scope;
            this.apQueryService = apQueryService;
            this.idProd = this.$routeParams['idProd'];
            this.recuperarProd(this.idProd);
            this.abrirTabCorrespondiente();
            this.$scope.$on('$routeUpdate', function (scope, next, current) {
                _this.abrirTabCorrespondiente();
            });
        }
        // API
        prodMainContentController.prototype.irAOrg = function (org) {
            window.location.replace("#!/org/" + org.id);
        };
        //--------------------------
        // Private
        //--------------------------
        prodMainContentController.prototype.onTabSelected = function (tabIndex) {
            var tabId;
            switch (tabIndex) {
                case 0:
                    tabId = "servicios";
                    break;
                case 1:
                    tabId = "parcelas";
                    break;
                default:
                    tabId = "servicios";
                    break;
            }
            window.location.replace("#!/prod/" + this.idProd + "?tab=" + tabId);
        };
        prodMainContentController.prototype.abrirTabCorrespondiente = function () {
            var tabId = this.$routeParams['tab'];
            switch (tabId) {
                case 'servicios':
                    this.tabIndex = 0;
                    break;
                case 'parcelas':
                    this.tabIndex = 1;
                    break;
                default:
                    this.tabIndex = 0;
                    break;
            }
        };
        prodMainContentController.prototype.recuperarProd = function (id) {
            var _this = this;
            this.apQueryService.getProd(id, new common.callbackLite(function (value) {
                _this.prod = value.data;
            }, function (reason) { }));
        };
        return prodMainContentController;
    }());
    prodMainContentController.$inject = ['$routeParams', '$scope', 'apQueryService'];
    apArea.prodMainContentController = prodMainContentController;
})(apArea || (apArea = {}));
//# sourceMappingURL=prodMainContentController.js.map