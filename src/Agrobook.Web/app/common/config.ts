/// <reference path="../_all.ts" />

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
            '../assets/img/avatar/1.png',
            '../assets/img/avatar/2.png',
            '../assets/img/avatar/3.png',
            '../assets/img/avatar/4.png',
            '../assets/img/avatar/5.png',
            '../assets/img/avatar/6.png',
            '../assets/img/avatar/7.png',
            '../assets/img/avatar/8.png',
            '../assets/img/avatar/9.png',
            '../assets/img/avatar/10.png'
        ];

        private _claims = {
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
        }

        private _repoIndex = {
            login: {
                lastestVersion: '1.0',
                localVersion: 'ag-login-localVersion',
                usuarioActual: 'ag-login-usuarioActual',
            }
        }

        private _eventIndex = {
            login: {
                loggedIn: 'loggedIn',
                loggedOut: 'loggedOut'
            },
            usuarios: {
                usuarioSeleccionado: 'usuarioSeleccionado',
                perfilActualizado: 'perfilActualizado',
                usuarioAgregadoAOrganizacion: 'usuarioAgregadoAOrganizacion'
            },
            filesWidget: {
                reloadFiles: 'filesWidget-reloadFiles',
                fileUploaded: 'filesWidget-fileUploaded',
                fileRestored: 'filesWidget-fileRestored',
                fileDeleted: 'filesWidget-fileDeleted'
            },
            archivos: {
                productorSeleccionado: 'productorSeleccionado',
                abrirCuadroDeCargaDeArchivos: 'abrirCuadroDeCargaDeArchivos',
                filtrar: 'filtrar'
            },
            ap_servicios: {
                nuevoServicioCreado: 'nuevoServicioCreado',
                cambioDeParcelaEnServicio: 'cambioDeParcelaEnServicio'
            }
        }

        private _tiposDeArchivos = {
            todos: new TipoDeArchivo("Todos", "list"),
            generico: new TipoDeArchivo("Generico", "generic-file"),
            fotos: new TipoDeArchivo("Fotos", "picture"),
            pdf: new TipoDeArchivo("PDF", "pdf"),
            mapas: new TipoDeArchivo("Mapas", "google-earth"),
            excel: new TipoDeArchivo("Excel", "excel"),
            word: new TipoDeArchivo("Word", "word"),
            powerPoint: new TipoDeArchivo("PowerPoint", "powerPoint")
        }

        private _conventions = {
            // add conventions here
            //nuevoServicioUrlToken: 'nuevo-servicio'
        }

        private _categoriaDeArchivos = {
            // servicio
            servicioDatosBasicos: 'servicioDatosBasicos',
            servicioParcelas: 'servicioParcelas',
            servicioDiagnostico: 'servicioDiagnostico',
            servicioPrescripciones: 'servicioPrescripciones',
            // org
            orgContratos: 'orgContratos'
        }

        get keyCodes() { return this._keyCodes; }
        get repoIndex() { return this._repoIndex; }
        get eventIndex() { return this._eventIndex; }
        get avatarUrls() { return this._avatarUrls; }
        get claims() { return this._claims; }
        get tiposDeArchivos() { return this._tiposDeArchivos; }
        get conventions() { return this._conventions }
        get categoriaDeArchivos() { return this._categoriaDeArchivos }
    }

    export class TipoDeArchivo {
        constructor(
            public display: string,
            public icon: string
        ) { }
    }
}