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
                center: { lat: -25.43108, lng: -55.63441 }
            });
            var ctaLayer = new google.maps.KmlLayer({
                map: map,
                url: 'https://github.com/Narvalex/Agrobook/blob/realease-v1.0.0/samples/sommer.kmz'
            });
        };
        return MainContentController;
    }());
    MainContentController.$inject = [];
    MapsArea.MainContentController = MainContentController;
})(MapsArea || (MapsArea = {}));
//# sourceMappingURL=main-contentController.js.map