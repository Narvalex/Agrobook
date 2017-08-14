/// <reference path="../_all.ts" />
var apArea;
(function (apArea) {
    angular.module('apArea', ['ngRoute', 'ngMaterial', 'ngMdIcons'])
        .value('config', new common.config())
        .service('toasterLite', common.toasterLite)
        .service('localStorageLite', common.localStorageLite)
        .service('loginService', login.loginService) // to log out...! do not remove this!
        .service('loginQueryService', login.loginQueryService)
        .service('apQueryService', apArea.apQueryService)
        .service('apService', apArea.apService)
        .service('fakeDb', apArea.fakeDb)
        .controller('sidenavController', apArea.sidenavController)
        .controller('bottomSheetButtonController', apArea.bottomSheetButtonController)
        .controller('bottomSheetController', apArea.bottomSheetController)
        .controller('toolbarHeaderController', apArea.toolbarHeaderController)
        .controller('userMenuWidgetController', common.userMenuWidgetController)
        .controller('mainContentController', apArea.mainContentController)
        .controller('orgMainContentController', apArea.orgMainContentController)
        .controller('orgTabServiciosController', apArea.orgTabServiciosController)
        .controller('prodMainContentController', apArea.prodMainContentController)
        .controller('prodTabParcelasController', apArea.prodTabParcelasController)
        .config(['$mdIconProvider', '$mdThemingProvider', '$httpProvider', '$routeProvider', function ($mdIconProvider, $mdThemingProvider, $httpProvider, $routeProvider) {
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
            apArea.getRouteConfigs().forEach(function (config) {
                $routeProvider.when(config.path, config.route);
            });
            $routeProvider.otherwise({ redirectTo: '/' });
        }])
        .run(['$rootScope', function ($rootScope) {
            // Aqui se pueden definir DTOS compartidos. Algo asi como "variables globales"
        }]);
})(apArea || (apArea = {}));
//# sourceMappingURL=apArea.js.map