/// <reference path="../../_all.ts" />
var archivosArea;
(function (archivosArea) {
    var mainContentController = (function () {
        function mainContentController($mdSidenav, $rooteScope, $routeParams, config) {
            this.$mdSidenav = $mdSidenav;
            this.$rooteScope = $rooteScope;
            this.$routeParams = $routeParams;
            this.config = config;
            this.setearElProductorEnTodosLados();
        }
        mainContentController.prototype.toggleSideNav = function () {
            this.$mdSidenav('right').toggle();
        };
        mainContentController.prototype.isSideNavOpen = function () {
            var open = this.$mdSidenav('right').isOpen();
            return open;
        };
        mainContentController.prototype.setearElProductorEnTodosLados = function () {
            /*
            Esta es la unica forma de hacer, por que capturando el evento routeChanged solo se puede hacer dentro de los
            controles ng-view
            */
            this.idProductor = this.$routeParams['idProductor'];
            this.$rooteScope.$broadcast(this.config.eventIndex.archivos.productorSeleccionado, this.idProductor);
        };
        return mainContentController;
    }());
    mainContentController.$inject = ['$mdSidenav', '$rootScope', '$routeParams', 'config'];
    archivosArea.mainContentController = mainContentController;
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=mainContentController.js.map