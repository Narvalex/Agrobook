/// <reference path="_all.ts" />
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
})(common || (common = {}));
//# sourceMappingURL=infrastructure.js.map