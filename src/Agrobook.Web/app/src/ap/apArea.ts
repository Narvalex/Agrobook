/// <reference path="../_all.ts" />

module apArea {
    angular.module('apArea', ['ngRoute', 'ngMaterial', 'ngMdIcons'])
        .value('config', new common.config())
        .service('localStorageLite', common.localStorageLite)
        .service('loginService', login.loginService) // to log out...! do not remove this!
        .service('loginQueryService', login.loginQueryService)
        .service('apQueryService', apQueryService)
        .controller('sidenavController', sidenavController)
        .controller('toolbarHeaderController', toolbarHeaderController)
        .controller('userMenuWidgetController', common.userMenuWidgetController)
        .controller('mainContentController', mainContentController)
        .config(['$mdIconProvider', '$mdThemingProvider', '$httpProvider', '$routeProvider', (
            $mdIconProvider: angular.material.IIconProvider,
            $mdThemingProvider: angular.material.IThemingProvider,
            $httpProvider: angular.IHttpProvider,
            $routeProvider: angular.route.IRouteProvider
        ) => {
            // most from flat icon dot com
            $mdIconProvider
                .defaultIconSet('./assets/svg/avatars.svg', 128)
                .icon('menu', './assets/svg/menu.svg', 24)
                .icon('close', './assets/svg/close.svg')
                .icon('agrobook-white', './assets/svg/agrobook-white.svg')
                .icon('agrobook-green', './assets/svg/agrobook-green.svg');

            $mdThemingProvider.theme('default')
                .primaryPalette('green')
                .accentPalette('blue');

            common.registerHttpInterceptors($httpProvider);

            getRouteConfigs().forEach(config => {
                $routeProvider.when(config.path, config.route);
            });
            $routeProvider.otherwise({ redirectTo: '/' });
        }]);
}