/// <reference path="../_all.ts" />

module usuariosArea {
    angular.module('usuariosArea', ['ngMaterial', 'ngMdIcons'])
        .value('config', new common.config())
        .service('localStorageLite', common.localStorageLite)
        .service('usuariosService', usuariosService)
        .service('loginService', login.loginService)
        .service('loginQueryService', login.loginQueryService)
        .controller('sidenavController', sidenavController)
        .controller('toolbarHeaderController', toolbarHeaderController)
        .controller('userMenuWidgetController', common.userMenuWidgetController)
        .config((
            $mdIconProvider: angular.material.IIconProvider,
            $mdThemingProvider: angular.material.IThemingProvider) => {

            // most from flat icon dot com
            $mdIconProvider
                .defaultIconSet('../app/assets/svg/avatars.svg', 128)
                .icon('menu', '../app/assets/svg/menu.svg')
                .icon('close', '../app/assets/svg/close.svg');

            $mdThemingProvider.theme('default')
                .primaryPalette('green')
                .accentPalette('blue');
        });
}