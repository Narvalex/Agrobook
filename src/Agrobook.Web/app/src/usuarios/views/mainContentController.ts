/// <reference path="../../_all.ts" />

module usuariosArea {
    export class mainContentController {
        static $inject = ['$routeParams', 'loginQueryService', 'usuariosQueryService', 'toasterLite'];

        constructor(
            private $routeParams: angular.route.IRouteParamsService,
            private loginQueryService: login.loginQueryService,
            private usuariosQueryService: usuariosQueryService,
            private toasterLite: common.toasterLite
        ) {
            var idUsuario = this.$routeParams['idUsuario'];
            if (idUsuario === undefined) {
                let usuario = this.loginQueryService.tryGetLocalLoginInfo();
                this.usuario = new usuarioInfoBasica(usuario.usuario, usuario.nombreParaMostrar, usuario.avatarUrl);
                this.loaded = true;
            }
            else {
                this.usuariosQueryService.obtenerInfoBasicaDeUsuario(
                    idUsuario,
                    (value) => {
                        this.usuario = value.data;
                        this.loaded = true;
                    },
                    (reason) => {
                        this.toasterLite.error('Ocurrió un error al recuperar información del usuario ' + idUsuario, this.toasterLite.delayForever);
                    });
            }

        }

        loaded: boolean = false;
        usuario: usuarioInfoBasica;
    }
}