/// <reference path="_all.ts" />
var Common;
(function (Common) {
    var config = (function () {
        function config() {
            this._keyCodes = {
                backspace: 8,
                tab: 9,
                enter: 13,
                esc: 27,
                space: 32,
                pageup: 33,
                pagedown: 34,
                end: 35,
                home: 36,
                left: 37,
                up: 38,
                right: 39,
                down: 40,
                insert: 45,
                del: 46
            };
        }
        Object.defineProperty(config.prototype, "keyCodes", {
            get: function () { return this._keyCodes; },
            enumerable: true,
            configurable: true
        });
        return config;
    }());
    Common.config = config;
})(Common || (Common = {}));
//# sourceMappingURL=config.js.map