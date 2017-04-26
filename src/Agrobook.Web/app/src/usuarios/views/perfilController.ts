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

        // Password
        nuevoPassword: string;
        nuevoPasswordConfirmacion: string;
        passwordActual: string;

        actualizarPerfil() {
            if (!this.perfilEstaEditado)
                this.toasterLite.info('No hay nada para actualizar');
            if (!this.intentarValidarEdicionDePerfil())
                return;

        }

        resetearPassword() {
            console.log('password reseteado');
        }

        private inicializarEdicionDeInfoBasica(usuarioRecuperado: usuarioInfoBasica) {
            this.usuarioRecuperado = usuarioRecuperado;
            this.usuarioEditado = new usuarioInfoBasica(
                usuarioRecuperado.nombre,
                usuarioRecuperado.nombreParaMostrar,
                usuarioRecuperado.avatarUrl);
        }

        private get perfilEstaEditado() : boolean {
            if (this.usuarioRecuperado.avatarUrl !== this.usuarioEditado.avatarUrl)
                return true;
            if (this.usuarioRecuperado.nombreParaMostrar !== this.usuarioEditado.nombreParaMostrar)
                return true;
            if (this.seQuiereActualizarPassword)
                return true;
            return false;
        }

        private get seQuiereActualizarPassword(): boolean {
            if (this.nuevoPassword !== undefined
                && this.nuevoPassword !== null
                && this.nuevoPassword !== '')
                return true;
            else
                return false;
        }

        private intentarValidarEdicionDePerfil(): boolean {
            if (this.seQuiereActualizarPassword) {
                if (this.passwordActual === undefined
                    || this.passwordActual === null
                    || this.passwordActual === '') {
                    this.toasterLite.error('Debe ingresar el password actual para actualizarlo.');
                    return false;
                }
                if (this.nuevoPassword !== this.nuevoPasswordConfirmacion)
                    this.toasterLite.error('El password ingresado no coincide con la confirmación');
                    return false;
            }

            return true;
        }
    }
}