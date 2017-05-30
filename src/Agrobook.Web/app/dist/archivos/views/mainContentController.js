/// <reference path="../../_all.ts" />
var archivosArea;
(function (archivosArea) {
    var mainContentController = (function () {
        function mainContentController($mdSidenav) {
            this.$mdSidenav = $mdSidenav;
        }
        mainContentController.prototype.toggleSideNav = function () {
            this.$mdSidenav('right').toggle();
        };
        mainContentController.prototype.isSideNavOpen = function () {
            var open = this.$mdSidenav('right').isOpen();
            return open;
        };
        return mainContentController;
    }());
    mainContentController.$inject = ['$mdSidenav'];
    archivosArea.mainContentController = mainContentController;
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=mainContentController.js.map