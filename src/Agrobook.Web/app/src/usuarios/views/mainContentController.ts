/// <reference path="../../_all.ts" />

module usuariosArea {
    export class mainContentController {
        static $inject = ['$routeParams', 'loginQueryService'];

        constructor(
            private $routeParams: angular.route.IRouteParamsService,
            private loginQueryService: login.loginQueryService
        ) {
            var idUsuario = this.$routeParams['idUsuario'];
            if (idUsuario === undefined) {
                let usuario = this.loginQueryService.tryGetLocalLoginInfo();
                idUsuario = usuario.usuario;
            }

            this.nombreCompleto = idUsuario;

        }

        nombreCompleto: string;
    }
}