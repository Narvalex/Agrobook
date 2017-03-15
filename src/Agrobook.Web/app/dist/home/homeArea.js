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
        .config(function ($mdIconProvider, $mdThemingProvider) {
        $mdIconProvider
            .defaultIconSet('./app/assets/svg/avatars.svg', 128)
            .icon('menu', './app/assets/svg/menu.svg', 24);
        $mdThemingProvider.theme('default')
            .primaryPalette('green')
            .accentPalette('blue');
    });
})(homeArea || (homeArea = {}));
//# sourceMappingURL=homeArea.js.map