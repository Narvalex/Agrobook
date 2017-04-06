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
            onSuccess: (value: ng.IHttpPromiseCallbackArg<usuarioEnLista[]>) => void,
            onError?: (reason: any) => void
        ) {
            this.httpLite.get('todos', onSuccess, onError);
        }
    }
}