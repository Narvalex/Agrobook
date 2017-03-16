/// <reference path="../_all.ts" />

module homeArea {
    angular.module("app", ['ngMaterial', 'ngMdIcons'])
        .value('config', new common.config())
        .service('localStorageLite', common.localStorageLite)
        .service('loginService', login.loginService)
        .service('loginQueryService', login.loginQueryService)
        .controller('toolbar-headerController', ToolbarHeaderController)
        .controller('userMenuWidgetController', common.userMenuWidgetController)
        .config(['$mdIconProvider', '$mdThemingProvider', '$httpProvider', '$injector', (
            $mdIconProvider: angular.material.IIconProvider,
            $mdThemingProvider: angular.material.IThemingProvider,
            $httpProvider: angular.IHttpProvider,
            $injector: angular.auto.IInjectorService
        ) => {

            $mdIconProvider
                .defaultIconSet('./app/assets/svg/avatars.svg', 128)
                .icon('menu', './app/assets/svg/menu.svg', 24);

            $mdThemingProvider.theme('default')
                .primaryPalette('green')
                .accentPalette('blue');


            $httpProvider.interceptors.push(['config', 'localStorageLite', (conf: common.config, ls: common.localStorageLite) => {
                return {
                    request: (config: angular.IRequestConfig): angular.IRequestConfig => {
                        var loginInfo = ls.get<login.loginResult>(conf.repoIndex.login.usuarioActual);

                        if (loginInfo !== undefined && loginInfo.loginExitoso) {
                            config.headers['Authorization'] = loginInfo.token;
                        }

                        config.headers['Accept'] = 'application/json';
                        return config;
                    }
                };
            }]);
        }]);
}