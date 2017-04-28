/// <reference path="../../_all.ts" />

module usuariosArea {
    export class perfilController {
        static $inject = ['$routeParams', 'loginQueryService', 'usuariosService',
            'usuariosQueryService', 'toasterLite', 'config', '$rootScope', '$scope'];

        constructor(
            private $routeParams: angular.route.IRouteParamsService,
            private loginQueryService: login.loginQueryService,
            private usuariosService: usuariosService,
            private usuariosQueryService: usuariosQueryService,
            private toasterLite: common.toasterLite,
            private config: common.config,
            private $rootScope: ng.IRootScopeService,
            private $scope: ng.IScope
        ) {
            this.avatarUrls = config.avatarUrls;

            let idUsuario = this.$routeParams['idUsuario'];
            let usuario: login.loginResult;
            if (idUsuario === undefined)
                idUsuario = this.loginQueryService.tryGetLocalLoginInfo().usuario;

            this.usuariosQueryService.obtenerInfoBasicaDeUsuario(
                idUsuario,
                (value) => {
                    this.inicializarEdicionDeInfoBasica(value.data);
                    this.loaded = true;
                },
                (reason) => {
                    this.toasterLite.error('Ocurrió un error al recuperar información del usuario ' + idUsuario, this.toasterLite.delayForever);
                });

            this.$scope.$on(this.config.eventIndex.usuarios.perfilActualizado,
                (e, args: common.perfilActualizado) => {
                    this.inicializarEdicionDeInfoBasica(new usuarioInfoBasica(
                        args.usuario, args.nombreParaMostrar, args.avatarUrl));
                });
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

            var dto = new actualizarPerfilDto(
                this.usuarioRecuperado.nombre,
                this.usuarioEditado.avatarUrl,
                this.usuarioEditado.nombreParaMostrar,
                this.passwordActual,
                this.nuevoPassword);

            this.usuariosService.actualizarPerfil(
                dto,
                value => {
                    this.$rootScope.$broadcast(this.config.eventIndex.usuarios.perfilActualizado,
                    new common.perfilActualizado(dto.usuario, dto.avatarUrl, dto.nombreParaMostrar));
                    this.toasterLite.success('El perfil se ha actualizado exitosamente');
                },
                reason => this.toasterLite.error('Ocurrió un error al intentar actualizar el perfil')
            );
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

        private get perfilEstaEditado(): boolean {
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
                if (this.nuevoPassword !== this.nuevoPasswordConfirmacion) {
                    this.toasterLite.error('El password ingresado no coincide con la confirmación');
                    return false;
                }
            }

            return true;
        }
    }
}