/// <reference path="../_all.ts" />

module apArea {
    angular.module('apArea', ['ngRoute', 'ngMaterial', 'ngMdIcons'])
        .value('config', new common.config())
        .service('toasterLite', common.toasterLite)
        .service('localStorageLite', common.localStorageLite)
        .service('loginService', login.loginService) // to log out...! do not remove this!
        .service('loginQueryService', login.loginQueryService)
        .service('apQueryService', apQueryService)
        .service('apService', apService)
        .service('fakeDb', fakeDb)
        .controller('sidenavController', sidenavController)
        .controller('bottomSheetButtonController', bottomSheetButtonController)
        .controller('bottomSheetController', bottomSheetController)
        .controller('toolbarHeaderController', toolbarHeaderController)
        .controller('userMenuWidgetController', common.userMenuWidgetController)
        .controller('mainContentController', mainContentController)
        // org controllers
        .controller('orgMainContentController', orgMainContentController)
        .controller('orgTabServiciosController', orgTabServiciosController)
        .controller('orgTabContratosController', orgTabContratosController)
        // prod controllers
        .controller('prodMainContentController', prodMainContentController)
        .controller('prodTabParcelasController', prodTabParcelasController)
        .controller('prodTabServiciosController', prodTabServiciosController)
        // servicios controllers
        .controller('serviciosMainContentController', serviciosMainContentController)
        .controller('serviciosTabResumenController', serviciosTabResumenController)
        // config
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
        }])
        .run(['$rootScope', (
            $rootScope: angular.IRootScopeService
        ) => {
            // Aqui se pueden definir DTOS compartidos. Algo asi como "variables globales"
        }]);
}