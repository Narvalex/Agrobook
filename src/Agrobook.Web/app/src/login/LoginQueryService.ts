/// <reference path="../_all.ts" />

module login {
    export class loginQueryService {
        private prefix = 'login/query/';

        static $inject = ['$http', 'config', 'localStorageLite'];

        constructor(
            private $http: angular.IHttpService,
            private config: common.config,
            private ls: common.localStorageLite) { }

        tryGetLocalLoginInfo(): loginResult {
            return this.ls.get<loginResult>(this.config.repoIndex.login.usuarioActual);
        }
    }
}