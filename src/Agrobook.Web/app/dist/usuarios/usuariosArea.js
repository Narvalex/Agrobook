/// <reference path="../_all.ts" />
var UsuariosArea;
(function (UsuariosArea) {
    angular.module('usuariosArea', ['ngMaterial', 'ngMdIcons'])
        .controller('sidenavController', UsuariosArea.sidenavController)
        .controller('toolbarHeaderController', UsuariosArea.toolbarHeaderController)
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