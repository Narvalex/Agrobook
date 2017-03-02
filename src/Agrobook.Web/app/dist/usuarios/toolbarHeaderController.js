/// <reference path="../_all.ts" />
var UsuariosArea;
(function (UsuariosArea) {
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
    UsuariosArea.toolbarHeaderController = toolbarHeaderController;
})(UsuariosArea || (UsuariosArea = {}));
//# sourceMappingURL=toolbarHeaderController.js.map