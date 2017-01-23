/// <reference path="_all.ts" />

module ContactManagerApp {
    angular.module("contactManagerApp", ['ngMaterial', 'ngMdIcons'])
        .service('userService', UserService)
        .controller("mainController", MainController)
        .config((
            $mdIconProvider: angular.material.IIconProvider,
            $mdThemingProvider: angular.material.IThemingProvider) => {

            $mdIconProvider.icon('menu', './assets/svg/menu.svg', 24);

            $mdThemingProvider.theme('default')
                .primaryPalette('blue')
                .accentPalette('red');
        });
}