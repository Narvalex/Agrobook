/// <reference path="../_all.ts" />
var MapsArea;
(function (MapsArea) {
    var MainContentController = (function () {
        function MainContentController() {
            this.initMap();
        }
        MainContentController.prototype.initMap = function () {
            var map = new google.maps.Map(document.getElementById('map'), {
                zoom: 15,
                center: { lat: -25.4486705, lng: -55.6474731 }
            });
            var ctaLayer = new google.maps.KmlLayer({
                map: map,
                url: 'https://raw.githubusercontent.com/Narvalex/Agrobook/realease-v1.0.0/samples/david.kmz'
            });
        };
        return MainContentController;
    }());
    MainContentController.$inject = [];
    MapsArea.MainContentController = MainContentController;
})(MapsArea || (MapsArea = {}));
//# sourceMappingURL=main-contentController.js.map