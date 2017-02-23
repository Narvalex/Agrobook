/// <reference path="../_all.ts" />
var geoArea;
(function (geoArea) {
    angular.module("geoArea", ['ngMaterial', 'ngMdIcons'])
        .config(function ($mdIconProvider, $mdThemingProvider) {
        $mdIconProvider
            .defaultIconSet('./app/assets/svg/avatars.svg', 128)
            .icon('menu', './app/assets/svg/menu.svg', 24);
        $mdThemingProvider.theme('default')
            .primaryPalette('green')
            .accentPalette('blue');
    });
})(geoArea || (geoArea = {}));
//# sourceMappingURL=geoArea.js.map