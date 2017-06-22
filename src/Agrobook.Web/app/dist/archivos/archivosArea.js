/// <;reference path="../_all.ts" />
var archivosArea;
(function (archivosArea) {
    angular.module('archivosArea', ['ngRoute', 'ngMaterial', 'ngMdIcons'])
        .value('config', new common.config())
        .service('localStorageLite', common.localStorageLite)
        .service('toasterLite', common.toasterLite)
        .service('httpLite', common.httpLite)
        .service('loginService', login.loginService)
        .service('uploadService', archivosArea.uploadService)
        .service('loginQueryService', login.loginQueryService)
        .service('archivosQueryService', archivosArea.archivosQueryService)
        .service('usuariosQueryService', usuariosArea.usuariosQueryService)
        .controller('userMenuWidgetController', common.userMenuWidgetController)
        .controller('sidenavController', archivosArea.sidenavController)
        .controller('prodSidenavController', archivosArea.prodSidenavController)
        .controller('toolbarHeaderController', archivosArea.toolbarHeaderController)
        .controller('uploadCenterController', archivosArea.uploadCenterController)
        .controller('mainContentController', archivosArea.mainContentController)
        .config(['$mdIconProvider', '$mdThemingProvider', '$httpProvider', '$routeProvider', function ($mdIconProvider, $mdThemingProvider, $httpProvider, $routeProvider) {
            // most from flat icon dot com
            $mdIconProvider
                .defaultIconSet('./assets/svg/avatars.svg', 128)
                .icon('menu', './assets/svg/menu.svg', 24)
                .icon('close', './assets/svg/close.svg');
            $mdThemingProvider.theme('default')
                .primaryPalette('green')
                .accentPalette('blue');
            common.registerHttpInterceptors($httpProvider);
            archivosArea.getRouteConfigs().forEach(function (config) {
                $routeProvider.when(config.path, config.route);
            });
            $routeProvider.otherwise({ redirectTo: '/' });
        }]);
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=archivosArea.js.map