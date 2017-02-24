/// <reference path="../_all.ts" />
var MapsArea;
(function (MapsArea) {
    var ToolbarHeaderController = (function () {
        function ToolbarHeaderController($mdSidenav) {
            this.$mdSidenav = $mdSidenav;
        }
        ToolbarHeaderController.prototype.toggleSideNav = function () {
            this.$mdSidenav('left').toggle();
        };
        return ToolbarHeaderController;
    }());
    ToolbarHeaderController.$inject = ['$mdSidenav'];
    MapsArea.ToolbarHeaderController = ToolbarHeaderController;
})(MapsArea || (MapsArea = {}));
//# sourceMappingURL=toolbar-headerController.js.map