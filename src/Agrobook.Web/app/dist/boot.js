/// <reference path="_all.ts" />
var Home;
(function (Home) {
    angular.module("app", ['ngMaterial', 'ngMdIcons'])
        .service('loginWriteService', login.loginService)
        .controller('toolbar-headerController', Home.ToolbarHeaderController)
        .config(function ($mdIconProvider, $mdThemingProvider) {
        $mdIconProvider
            .defaultIconSet('./app/assets/svg/avatars.svg', 128)
            .icon('menu', './app/assets/svg/menu.svg', 24);
        $mdThemingProvider.theme('default')
            .primaryPalette('green')
            .accentPalette('blue');
    });
})(Home || (Home = {}));
//# sourceMappingURL=boot.js.map