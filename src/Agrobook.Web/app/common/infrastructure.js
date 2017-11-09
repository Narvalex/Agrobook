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
            this.defaultPosition = 'bottom right';
            this.tplUrl = '../common/toasts/toast-cerrable.html';
        }
        toasterLite.prototype.info = function (message, delay, closeButton) {
            if (delay === void 0) { delay = this.defaultDelay; }
            if (closeButton === void 0) { closeButton = true; }
            var self = this;
            var options = {
                hideDelay: delay,
                position: this.defaultPosition,
                controllerAs: 'vm',
                templateUrl: this.tplUrl,
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
                templateUrl: this.tplUrl,
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
                templateUrl: this.tplUrl,
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
        toasterLite.prototype.default = function (message, delay, closeButton, position) {
            if (delay === void 0) { delay = this.defaultDelay; }
            if (closeButton === void 0) { closeButton = true; }
            if (position === void 0) { position = this.defaultPosition; }
            var self = this;
            var options = {
                hideDelay: delay,
                position: position,
                controllerAs: 'vm',
                templateUrl: this.tplUrl,
                controller: (function () {
                    function class_4() {
                        this.message = message;
                        this.close = function () { self.$mdToast.hide(); };
                    }
                    return class_4;
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
            var self = this;
            return this.$httpService.get(this.buildUrl(url))
                .then(successCallback, function (reason) {
                self.handleError(reason, errorCallback);
            });
        };
        httpLite.prototype.post = function (url, dto, successCallback, errorCallback) {
            var self = this;
            this.$httpService.post(this.buildUrl(url), dto)
                .then(successCallback, function (reason) {
                self.handleError(reason, errorCallback);
            });
        };
        httpLite.prototype.getWithCallback = function (url, callback) {
            this.get(url, callback.onSuccess, callback.onError);
        };
        httpLite.prototype.postWithCallback = function (url, dto, callback) {
            this.post(url, dto, callback.onSuccess, callback.onError);
        };
        httpLite.prototype.buildUrl = function (url) {
            return this.prefix + '/' + url;
        };
        httpLite.prototype.handleError = function (reason, errorCallback) {
            // TODO here
            if (reason.status === 401) {
                window.location.replace('../home/index.html?unauth=1');
            }
            if (errorCallback !== undefined && errorCallback !== null)
                errorCallback(reason);
        };
        return httpLite;
    }());
    common.httpLite = httpLite;
    var callbackLite = (function () {
        function callbackLite(onSuccess, onError) {
            this.onSuccess = onSuccess;
            this.onError = onError;
        }
        return callbackLite;
    }());
    common.callbackLite = callbackLite;
    var numberFormatter = (function () {
        function numberFormatter() {
        }
        numberFormatter.prototype.parseNumberWithCommaAsDecimalSeparator = function (number) {
            var value = number.replace(',', '.');
            return parseFloat(value);
        };
        numberFormatter.prototype.formatFromUSNumber = function (n, decimalsQuantity) {
            if (decimalsQuantity === void 0) { decimalsQuantity = 2; }
            var stringNumber = n.toString();
            stringNumber = stringNumber.replace('.', ',');
            var snArray = stringNumber.split(',');
            var sn1 = snArray[0];
            var sn2 = snArray.length > 1 ? ',' + snArray[1].substring(0, decimalsQuantity) : '';
            var regex = /(\d+)(\d{3})/;
            while (regex.test(sn1))
                sn1 = sn1.replace(regex, '$1' + '.' + '$2');
            return sn1 + sn2;
        };
        return numberFormatter;
    }());
    common.numberFormatter = numberFormatter;
})(common || (common = {}));
//# sourceMappingURL=infrastructure.js.map