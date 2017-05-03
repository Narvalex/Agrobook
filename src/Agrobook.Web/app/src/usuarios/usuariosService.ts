/// <reference path="../_all.ts" />

module usuariosArea {
    export class usuariosService extends common.httpLite {
        static $inject = ['$http'];

        constructor(
            private $http: ng.IHttpService
        ) {
            super($http, 'usuarios');
        }

        crearNuevoUsuario(
            usuario: usuarioDto,
            onSuccess: (value: ng.IHttpPromiseCallbackArg<{}>) => void,
            onError: (reason: any) => void
        ) {
            this.post('crear-nuevo-usuario', usuario, onSuccess, onError);
        }

        actualizarPerfil(
            usuario: actualizarPerfilDto,
            onSuccess: (value: ng.IHttpPromiseCallbackArg<{}>) => void,
            onError: (reason: any) => void
        ) {
            this.post('actualizar-perfil', usuario, onSuccess, onError);
        }

        resetearPassword(
            usuario: string,
            onSuccess: (value: ng.IHttpPromiseCallback<{}>) => void,
            onError: (reason: any) => void
        ) {
            this.post('resetear-password/' + usuario, {}, onSuccess, onError);
        }
    }
}