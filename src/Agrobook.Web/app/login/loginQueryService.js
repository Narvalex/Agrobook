/// <reference path="../_all.ts" />
var login;
(function (login) {
    var loginQueryService = /** @class */ (function () {
        function loginQueryService($http, config, ls) {
            this.$http = $http;
            this.config = config;
            this.ls = ls;
            this.prefix = 'login/query/';
        }
        loginQueryService.prototype.tryGetLocalLoginInfo = function () {
            return this.ls.get(this.config.repoIndex.login.usuarioActual);
        };
        loginQueryService.$inject = ['$http', 'config', 'localStorageLite'];
        return loginQueryService;
    }());
    login.loginQueryService = loginQueryService;
})(login || (login = {}));
//# sourceMappingURL=loginQueryService.js.map