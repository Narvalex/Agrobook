/// <reference path="../_all.ts" />
var homeArea;
(function (homeArea) {
    angular.module("app", ['ngMaterial', 'ngMdIcons'])
        .value('config', new common.config())
        .service('localStorageLite', common.localStorageLite)
        .service('loginService', login.loginService)
        .service('loginQueryService', login.loginQueryService)
        .controller('toolbar-headerController', homeArea.ToolbarHeaderController)
        .controller('userMenuWidgetController', common.userMenuWidgetController)
        .config(['$mdIconProvider', '$mdThemingProvider', '$httpProvider', '$injector', function ($mdIconProvider, $mdThemingProvider, $httpProvider, $injector) {
            $mdIconProvider
                .defaultIconSet('./assets/svg/avatars.svg', 128)
                .icon('menu', './assets/svg/menu.svg', 24);
            $mdThemingProvider.theme('default')
                .primaryPalette('green')
                .accentPalette('blue');
            common.registerHttpInterceptors($httpProvider);
        }]);
})(homeArea || (homeArea = {}));
//# sourceMappingURL=homeArea.js.map