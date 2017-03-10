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
            var object = JSON.parse(payload);
            return object;
        };
        return localStorageLite;
    }());
    localStorageLite.$inject = [];
    common.localStorageLite = localStorageLite;
})(common || (common = {}));
//# sourceMappingURL=infrastructure.js.map