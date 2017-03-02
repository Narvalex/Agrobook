/// <reference path="../_all.ts" />

module UsuariosArea {
    angular.module('usuariosArea', ['ngMaterial', 'ngMdIcons'])
        .controller('sidenavController', sidenavController)
        .controller('toolbarHeaderController', toolbarHeaderController)
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