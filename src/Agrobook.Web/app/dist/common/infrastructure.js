/// <reference path="../_all.ts" />
var common;
(function (common) {
    var localStorageLite = (function () {
        function localStorageLite() {
        }
        localStorageLite.prototype.save = function (key, payload) {
            var serialized = JSON.stringify(payload);
            localStorage[key] = serialized;
        };
        localStorageLite.prototype.get = function (key) {
            var payload = localStorage[key];
            if (payload === undefined)
                return undefined;
            var object = JSON.parse(payload);
            return object;
        };
        localStorageLite.prototype.delete = function (key) {
            localStorage.removeItem(key);
        };
        return localStorageLite;
    }());
    localStorageLite.$inject = [];
    common.localStorageLite = localStorageLite;
    var toasterLite = (function () {
        function toasterLite($mdToast) {
            this.$mdToast = $mdToast;
            this.defaultDelay = 5000; // 5 seconds
            this.defaultPosition = 'top right';
        }
        toasterLite.prototype.info = function (message, delay, closeButton) {
            if (delay === void 0) { delay = this.defaultDelay; }
            if (closeButton === void 0) { closeButton = true; }
            var self = this;
            var options = {
                hideDelay: delay,
                position: this.defaultPosition,
                controllerAs: 'vm',
                templateUrl: './dist/common/toasts/toast-cerrable.html',
                toastClass: 'info',
                controller: (function () {
                    function class_1() {
                        this.message = message;
                        this.close = function () { self.$mdToast.hide(); };
                    }
                    return class_1;
                }())
            };
            this.$mdToast.show(options);
        };
        toasterLite.prototype.success = function (message, delay, closeButton) {
            if (delay === void 0) { delay = this.defaultDelay; }
            if (closeButton === void 0) { closeButton = true; }
            var self = this;
            var options = {
                hideDelay: delay,
                position: this.defaultPosition,
                controllerAs: 'vm',
                templateUrl: './dist/common/toasts/toast-cerrable.html',
                toastClass: 'success',
                controller: (function () {
                    function class_2() {
                        this.message = message;
                        this.close = function () { self.$mdToast.hide(); };
                    }
                    return class_2;
                }())
            };
            this.$mdToast.show(options);
        };
        toasterLite.prototype.error = function (message, delay, closeButton) {
            if (delay === void 0) { delay = this.defaultDelay; }
            if (closeButton === void 0) { closeButton = true; }
            var self = this;
            var options = {
                hideDelay: delay,
                position: this.defaultPosition,
                controllerAs: 'vm',
                templateUrl: './dist/common/toasts/toast-cerrable.html',
                toastClass: 'error',
                controller: (function () {
                    function class_3() {
                        this.message = message;
                        this.close = function () { self.$mdToast.hide(); };
                    }
                    return class_3;
                }())
            };
            this.$mdToast.show(options);
        };
        Object.defineProperty(toasterLite.prototype, "delayForever", {
            get: function () { return 3600000; },
            enumerable: true,
            configurable: true
        });
        ; // an hour! :O
        return toasterLite;
    }());
    toasterLite.$inject = ['$mdToast'];
    common.toasterLite = toasterLite;
    var httpLite = (function () {
        function httpLite($httpService, prefix) {
            if (prefix === void 0) { prefix = ''; }
            this.$httpService = $httpService;
            this.prefix = prefix;
        }
        httpLite.prototype.get = function (url, successCallback, errorCallback) {
            return this.$httpService.get(this.buildUrl(url))
                .then(successCallback, errorCallback);
        };
        httpLite.prototype.post = function (url, dto, successCallback, erroCallback) {
            this.$httpService.post(this.buildUrl(url), dto)
                .then(successCallback, erroCallback);
        };
        httpLite.prototype.buildUrl = function (url) {
            return this.prefix + '/' + url;
        };
        return httpLite;
    }());
    common.httpLite = httpLite;
})(common || (common = {}));
//# sourceMappingURL=infrastructure.js.map