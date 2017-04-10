/// <reference path="_all.ts" />

module common {
    export class config {

        private _keyCodes = {
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

        private _repoIndex = {
            login: {
                usuarioActual: 'ag-login-usuarioActual'
            }
        }

        private _eventIndex = {
            login: {
                loggedIn: 'loggedIn',
                loggedOut: 'loggedOut'
            },
            usuarios: {
                usuarioSeleccionado: 'usuarioSeleccionado'
            }
        }

        get keyCodes() { return this._keyCodes; }
        get repoIndex() { return this._repoIndex; }
        get eventIndex() { return this._eventIndex; }
    }
}