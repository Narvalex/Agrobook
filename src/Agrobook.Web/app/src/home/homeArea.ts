/// <reference path="../_all.ts" />

module HomeArea {
    angular.module("app", ['ngMaterial', 'ngMdIcons'])
        .value('config', new common.config())
        .service('localStorageLite', common.localStorageLite)
        .service('loginService', login.loginService)
        .service('loginQueryService', login.loginQueryService)
        .controller('toolbar-headerController', ToolbarHeaderController)
        .controller('userMenuWidgetController', common.userMenuWidgetController)
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