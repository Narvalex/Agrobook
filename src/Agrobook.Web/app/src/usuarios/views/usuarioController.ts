/// <reference path="../../_all.ts" />

module usuariosArea {
    export class usuarioController {
        static $inject = ['$routeParams'];

        constructor(
            private $routeParams: angular.route.IRouteParamsService
        ) {
            var idUsuario = this.$routeParams['idUsuario'];
            this.nombreCompleto = idUsuario;

        }

        nombreCompleto: string;
    }
}