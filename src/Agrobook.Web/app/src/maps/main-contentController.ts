/// <reference path="../_all.ts" />

module MapsArea {
    export class MainContentController {
        static $inject = [];

        constructor() {
            this.initMap();
        }

        initMap(): void {
            var map = new google.maps.Map(document.getElementById('map'), {
                zoom: 11,
                center: { lat: -25.34578, lng: -55.64516 }
            });

            var ctaLayer = new google.maps.KmlLayer({
                map: map,
                url: 'efasdf'
            });
        }
    }
}