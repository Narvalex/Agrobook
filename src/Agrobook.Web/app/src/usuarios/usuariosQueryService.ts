/// <reference path="../_all.ts" />

module usuariosArea {
    export class usuariosQueryService {
        private prefix = 'usuarios/query';

        static $inject = ['httpLite'];

        constructor(
            private httpLite: common.httpLite
        ) {
        }

        obtenerListaDeTodosLosUsuarios(
            onSuccess: (value: ng.IHttpPromiseCallbackArg<usuarioEnLista[]>) => void,
            onError: (reason: any) => void
        ) {
            
        }
    }
}