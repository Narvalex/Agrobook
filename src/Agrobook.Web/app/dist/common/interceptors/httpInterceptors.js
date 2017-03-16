/// <reference path="../../_all.ts" />
var common;
(function (common) {
    var httpRequestInterceptor = (function () {
        function httpRequestInterceptor(ls, config) {
            this.ls = ls;
            this.config = config;
            return this;
        }
        httpRequestInterceptor.prototype.request = function (config) {
            var loginInfo = this.ls.get(this.config.repoIndex.login.usuarioActual);
            if (loginInfo !== undefined && loginInfo.loginExitoso) {
                config.headers['Authorization'] = loginInfo.token;
            }
            config.headers['Accept'] = 'application/json';
            return config;
        };
        return httpRequestInterceptor;
    }());
    httpRequestInterceptor.$inject = ['localStorageLite', 'config'];
    common.httpRequestInterceptor = httpRequestInterceptor;
    var httpResponseInterceptor = (function () {
        function httpResponseInterceptor() {
        }
        return httpResponseInterceptor;
    }());
    common.httpResponseInterceptor = httpResponseInterceptor;
})(common || (common = {}));
//# sourceMappingURL=httpInterceptors.js.map