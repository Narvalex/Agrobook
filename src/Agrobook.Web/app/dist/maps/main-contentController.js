/// <reference path="../_all.ts" />
var MapsArea;
(function (MapsArea) {
    var MainContentController = (function () {
        function MainContentController() {
            this.initMap();
        }
        MainContentController.prototype.initMap = function () {
            var map = new google.maps.Map(document.getElementById('map'), {
                zoom: 11,
                center: { lat: -25.34578, lng: -55.64516 }
            });
            var ctaLayer = new google.maps.KmlLayer({
                map: map,
                url: 'https://raw.githubusercontent.com/Narvalex/Agrobook/realease-v1.0.0/samples/sommer.kml'
            });
        };
        return MainContentController;
    }());
    MainContentController.$inject = [];
    MapsArea.MainContentController = MainContentController;
})(MapsArea || (MapsArea = {}));
//# sourceMappingURL=main-contentController.js.map