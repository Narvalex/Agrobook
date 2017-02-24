/// <reference path="../_all.ts" />
var Home;
(function (Home) {
    var ToolbarHeaderController = (function () {
        function ToolbarHeaderController() {
        }
        ToolbarHeaderController.prototype.login = function () {
            location.href = "areas/maps.html";
        };
        return ToolbarHeaderController;
    }());
    ToolbarHeaderController.$inject = [];
    Home.ToolbarHeaderController = ToolbarHeaderController;
})(Home || (Home = {}));
//# sourceMappingURL=toolbar-headerController.js.map