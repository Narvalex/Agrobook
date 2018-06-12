/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var prodMainContentController = (function () {
        function prodMainContentController($routeParams, $scope, apQueryService, config, loginService, toasterLite) {
            var _this = this;
            this.$routeParams = $routeParams;
            this.$scope = $scope;
            this.apQueryService = apQueryService;
            this.config = config;
            this.loginService = loginService;
            this.toasterLite = toasterLite;
            this.idProd = this.$routeParams['idProd'];
            this.recuperarProd(this.idProd);
            this.puedeIrAOrg = this.loginService.autorizar([config.claims.roles.Gerente, config.claims.roles.Tecnico]);
            this.abrirTabCorrespondiente();
            this.$scope.$on('$routeUpdate', function (scope, next, current) {
                _this.abrirTabCorrespondiente();
            });
        }
        // API
        prodMainContentController.prototype.irAOrg = function (org) {
            if (this.puedeIrAOrg)
                window.location.href = "#!/org/" + org.id;
            else
                this.toasterLite.info('Esta es la cooperativa a la cual pertence: ' + org.display);
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
            window.location.href = "#!/prod/" + this.idProd + "?tab=" + tabId;
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
    prodMainContentController.$inject = ['$routeParams', '$scope', 'apQueryService', 'config', 'loginService', 'toasterLite'];
    apArea.prodMainContentController = prodMainContentController;
})(apArea || (apArea = {}));
//# sourceMappingURL=prodMainContentController.js.map