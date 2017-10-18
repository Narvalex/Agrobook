/// <reference path="../_all.ts" />

module common {
    export function registerHttpInterceptors($httpProvider: angular.IHttpProvider) {
        $httpProvider.interceptors.push(['config', 'localStorageLite', (conf: common.config, ls: common.localStorageLite) => {
            // The interceptor for the unauth response is in the httpLite component.
            return {
                request: (config: angular.IRequestConfig): angular.IRequestConfig => {
                    var loginInfo = ls.get<login.loginResult>(conf.repoIndex.login.usuarioActual);

                    if (loginInfo !== undefined && loginInfo.loginExitoso) {
                        config.headers['Authorization'] = loginInfo.token;
                    }

                   // config.headers['Accept'] = 'application/json';
                    // the line above breaks when trying to obtain some svg...
                    return config;
                }
            };
        }]);
    }
}