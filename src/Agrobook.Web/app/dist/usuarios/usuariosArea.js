/// <reference path="../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    angular.module('usuariosArea', ['ngMaterial', 'ngMdIcons'])
        .value('config', new common.config())
        .service('localStorageLite', common.localStorageLite)
        .service('usuariosService', usuariosArea.usuariosService)
        .service('loginService', login.loginService)
        .service('loginQueryService', login.loginQueryService)
        .controller('sidenavController', usuariosArea.sidenavController)
        .controller('toolbarHeaderController', usuariosArea.toolbarHeaderController)
        .controller('userMenuWidgetController', common.userMenuWidgetController)
        .config(['$mdIconProvider', '$mdThemingProvider', '$httpProvider', function ($mdIconProvider, $mdThemingProvider, $httpProvider) {
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
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=usuariosArea.js.map