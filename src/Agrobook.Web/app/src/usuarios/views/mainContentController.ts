/// <reference path="../../_all.ts" />

module usuariosArea {
    export class mainContentController {
        static $inject = ['$routeParams', 'loginQueryService', 'usuariosQueryService', 'toasterLite', '$scope',
            'config'];

        constructor(
            private $routeParams: angular.route.IRouteParamsService,
            private loginQueryService: login.loginQueryService,
            private usuariosQueryService: usuariosQueryService,
            private toasterLite: common.toasterLite,
            private $scope: ng.IScope,
            private config: common.config
        ) {
            let idUsuario = this.$routeParams['idUsuario'];
            if (idUsuario === undefined)
                idUsuario = this.loginQueryService.tryGetLocalLoginInfo().usuario;

            this.usuariosQueryService.obtenerInfoBasicaDeUsuario(
                idUsuario,
                (value) => {
                    this.usuario = value.data;
                    this.loaded = true;
                },
                (reason) => {
                    this.toasterLite.error('Ocurrió un error al recuperar información del usuario ' + idUsuario, this.toasterLite.delayForever);
                });

            this.$scope.$on(this.config.eventIndex.usuarios.perfilActualizado,
                (e, args: common.perfilActualizado) => {
                    this.usuario = new usuarioInfoBasica(
                        this.usuario.nombre,
                        args.nombreParaMostrar,
                        args.avatarUrl);
                });
        }

        loaded: boolean = false;
        usuario: usuarioInfoBasica;
    }
}