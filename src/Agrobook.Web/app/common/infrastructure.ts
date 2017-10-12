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
        private defaultPosition = 'bottom right';

        private tplUrl = '../common/toasts/toast-cerrable.html';

        constructor(
            private $mdToast: angular.material.IToastService) { 
        }

        info(message: string, delay: number = this.defaultDelay, closeButton: boolean = true) {
            var self = this;
            let options: angular.material.IToastOptions = {
                hideDelay: delay,
                position: this.defaultPosition,
                controllerAs: 'vm',
                templateUrl: this.tplUrl,
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
                templateUrl: this.tplUrl,
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
                templateUrl: this.tplUrl,
                toastClass: 'error',
                controller: class {
                    message: string = message;
                    close = () => { self.$mdToast.hide(); }
                }
            };
            this.$mdToast.show(options);
        }

        default(message: string, delay: number = this.defaultDelay, closeButton: boolean = true, position: string = this.defaultPosition) {
            var self = this;
            let options: angular.material.IToastOptions = {
                hideDelay: delay,
                position: position,
                controllerAs: 'vm',
                templateUrl: this.tplUrl,
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

        protected get<TResult>(
            url: string,
            successCallback: (value: ng.IHttpPromiseCallbackArg<TResult>) => any,
            errorCallback?: (reason: any) => any
        ) {
            var self = this;
            return this.$httpService.get(this.buildUrl(url))
                .then<TResult>(successCallback,
                reason => {
                    self.handleError(reason, errorCallback);
                });
        }

        protected post<TResult>(
            url: string,
            dto,
            successCallback: (value: ng.IHttpPromiseCallbackArg<TResult>) => any,
            errorCallback?: (reason: any) => any
        ) {
            var self = this;
            this.$httpService.post<TResult>(this.buildUrl(url), dto)
                .then<TResult>(successCallback,
                reason => {
                    self.handleError(reason, errorCallback);
                });
        }

        protected getWithCallback<TResult>(url: string, callback: callbackLite<TResult>) {
            this.get<TResult>(url, callback.onSuccess, callback.onError);
        }

        protected postWithCallback<TResult>(url: string, dto: any, callback: callbackLite<TResult>) {
            this.post<TResult>(url, dto, callback.onSuccess, callback.onError);
        }

        private buildUrl(url: string) : string {
            return this.prefix + '/' + url;
        }

        private handleError(reason: any, errorCallback?: (reason: any) => any) {
            // TODO here
            if (reason.status === 401) {
                window.location.replace('../home/index.html?unauth=1');
            }

            if (errorCallback !== undefined && errorCallback !== null)
                errorCallback(reason);
        }
    }

    export class callbackLite<TResult> {
        constructor(
            public onSuccess: (value: ng.IHttpPromiseCallbackArg<TResult>) => any,
            public onError: (reason: any) => any
        ) {
        }
    }
}