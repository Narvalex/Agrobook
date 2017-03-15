/// <reference path="../_all.ts" />
var UsuariosArea;
(function (UsuariosArea) {
    angular.module('usuariosArea', ['ngMaterial', 'ngMdIcons'])
        .value('config', new common.config())
        .service('localStorageLite', common.localStorageLite)
        .service('usuariosWriteService', UsuariosArea.usuariosWriteService)
        .service('loginService', login.loginService)
        .service('loginQueryService', login.loginQueryService)
        .controller('sidenavController', UsuariosArea.sidenavController)
        .controller('toolbarHeaderController', UsuariosArea.toolbarHeaderController)
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
})(UsuariosArea || (UsuariosArea = {}));
//# sourceMappingURL=usuariosArea.js.map