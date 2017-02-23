/// <reference path="../_all.ts" />

module geoArea {
    angular.module("geoArea", ['ngMaterial', 'ngMdIcons'])
        .config((
            $mdIconProvider: angular.material.IIconProvider,
            $mdThemingProvider: angular.material.IThemingProvider) => {

            $mdIconProvider
                .defaultIconSet('./app/assets/svg/avatars.svg', 128)
                .icon('menu', './app/assets/svg/menu.svg', 24);

            $mdThemingProvider.theme('default')
                .primaryPalette('green')
                .accentPalette('blue');
        });
}