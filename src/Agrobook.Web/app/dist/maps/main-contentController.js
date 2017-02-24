/// <reference path="../_all.ts" />
var MapsArea;
(function (MapsArea) {
    var MainContentController = (function () {
        function MainContentController() {
            this.initMap();
        }
        MainContentController.prototype.initMap = function () {
            var map = new google.maps.Map(document.getElementById('map'), {
                center: { lat: -34.397, lng: 150.644 },
                zoom: 8
            });
        };
        return MainContentController;
    }());
    MainContentController.$inject = [];
    MapsArea.MainContentController = MainContentController;
})(MapsArea || (MapsArea = {}));
//# sourceMappingURL=main-contentController.js.map