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

        private user: loginResult = null;

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
                    this.user = value.data;
                    this.ls.save(this.config.repoIndex.login.usuarioActual, value.data);
                    this.$rootScope.$broadcast(this.config.eventIndex.login.loggedIn, {});
                    successCallback(value);
                },
                reason => { errorCallback(reason); });
        }

        logOut() {
            this.user = null;
            this.ls.delete(this.config.repoIndex.login.usuarioActual);
            this.$rootScope.$broadcast(this.config.eventIndex.login.loggedOut, {});
        }

        autorizar(claims: string[]): boolean {
            if (this.user === null) {
                this.user = this.ls.get<loginResult>(this.config.repoIndex.login.usuarioActual);
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
        }
    }
}