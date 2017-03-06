/// <reference path="../_all.ts" />

module login {
    export class loginService {
        private prefix = 'login';

        static $inject = ['$http'];

        constructor(
            private $http: angular.IHttpService) { }

        private post<TResult>(url: string, dto: any): ng.IHttpPromise<TResult> {
            return this.$http.post(this.prefix + '/' + url, dto);
        }

        tryLogin(credenciales: credencialesDto): ng.IHttpPromise<loginResult> {
            return this.post<loginResult>('try-login', credenciales);
        }
    }
}