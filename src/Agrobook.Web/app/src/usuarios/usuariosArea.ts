/// <reference path="../_all.ts" />

module usuariosArea {
    angular.module('usuariosArea', ['ngMaterial', 'ngMdIcons'])
        .value('config', new common.config())
        .service('localStorageLite', common.localStorageLite)
        .service('toasterLite', common.toasterLite)
        .service('usuariosService', usuariosService)
        .service('loginService', login.loginService)
        .service('loginQueryService', login.loginQueryService)
        .controller('sidenavController', sidenavController)
        .controller('toolbarHeaderController', toolbarHeaderController)
        .controller('userMenuWidgetController', common.userMenuWidgetController)
        .config(['$mdIconProvider', '$mdThemingProvider', '$httpProvider', (
            $mdIconProvider: angular.material.IIconProvider,
            $mdThemingProvider: angular.material.IThemingProvider,
            $httpProvider: angular.IHttpProvider) => {

            // most from flat icon dot com
            $mdIconProvider
                .defaultIconSet('./assets/svg/avatars.svg', 128)
                .icon('menu', './assets/svg/menu.svg', 24)
                .icon('close', './assets/svg/close.svg');

            $mdThemingProvider.theme('default')
                .primaryPalette('green')
                .accentPalette('blue');

            common.registerHttpInterceptors($httpProvider); 
        }]);
}