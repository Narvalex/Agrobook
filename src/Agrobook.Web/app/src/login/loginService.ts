/// <reference path="../_all.ts" />

module login {
    export class loginService {
        private prefix = 'login/';

        static $inject = ['$http', 'config', 'localStorageLite', '$rootScope'];

        constructor(
            private $http: angular.IHttpService,
            private config: common.config,
            private ls: common.localStorageLite,
            private $rootScope: angular.IRootScopeService
        ) {
        }

        private post<TResult>(url: string, dto: any): ng.IHttpPromise<TResult> {
            return this.$http.post(this.prefix + url, dto);
        }

        tryLogin(
            credenciales: credencialesDto,
            successCallback: (value: ng.IHttpPromiseCallbackArg<loginResult>) => void,
            errorCallback: (reason: any) => void): void {
            this.post<loginResult>('try-login', credenciales)
                .then(
                value => {
                    this.ls.save(this.config.repoIndex.login.usuarioActual, value.data);
                    this.$rootScope.$broadcast(this.config.eventIndex.login.loggedIn, {});
                    successCallback(value);
                },
                reason => { errorCallback(reason); });
        }
    }
}