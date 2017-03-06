/// <reference path="_all.ts" />

module Home {
    angular.module("app", ['ngMaterial', 'ngMdIcons'])
        .service('loginWriteService', login.loginService)
        .controller('toolbar-headerController', ToolbarHeaderController)
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