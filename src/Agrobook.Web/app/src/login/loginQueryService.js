/// <reference path="../_all.ts" />
var login;
(function (login) {
    var loginQueryService = (function () {
        function loginQueryService($http, config, ls) {
            this.$http = $http;
            this.config = config;
            this.ls = ls;
            this.prefix = 'login/query/';
        }
        loginQueryService.prototype.tryGetLocalLoginInfo = function () {
            return this.ls.get(this.config.repoIndex.login.usuarioActual);
        };
        return loginQueryService;
    }());
    loginQueryService.$inject = ['$http', 'config', 'localStorageLite'];
    login.loginQueryService = loginQueryService;
})(login || (login = {}));
//# sourceMappingURL=loginQueryService.js.map