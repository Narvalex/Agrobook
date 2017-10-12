/// <reference path="../_all.ts" />

module usuariosArea {
    export class usuariosQueryService extends common.httpLite {
        static $inject = ['$http'];

        constructor(
            private $http: ng.IHttpService
        ) {
            super($http, '../usuarios/query');
        }

        obtenerListaDeTodosLosUsuarios(
            onSuccess: (value: ng.IHttpPromiseCallbackArg<usuarioInfoBasica[]>) => void,
            onError?: (reason: any) => void
        ) {
            super.get('todos', onSuccess, onError);
        }

        obtenerListaDeClaims(
            onSuccess: (value: ng.IHttpPromiseCallbackArg<claimDto[]>) => void,
            onError?: (reason: any) => void
        ) {
            super.get('claims', onSuccess, onError);
        }

        obtenerClaimsDelUsuario(
            idUsuario: string,
            callback: common.callbackLite<claimDto[]>
        ) {
            super.getWithCallback('claims/' + idUsuario, callback);
        }

        obtenerInfoBasicaDeUsuario(
            usuario: string,
            onSuccess: (value: ng.IHttpPromiseCallbackArg<usuarioInfoBasica>) => void,
            onError?: (reason) => void
        ) {
            super.get('info-basica/' + usuario, onSuccess, onError);
        }

        obtenerOrganizacionesDelUsuario(
            usuarioId: string,
            onSuccess: (value: ng.IHttpPromiseCallbackArg<organizacionDto[]>) => void,
            onError?: (reason) => void
        ) {
            super.get('organizaciones/' + usuarioId, onSuccess, onError);
        }

        obtenerOrganizacionesMarcadasDelUsuario(
            usuarioId: string,
            onSuccess: (value: ng.IHttpPromiseCallbackArg<organizacionDto[]>) => void,
            onError?: (reason) => void
        ) {
            super.get('organizaciones-marcadas-del-usuario/' + usuarioId, onSuccess, onError);
        }

        obtenerOrganizaciones(
            onSuccess: (value: ng.IHttpPromiseCallbackArg<organizacionDto[]>) => void,
            onError?: (reason) => void
        ) {
            super.get('organizaciones', onSuccess, onError);
        }

        // TODO: Maybe? obtenerTodosLosGrupos
        obtenerGrupos(
            organizacionId: string,
            idUsuario: string,
            onSuccess: (value: ng.IHttpPromiseCallbackArg<grupoDto[]>) => void,
            onError?: (reason) => void
        ) {
            super.get(`grupos/${organizacionId}/${idUsuario}`, onSuccess, onError);
        }
    }
}