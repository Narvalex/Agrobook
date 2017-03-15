/// <reference path="../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
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
    usuariosArea.toolbarHeaderController = toolbarHeaderController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=toolbarHeaderController.js.map