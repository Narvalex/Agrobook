/// <reference path="../_all.ts" />

module MapsArea {
    export class MainContentController {
        static $inject = [];

        constructor() {
            this.initMap();
        }

        initMap(): void {
            var map = new google.maps.Map(document.getElementById('map'), {
                zoom: 20,
                center: { lat: -25.42983, lng: -55.63346 }
            });

            var ctaLayer = new google.maps.KmlLayer({
                map: map,
                url: 'https://raw.githubusercontent.com/Narvalex/Agrobook/realease-v1.0.0/samples/big.kml'
            });
        }
    }
}