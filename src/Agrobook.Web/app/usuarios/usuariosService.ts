/// <reference path="../_all.ts" />

module usuariosArea {
    export class usuariosService extends common.httpLite {
        static $inject = ['$http'];

        constructor(
            private $http: ng.IHttpService
        ) {
            super($http, '../usuarios');
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

        cambiarNombreDeOrganizacion(idOrg: string, nombre: string, callback: common.callbackLite<any>) {
            let cmd = { idOrg: idOrg, nombre: nombre }
            this.postWithCallback('cambiar-nombre-de-organizacion', cmd, callback);
        }

        eliminarOrganizacion(org: organizacionDto, callback: common.callbackLite<{}>) {
            this.postWithCallback('eliminar-organizacion', { idOrg: org.id }, callback);
        }

        restaurarOrganizacion(org: organizacionDto, callback: common.callbackLite<{}>) {
            this.postWithCallback('restaurar-organizacion', { idOrg: org.id }, callback);
        }

        agregarUsuarioALaOrganizacion(
            idUsuario: string,
            idOrganizacion: string,
            onSuccess: (value: ng.IHttpPromiseCallback<{}>) => void,
            onError: (reason: any) => void
        ) {
            this.post(`agregar-usuario-a-la-organizacion/${idUsuario}/${idOrganizacion}`, {}, onSuccess, onError);
        }

        removerUsuarioDeOrganizacion(idUsuario: string, idOrganizacion: string, callback: common.callbackLite<any>) {
            let cmd = {
                idUsuario: idUsuario,
                idOrganizacion: idOrganizacion
            };
            this.postWithCallback('remover-usuario-de-organizacion', cmd, callback);
        }

        otorgarPermiso(
            idUsuario: string,
            permiso: string,
            callback: common.callbackLite<{}>
        ) {
            super.postWithCallback(`otorgar-permiso?usuario=${idUsuario}&permiso=${permiso}`, {}, callback); 
        }

        retirarPermiso(
            idUsuario: string,
            permiso: string,
            callback: common.callbackLite<{}>
        ) {
            super.postWithCallback(`retirar-permiso?usuario=${idUsuario}&permiso=${permiso}`, {}, callback);
        }
    }
}