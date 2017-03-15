/// <reference path="../_all.ts" />
var login;
(function (login) {
    var loginService = (function () {
        function loginService($http, config, ls, $rootScope) {
            this.$http = $http;
            this.config = config;
            this.ls = ls;
            this.$rootScope = $rootScope;
            this.prefix = 'login/';
        }
        loginService.prototype.post = function (url, dto) {
            return this.$http.post(this.prefix + url, dto);
        };
        loginService.prototype.tryLogin = function (credenciales, successCallback, errorCallback) {
            var _this = this;
            this.post('try-login', credenciales)
                .then(function (value) {
                _this.ls.save(_this.config.repoIndex.login.usuarioActual, value.data);
                _this.$rootScope.$broadcast(_this.config.eventIndex.login.loggedIn, {});
                successCallback(value);
            }, function (reason) { errorCallback(reason); });
        };
        return loginService;
    }());
    loginService.$inject = ['$http', 'config', 'localStorageLite', '$rootScope'];
    login.loginService = loginService;
})(login || (login = {}));
//# sourceMappingURL=loginService.js.map