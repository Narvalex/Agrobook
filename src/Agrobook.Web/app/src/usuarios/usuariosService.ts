﻿/// <reference path="../_all.ts" />

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

        crearNuevaOrganizacion(
            nombreOrg: string,
            onSuccess: (value: ng.IHttpPromiseCallbackArg<organizacionDto>) => void,
            onError: (reason: any) => void
        ) {
            this.post('crear-nueva-organizacion/' + nombreOrg, {}, onSuccess, onError);
        }

        crearNuevoGrupo(
            orgId: string,
            displayGrupoName: string,
            onSuccess: (value: ng.IHttpPromiseCallback<{}>) => void,
            onError: (reason: any) => void
        ) {
            this.post('crear-nuevo-grupo/' + orgId + '/' + displayGrupoName, {}, onSuccess, onError);
        }

        agregarUsuarioALaOrganizacion(
            idUsuario: string,
            idOrganizacion: string,
            onSuccess: (value: ng.IHttpPromiseCallback<{}>) => void,
            onError: (reason: any) => void
        ) {
            this.post(`agregar-usuario-a-la-organizacion/${idUsuario}/${idOrganizacion}`, {}, onSuccess, onError);
        }
    }
}