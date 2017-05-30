/// <reference path="../_all.ts" />
var archivosArea;
(function (archivosArea) {
    var sidenavController = (function () {
        function sidenavController($mdSidenav, toasterLite, $rootScope, config) {
            var _this = this;
            this.$mdSidenav = $mdSidenav;
            this.toasterLite = toasterLite;
            this.$rootScope = $rootScope;
            this.config = config;
            this.$rootScope.$on(this.config.eventIndex.archivos.productorSeleccionado, function (e, args) {
                _this.idProductor = args;
            });
        }
        sidenavController.prototype.toggleSideNav = function () {
            this.$mdSidenav('left').toggle();
        };
        sidenavController.prototype.nuevoArchivo = function () {
            this.toasterLite.info("nuevo archivo para " + this.idProductor + "!");
        };
        return sidenavController;
    }());
    sidenavController.$inject = ['$mdSidenav', 'toasterLite', '$rootScope', 'config'];
    archivosArea.sidenavController = sidenavController;
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=sidenavController.js.map