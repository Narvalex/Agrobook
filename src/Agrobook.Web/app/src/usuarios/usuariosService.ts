/// <reference path="../_all.ts" />

module usuariosArea {
    export class usuariosService {
        private prefix = 'usuarios/';

        static $inject = ['$http'];

        constructor(
            private $http: angular.IHttpService)
        { }

        private post<TResult>(url: string, dto: any): ng.IHttpPromise<TResult> {
            return this.$http.post(this.prefix + url, dto);
        }

        crearNuevoUsuario(
            usuario: UsuarioDto,
            s: (value: ng.IHttpPromiseCallbackArg<{}>) => void,
            e: (reason: any) => void): void {
            this.post<{}>('crear-nuevo-usuario', usuario).then(
                value => { s(value); },
                reason => { e(reason); }
            );
        }
    }
}