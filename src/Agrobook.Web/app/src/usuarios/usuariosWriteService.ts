/// <reference path="../_all.ts" />

module UsuariosArea {
    export class usuariosWriteService {
        static $inject = ['$http'];

        constructor(
            private $http: angular.IHttpService) 
        { }

        crearNuevoUsuario(usuario: UsuarioDto): any {
            return this.$http.post('', usuario);
        }
    }
}