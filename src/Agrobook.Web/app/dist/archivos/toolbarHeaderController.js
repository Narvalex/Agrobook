/// <reference path="../_all.ts" />
var archivosArea;
(function (archivosArea) {
    var toolbarHeaderController = (function () {
        function toolbarHeaderController($mdSidenav) {
            this.$mdSidenav = $mdSidenav;
        }
        toolbarHeaderController.prototype.toggleSideNav = function () {
            this.$mdSidenav('left').toggle();
        };
        return toolbarHeaderController;
    }());
    toolbarHeaderController.$inject = ['$mdSidenav'];
    archivosArea.toolbarHeaderController = toolbarHeaderController;
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=toolbarHeaderController.js.map