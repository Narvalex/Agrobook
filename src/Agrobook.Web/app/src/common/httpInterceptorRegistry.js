/// <reference path="../_all.ts" />
var common;
(function (common) {
    function registerHttpInterceptors($httpProvider) {
        $httpProvider.interceptors.push(['config', 'localStorageLite', function (conf, ls) {
                return {
                    request: function (config) {
                        var loginInfo = ls.get(conf.repoIndex.login.usuarioActual);
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
    common.registerHttpInterceptors = registerHttpInterceptors;
})(common || (common = {}));
//# sourceMappingURL=httpInterceptorRegistry.js.map