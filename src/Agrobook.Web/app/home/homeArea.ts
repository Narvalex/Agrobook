/// <reference path="../_all.ts" />

module homeArea {
    angular.module("app", ['ngMaterial', 'ngMdIcons'])
        .value('config', new common.config())
        .service('localStorageLite', common.localStorageLite)
        .service('httpLite', common.httpLite)
        .service('loginService', login.loginService)
        .service('loginQueryService', login.loginQueryService)
        .controller('mainContentController', mainContentController)
        .controller('toolbar-headerController', ToolbarHeaderController)
        .controller('userMenuWidgetController', common.userMenuWidgetController)
        .config(['$mdIconProvider', '$mdThemingProvider', '$httpProvider', '$injector', (
            $mdIconProvider: angular.material.IIconProvider,
            $mdThemingProvider: angular.material.IThemingProvider,
            $httpProvider: angular.IHttpProvider,
            $injector: angular.auto.IInjectorService
        ) => {

            // iconarchive, flaticon
            // Sistem Icon http://www.flaticon.com/free-icon/tree-leaf_3923#term=leaf&page=1&position=11
            // Agrobook icon https://www.flaticon.com/free-icon/tree-leaf_3923
            $mdIconProvider
                .defaultIconSet('../assets/svg/avatars.svg', 128)
                .icon('agrobook-white', '../assets/svg/agrobook-white.svg')
                .icon('agrobook-green', '../assets/svg/agrobook-green.svg')
                .icon('menu', '../assets/svg/menu.svg', 24);

            $mdThemingProvider.theme('default')
                .primaryPalette('green')
                .accentPalette('blue');

            common.registerHttpInterceptors($httpProvider);            
        }]);
}