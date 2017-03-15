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
        .config(function ($mdIconProvider, $mdThemingProvider) {
        // most from flat icon dot com
        $mdIconProvider
            .defaultIconSet('../app/assets/svg/avatars.svg', 128)
            .icon('menu', '../app/assets/svg/menu.svg')
            .icon('close', '../app/assets/svg/close.svg');
        $mdThemingProvider.theme('default')
            .primaryPalette('green')
            .accentPalette('blue');
    });
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=usuariosArea.js.map