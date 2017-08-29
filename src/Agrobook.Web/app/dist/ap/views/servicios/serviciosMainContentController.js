/// <reference path="../../../_all.ts" />
var apArea;
(function (apArea) {
    var serviciosMainContentController = (function () {
        function serviciosMainContentController($routeParams, $scope, apQueryService, config) {
            var _this = this;
            this.$routeParams = $routeParams;
            this.$scope = $scope;
            this.apQueryService = apQueryService;
            this.config = config;
            this.idProd = this.$routeParams['idProd'];
            this.idServicio = this.$routeParams['idServicio'];
            var action = this.$routeParams['action'];
            this.action = action === undefined ? 'view' : action;
            this.esNuevo = this.action === 'new';
            this.recuperarProductorYResolverTitulo();
            this.abrirTabCorrespondiente();
            this.$scope.$on('$routeUpdate', function (scope, next, current) {
                _this.abrirTabCorrespondiente();
            });
        }
        //--------------------------
        // Private
        //--------------------------
        serviciosMainContentController.prototype.onTabSelected = function (tabIndex) {
            var tabId;
            switch (tabIndex) {
                case 0:
                    tabId = "resumen";
                    break;
                case 1:
                    tabId = "parcela";
                    break;
                case 2:
                    tabId = "diagnostico";
                    break;
                case 3:
                    tabId = "prescripciones";
                    break;
                default:
                    tabId = "resumen";
                    break;
            }
            window.location.replace("#!/servicios/" + this.idProd + "/" + this.idServicio + "?tab=" + tabId + "&action=" + this.action);
        };
        serviciosMainContentController.prototype.abrirTabCorrespondiente = function () {
            var tabId = this.$routeParams['tab'];
            switch (tabId) {
                case 'resumen':
                    this.tabIndex = 0;
                    break;
                case 'parcela':
                    this.tabIndex = 1;
                    break;
                case 'diagnostico':
                    this.tabIndex = 2;
                    break;
                case 'prescripciones':
                    this.tabIndex = 3;
                    break;
                default:
                    this.tabIndex = 0;
                    break;
            }
        };
        serviciosMainContentController.prototype.recuperarProductorYResolverTitulo = function () {
            var _this = this;
            this.apQueryService.getProd(this.idProd, new common.callbackLite(function (value) {
                _this.productor = value.data;
                _this.resolverTitulo();
            }, function (reason) { }));
        };
        serviciosMainContentController.prototype.resolverTitulo = function () {
            switch (this.action) {
                case 'new':
                    this.title = " Nuevo servicio para " + this.productor.display;
                    break;
                case 'edit':
                    break;
                case 'view':
                    this.title = "Servicio " + 'place servicio here';
                    break;
            }
        };
        return serviciosMainContentController;
    }());
    serviciosMainContentController.$inject = ['$routeParams', '$scope', 'apQueryService', 'config'];
    apArea.serviciosMainContentController = serviciosMainContentController;
})(apArea || (apArea = {}));
//# sourceMappingURL=serviciosMainContentController.js.map