/// <reference path="../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    angular.module('usuariosArea', ['ngRoute', 'ngMaterial', 'ngMdIcons'])
        .value('config', new common.config())
        .service('localStorageLite', common.localStorageLite)
        .service('toasterLite', common.toasterLite)
        .service('httpLite', common.httpLite)
        .service('usuariosService', usuariosArea.usuariosService)
        .service('usuariosQueryService', usuariosArea.usuariosQueryService)
        .service('loginService', login.loginService) // to log out...! do not remove this!
        .service('loginQueryService', login.loginQueryService)
        .controller('sidenavController', usuariosArea.sidenavController)
        .controller('toolbarHeaderController', usuariosArea.toolbarHeaderController)
        .controller('userMenuWidgetController', common.userMenuWidgetController)
        .controller('mainContentController', usuariosArea.mainContentController)
        .controller('perfilController', usuariosArea.perfilController)
        .controller('organizacionesController', usuariosArea.organizacionesController)
        .config(['$mdIconProvider', '$mdThemingProvider', '$httpProvider', '$routeProvider', function ($mdIconProvider, $mdThemingProvider, $httpProvider, $routeProvider) {
            // most from flat icon dot com
            $mdIconProvider
                .defaultIconSet('../../assets/svg/avatars.svg', 128)
                .icon('menu', '../../assets/svg/menu.svg', 24)
                .icon('close', '../../assets/svg/close.svg')
                .icon('agrobook-white', '../../assets/svg/agrobook-white.svg')
                .icon('agrobook-green', '../../assets/svg/agrobook-green.svg');
            $mdThemingProvider.theme('default')
                .primaryPalette('green')
                .accentPalette('blue');
            common.registerHttpInterceptors($httpProvider);
            usuariosArea.getRouteConfigs().forEach(function (config) {
                $routeProvider.when(config.path, config.route);
            });
            $routeProvider.otherwise({ redirectTo: '/' });
        }]);
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=usuariosArea.js.map