/// <reference path="../_all.ts" />
var MapsArea;
(function (MapsArea) {
    angular.module("mapsArea", ['ngMaterial', 'ngMdIcons'])
        .controller("toolbar-headerController", MapsArea.ToolbarHeaderController)
        .controller("main-contentController", MapsArea.MainContentController)
        .config(function ($mdIconProvider, $mdThemingProvider) {
        $mdIconProvider
            .defaultIconSet('../app/assets/svg/avatars.svg', 128)
            .icon('menu', '../app/assets/svg/menu.svg', 24);
        $mdThemingProvider.theme('default')
            .primaryPalette('green')
            .accentPalette('blue');
    });
})(MapsArea || (MapsArea = {}));
//# sourceMappingURL=mapsArea.js.map