/// <reference path="../_all.ts" />

module usuariosArea {
    export class usuariosQueryService {
        static $inject = ['httpLite'];

        constructor(
            private httpLite: common.httpLite
        ) {
            this.httpLite.prefix = 'usuarios/query';
        }

        obtenerListaDeTodosLosUsuarios(
            onSuccess: (value: ng.IHttpPromiseCallbackArg<usuarioInfoBasica[]>) => void,
            onError?: (reason: any) => void
        ) {
            this.httpLite.get('todos', onSuccess, onError);
        }

        obtenerInfoBasicaDeUsuario(
            usuario: string,
            onSuccess: (value: ng.IHttpPromiseCallbackArg<usuarioInfoBasica>) => void,
            onError?: (reason) => void
        ) {
            this.httpLite.get('info-basica/' + usuario, onSuccess, onError);
        }
    }
}