/// <reference path="../_all.ts" />
var common;
(function (common) {
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
            this._avatarUrls = [
                './assets/img/avatar/1.png',
                './assets/img/avatar/2.png',
                './assets/img/avatar/3.png',
                './assets/img/avatar/4.png',
                './assets/img/avatar/5.png',
                './assets/img/avatar/6.png',
                './assets/img/avatar/7.png',
                './assets/img/avatar/8.png',
                './assets/img/avatar/9.png'
            ];
            this._repoIndex = {
                login: {
                    usuarioActual: 'ag-login-usuarioActual'
                }
            };
            this._eventIndex = {
                login: {
                    loggedIn: 'loggedIn',
                    loggedOut: 'loggedOut'
                },
                usuarios: {
                    usuarioSeleccionado: 'usuarioSeleccionado',
                    perfilActualizado: 'perfilActualizado'
                },
                archivos: {
                    productorSeleccionado: 'productorSeleccionado'
                }
            };
        }
        Object.defineProperty(config.prototype, "keyCodes", {
            get: function () { return this._keyCodes; },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(config.prototype, "repoIndex", {
            get: function () { return this._repoIndex; },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(config.prototype, "eventIndex", {
            get: function () { return this._eventIndex; },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(config.prototype, "avatarUrls", {
            get: function () { return this._avatarUrls; },
            enumerable: true,
            configurable: true
        });
        return config;
    }());
    common.config = config;
})(common || (common = {}));
//# sourceMappingURL=config.js.map