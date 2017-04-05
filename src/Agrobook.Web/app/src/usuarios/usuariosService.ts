/// <reference path="../_all.ts" />

module usuariosArea {
    export class usuariosService {
        static $inject = ['httpLite'];

        constructor(
            private httpLite: common.httpLite
        ) {
            this.httpLite.prefix = 'usuarios';
        }

        crearNuevoUsuario(
            usuario: UsuarioDto,
            onSuccess: (value: ng.IHttpPromiseCallbackArg<{}>) => void,
            onError: (reason: any) => void
        ) {
            this.httpLite.post('crear-nuevo-usuario', usuario, onSuccess, onError);
        }
    }
}