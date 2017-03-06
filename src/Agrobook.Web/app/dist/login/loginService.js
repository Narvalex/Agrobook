/// <reference path="../_all.ts" />
var login;
(function (login) {
    var loginService = (function () {
        function loginService($http) {
            this.$http = $http;
            this.prefix = 'login';
        }
        loginService.prototype.post = function (url, dto) {
            return this.$http.post(this.prefix + '/' + url, dto);
        };
        loginService.prototype.tryLogin = function (credenciales) {
            return this.post('try-login', credenciales);
        };
        return loginService;
    }());
    loginService.$inject = ['$http'];
    login.loginService = loginService;
})(login || (login = {}));
//# sourceMappingURL=loginService.js.map