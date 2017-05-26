/// <reference path="../_all.ts" />
var archivosArea;
(function (archivosArea) {
    var prodSidenavController = (function () {
        function prodSidenavController($mdSidenav) {
            this.$mdSidenav = $mdSidenav;
        }
        prodSidenavController.prototype.toggleSideNav = function () {
            this.$mdSidenav('left').toggle();
        };
        return prodSidenavController;
    }());
    prodSidenavController.$inject = ['$mdSidenav'];
    archivosArea.prodSidenavController = prodSidenavController;
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=prodSidenavController.js.map