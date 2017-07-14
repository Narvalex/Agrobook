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
                './assets/img/avatar/9.png',
                './assets/img/avatar/10.png'
            ];
            this._claims = {
                roles: {
                    Admin: 'rol-admin',
                    Gerente: 'rol-gerente',
                    Tecnico: 'rol-tecnico',
                    Productor: 'rol-productor',
                    Invitado: 'rol-invitado'
                },
                permisos: {
                    AdministrarOrganizaciones: 'permiso-administrar-organizaciones'
                }
            };
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
                    perfilActualizado: 'perfilActualizado',
                    usuarioAgregadoAOrganizacion: 'usuarioAgregadoAOrganizacion'
                },
                archivos: {
                    productorSeleccionado: 'productorSeleccionado',
                    abrirCuadroDeCargaDeArchivos: 'abrirCuadroDeCargaDeArchivos',
                    filtrar: 'filtrar'
                }
            };
            this._tiposDeArchivos = {
                todos: new TipoDeArchivo("Todos", "list"),
                generico: new TipoDeArchivo("Generico", "generic-file"),
                fotos: new TipoDeArchivo("Fotos", "picture"),
                pdf: new TipoDeArchivo("PDF", "pdf"),
                mapas: new TipoDeArchivo("Mapas", "google-earth"),
                excel: new TipoDeArchivo("Excel", "excel"),
                word: new TipoDeArchivo("Word", "word"),
                powerPoint: new TipoDeArchivo("PowerPoint", "powerPoint")
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
        Object.defineProperty(config.prototype, "claims", {
            get: function () { return this._claims; },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(config.prototype, "tiposDeArchivos", {
            get: function () { return this._tiposDeArchivos; },
            enumerable: true,
            configurable: true
        });
        return config;
    }());
    common.config = config;
    var TipoDeArchivo = (function () {
        function TipoDeArchivo(display, icon) {
            this.display = display;
            this.icon = icon;
        }
        return TipoDeArchivo;
    }());
    common.TipoDeArchivo = TipoDeArchivo;
})(common || (common = {}));
//# sourceMappingURL=config.js.map