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
                .defaultIconSet('./app/assets/svg/avatars.svg', 128)
                .icon('menu', './app/assets/svg/menu.svg', 24);
            $mdThemingProvider.theme('default')
                .primaryPalette('green')
                .accentPalette('blue');
            $httpProvider.interceptors.push(['config', 'localStorageLite', function (conf, ls) {
                    return {
                        request: function (config) {
                            var loginInfo = ls.get(conf.repoIndex.login.usuarioActual);
                            if (loginInfo !== undefined && loginInfo.loginExitoso) {
                                config.headers['Authorization'] = loginInfo.token;
                            }
                            config.headers['Accept'] = 'application/json';
                            return config;
                        }
                    };
                }]);
        }]);
})(homeArea || (homeArea = {}));
//# sourceMappingURL=homeArea.js.map