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

        private _avatarUrls = [
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
                usuarioSeleccionado: 'usuarioSeleccionado',
                perfilActualizado: 'perfilActualizado'
            }
        }

        get keyCodes() { return this._keyCodes; }
        get repoIndex() { return this._repoIndex; }
        get eventIndex() { return this._eventIndex; }
        get avatarUrls() { return this._avatarUrls; }
    }
}