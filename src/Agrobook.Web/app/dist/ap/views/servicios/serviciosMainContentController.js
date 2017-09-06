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
            this.servicio = new apArea.servicioDto(this.$routeParams['idServicio'], null, null, null, null, null, null);
            this.resolveAction();
            this.recuperarProductorYResolverTitulo();
            this.abrirTabCorrespondiente();
            this.$scope.$on('$routeUpdate', function (scope, next, current) {
                _this.resolveAction();
                _this.abrirTabCorrespondiente();
            });
            this.$scope.$on(this.config.eventIndex.ap_servicios.nuevoServicioCreado, function (e, args) {
                _this.esNuevo = false;
                _this.action = 'view';
                _this.servicio = args;
            });
            this.$scope.$on(this.config.eventIndex.ap_servicios.cambioDeParcelaEnServicio, function (e, parcelaDisplay) {
                _this.servicio.parcelaDisplay = parcelaDisplay;
            });
        }
        //--------------------------
        // Private
        //--------------------------
        serviciosMainContentController.prototype.resolveAction = function () {
            var action = this.$routeParams['action'];
            this.action = action === undefined ? 'view' : action;
            this.esNuevo = this.action === 'new';
        };
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
            window.location.replace("#!/servicios/" + this.idProd + "/" + this.servicio.id + "?tab=" + tabId + "&action=" + this.action);
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
            }, function (reason) { }));
        };
        return serviciosMainContentController;
    }());
    serviciosMainContentController.$inject = ['$routeParams', '$scope', 'apQueryService', 'config'];
    apArea.serviciosMainContentController = serviciosMainContentController;
})(apArea || (apArea = {}));
//# sourceMappingURL=serviciosMainContentController.js.map