/// <reference path="../_all.ts" />

module common {
    export class localStorageLite {

        static $inject = [];

        constructor() { }

        save(key: string, payload: any): void {
            var serialized = JSON.stringify(payload);
            localStorage[key] = serialized;
        }

        get<T>(key: string): T {
            var payload = localStorage[key];
            if (payload === undefined)
                return undefined;

            var object = JSON.parse(payload);
            return object as T;
        }

        delete(key: string) {
            localStorage.removeItem(key);
        }
    }

    export class toasterLite {
        static $inject = ['$mdToast'];

        private defaultDelay = 5000; // 5 seconds
        private defaultPosition = 'top right';

        constructor(
            private $mdToast: angular.material.IToastService) { 
        }

        info(message: string, delay: number = this.defaultDelay, closeButton: boolean = true) {
            var self = this;
            let options: angular.material.IToastOptions = {
                hideDelay: delay,
                position: this.defaultPosition,
                controllerAs: 'vm',
                templateUrl: './dist/common/toasts/toast-cerrable.html',
                toastClass: 'info',
                controller: class {
                    message: string = message;
                    close = () => { self.$mdToast.hide(); }
                }
            };
            this.$mdToast.show(options);
        }

        success(message: string, delay: number = this.defaultDelay, closeButton: boolean = true) {
            var self = this;
            let options: angular.material.IToastOptions = {
                hideDelay: delay,
                position: this.defaultPosition,
                controllerAs: 'vm',
                templateUrl: './dist/common/toasts/toast-cerrable.html',
                toastClass: 'success',
                controller: class {
                    message: string = message;
                    close = () => { self.$mdToast.hide(); }
                }
            };
            this.$mdToast.show(options);
        }

        error(message: string, delay: number = this.defaultDelay, closeButton: boolean = true) {
            var self = this;
            let options: angular.material.IToastOptions = {
                hideDelay: delay,
                position: this.defaultPosition,
                controllerAs: 'vm',
                templateUrl: './dist/common/toasts/toast-cerrable.html',
                toastClass: 'error',
                controller: class {
                    message: string = message;
                    close = () => { self.$mdToast.hide(); }
                }
            };
            this.$mdToast.show(options);
        }

        get delayForever(): number { return 3600000; }; // an hour! :O
    }

    export abstract class httpLite {

        constructor(
            private $httpService: angular.IHttpService,
            private prefix: string = '',
        ) {
        }

        get<TResult>(
            url: string,
            successCallback: (value: ng.IHttpPromiseCallbackArg<TResult>) => any,
            errorCallback?: (reason: any) => any
        ) {
            return this.$httpService.get(this.buildUrl(url))
                .then<TResult>(successCallback, errorCallback);
        }

        post<TResult>(
            url: string,
            dto,
            successCallback: (value: ng.IHttpPromiseCallbackArg<TResult>) => any,
            erroCallback?: (reason: any) => any
        ) {
            this.$httpService.post<TResult>(this.buildUrl(url), dto)
                .then<TResult>(successCallback, erroCallback);
        }

        private buildUrl(url: string) : string {
            return this.prefix + '/' + url;
        }
    }
}