/// <reference path="../_all.ts" />

module MapsArea {
    angular.module("mapsArea", ['ngMaterial', 'ngMdIcons'])
        .controller("toolbar-headerController", ToolbarHeaderController)
        .controller("main-contentController", MainContentController)
        .config((
            $mdIconProvider: angular.material.IIconProvider,
            $mdThemingProvider: angular.material.IThemingProvider) => {

            $mdIconProvider
                .defaultIconSet('../app/assets/svg/avatars.svg', 128)
                .icon('menu', '../app/assets/svg/menu.svg', 24);

            $mdThemingProvider.theme('default')
                .primaryPalette('green')
                .accentPalette('blue');
        });
}