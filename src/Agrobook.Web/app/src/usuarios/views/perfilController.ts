/// <reference path="../../_all.ts" />

module usuariosArea {
    export class perfilController {
        static $inject = ['$routeParams', 'loginQueryService', 'usuariosQueryService', 'toasterLite', 'config'];

        constructor(
            private $routeParams: angular.route.IRouteParamsService,
            private loginQueryService: login.loginQueryService,
            private usuariosQueryService: usuariosQueryService,
            private toasterLite: common.toasterLite,
            private config: common.config
        ) {
            this.avatarUrls = config.avatarUrls;

            var idUsuario = this.$routeParams['idUsuario'];
            if (idUsuario === undefined) {
                let usuario = this.loginQueryService.tryGetLocalLoginInfo();
                this.inicializarEdicionDeInfoBasica(new usuarioInfoBasica(usuario.usuario, usuario.nombreParaMostrar, usuario.avatarUrl));
                this.loaded = true;
            }
            else {
                this.usuariosQueryService.obtenerInfoBasicaDeUsuario(
                    idUsuario,
                    (value) => {
                        this.inicializarEdicionDeInfoBasica(value.data);
                        this.loaded = true;
                    },
                    (reason) => {
                        this.toasterLite.error('Ocurrió un error al recuperar información del usuario ' + idUsuario, this.toasterLite.delayForever);
                    });
            }
        }

        loaded: boolean = false;
        avatarUrls = [];
        usuarioRecuperado: usuarioInfoBasica;
        usuarioEditado: usuarioInfoBasica;

        private inicializarEdicionDeInfoBasica(usuarioRecuperado: usuarioInfoBasica) {
            this.usuarioRecuperado = usuarioRecuperado;
            this.usuarioEditado = new usuarioInfoBasica(
                usuarioRecuperado.nombre,
                usuarioRecuperado.nombreParaMostrar,
                usuarioRecuperado.avatarUrl);
        }
    }
}