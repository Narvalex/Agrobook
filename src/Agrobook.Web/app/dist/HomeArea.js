/// <reference path="_all.ts" />
var HomeArea;
(function (HomeArea) {
    angular.module("app", ['ngMaterial', 'ngMdIcons'])
        .value('config', new common.config())
        .service('localStorageLite', common.localStorageLite)
        .service('loginWriteService', login.loginService)
        .controller('toolbar-headerController', HomeArea.ToolbarHeaderController)
        .config(function ($mdIconProvider, $mdThemingProvider) {
        $mdIconProvider
            .defaultIconSet('./app/assets/svg/avatars.svg', 128)
            .icon('menu', './app/assets/svg/menu.svg', 24);
        $mdThemingProvider.theme('default')
            .primaryPalette('green')
            .accentPalette('blue');
    });
})(HomeArea || (HomeArea = {}));
//# sourceMappingURL=HomeArea.js.map