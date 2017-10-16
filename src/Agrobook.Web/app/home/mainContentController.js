/// <reference path="../_all.ts" />
var homeArea;
(function (homeArea) {
    var mainContentController = (function () {
        function mainContentController() {
            var date = new Date();
            this.currentYear = date.getFullYear();
        }
        mainContentController.prototype.goTo = function (url) {
            //  this is the right way, in other to be able to go back 
            window.location.href = url;
        };
        return mainContentController;
    }());
    mainContentController.$inject = [];
    homeArea.mainContentController = mainContentController;
})(homeArea || (homeArea = {}));
//# sourceMappingURL=mainContentController.js.map