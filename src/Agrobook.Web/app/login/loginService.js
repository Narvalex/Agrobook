/// <reference path="../_all.ts" />
var login;
(function (login) {
    var loginService = (function () {
        function loginService($http, config, ls, $rootScope) {
            this.$http = $http;
            this.config = config;
            this.ls = ls;
            this.$rootScope = $rootScope;
            this.prefix = '../login/';
            this.user = null;
            var localVersion = this.ls.get(this.config.repoIndex.login.localVersion);
            if (localVersion === undefined || localVersion === null || localVersion !== this.config.repoIndex.login.lastestVersion)
                this.ls.delete(this.config.repoIndex.login.localVersion);
        }
        loginService.prototype.post = function (url, dto) {
            return this.$http.post(this.prefix + url, dto);
        };
        loginService.prototype.tryLogin = function (credenciales, successCallback, errorCallback) {
            var _this = this;
            this.post('try-login', credenciales)
                .then(function (value) {
                _this.user = value.data;
                _this.ls.save(_this.config.repoIndex.login.usuarioActual, value.data);
                _this.$rootScope.$broadcast(_this.config.eventIndex.login.loggedIn, {});
                successCallback(value);
            }, function (reason) { errorCallback(reason); });
        };
        loginService.prototype.logOut = function () {
            this.user = null;
            this.ls.delete(this.config.repoIndex.login.usuarioActual);
            this.$rootScope.$broadcast(this.config.eventIndex.login.loggedOut, {});
        };
        loginService.prototype.autorizar = function (claims) {
            if (this.user === null) {
                this.user = this.ls.get(this.config.repoIndex.login.usuarioActual);
                if (this.user === null || this.user === undefined)
                    return false;
            }
            for (var i = 0; i < claims.length; i++) {
                for (var j = 0; j < this.user.claims.length; j++) {
                    if (this.user.claims[j] === 'rol-admin'
                        || claims[i] === this.user.claims[j])
                        return true;
                }
            }
            return false;
        };
        return loginService;
    }());
    loginService.$inject = ['$http', 'config', 'localStorageLite', '$rootScope'];
    login.loginService = loginService;
})(login || (login = {}));
//# sourceMappingURL=loginService.js.map