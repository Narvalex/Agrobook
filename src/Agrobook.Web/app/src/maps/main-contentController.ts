/// <reference path="../_all.ts" />

module MapsArea {
    export class MainContentController {
        static $inject = [];

        constructor() {
            this.initMap();
        }

        initMap(): void {
            var map = new google.maps.Map(document.getElementById('map'), {
                zoom: 15,
                center: { lat: -25.4486705, lng: -55.6474731 }
            });

            var ctaLayer = new google.maps.KmlLayer({
                map: map,
                url: 'https://raw.githubusercontent.com/Narvalex/Agrobook/realease-v1.0.0/samples/david.kmz'
            });
        }
    }
}