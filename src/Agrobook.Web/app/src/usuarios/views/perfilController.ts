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

            this.usuarioLogueado = this.loginQueryService.tryGetLocalLoginInfo();

            let idUsuario = this.$routeParams['idUsuario'];
            let usuario: login.loginResult;
            if (idUsuario === undefined)
                idUsuario = this.usuarioLogueado.usuario;

            this.usuariosQueryService.obtenerInfoBasicaDeUsuario(
                idUsuario,
                (value) => {
                    this.inicializarEdicionDeInfoBasica(value.data);
                    this.infoBasicaLoaded = true;
                },
                (reason) => {
                    this.toasterLite.error('Ocurrió un error al recuperar información del usuario ' + idUsuario, this.toasterLite.delayForever);
                });

            this.usuariosQueryService.obtenerListaDeClaims(
                value => {
                    this.claims = value.data;
                    this.claimsLoaded = true;
                },
                reason => {
                    this.toasterLite.error("Error! Ver logs");
                });

            this.usuariosQueryService.obtenerClaimsDelUsuario(idUsuario,
                new common.callbackLite<claimDto[]>(
                    value => {
                        value.data.forEach(x => {
                            this.permisosOtorgados.push(x); // tenemos que pushear, no reemplazar
                        });
                        this.permisosOtorgadosLoaded = true;
                    },
                    reason => {
                        this.toasterLite.error("Error al recuperar claims");
                    })
            );

            this.$scope.$on(this.config.eventIndex.usuarios.perfilActualizado,
                (e, args: common.perfilActualizado) => {
                    this.inicializarEdicionDeInfoBasica(new usuarioInfoBasica(
                        args.usuario, args.nombreParaMostrar, args.avatarUrl));
                });
        }

        infoBasicaLoaded: boolean = false;
        claimsLoaded: boolean = false;
        permisosOtorgadosLoaded: boolean = false;
        allLoaded(): boolean { return this.infoBasicaLoaded && this.claimsLoaded && this.permisosOtorgadosLoaded; }

        // Claim Adding / Removal
        claims: claimDto[];
        permisosOtorgados: claimDto[] = []; // parece que debe estar inicializado para que los chips aparezcan
        aplicandoPermisos = false;

        avatarUrls = [];
        usuarioRecuperado: usuarioInfoBasica;
        usuarioEditado: usuarioInfoBasica;
        usuarioLogueado: login.loginResult;

        // Password
        nuevoPassword: string;
        nuevoPasswordConfirmacion: string;
        passwordActual: string;

        actualizarPerfil() {
            if (!this.perfilEstaEditado) {
                this.toasterLite.info('No hay nada para actualizar');
                return;
            }
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
                    this.toasterLite.info('El perfil se ha actualizado exitosamente. Atención: si actualizó su contraseña entonces usted debe volver a iniciar sesión.');
                },
                reason => this.toasterLite.error('Ocurrió un error al intentar actualizar el perfil')
            );
        }

        resetearPassword() {
            this.usuariosService.resetearPassword(this.usuarioRecuperado.nombre,
                value => this.toasterLite.success('Password reseteado exitosamente'),
                reason => this.toasterLite.error('Ocurrió un error al intentar resetear el password'));
        }

        public get perfilEstaEditado(): boolean {
            if (this.usuarioRecuperado === undefined)
                return false;
            if (this.usuarioRecuperado.avatarUrl !== this.usuarioEditado.avatarUrl)
                return true;
            if (this.usuarioRecuperado.nombreParaMostrar !== this.usuarioEditado.nombreParaMostrar)
                return true;
            if (this.seQuiereActualizarPassword)
                return true;
            return false;
        }

        public otorgarPermiso(claim: claimDto) {
            this.aplicandoPermisos = true;
            this.usuariosService.otorgarPermiso(this.usuarioRecuperado.nombre, claim.id,
                new common.callbackLite(
                    value => {
                        this.permisosOtorgados.push(claim);

                        this.toasterLite.success('Permiso otorgado');
                        this.aplicandoPermisos = false;
                    },
                    reason => {
                        this.toasterLite.error('No se pudo otorgar el permiso');

                        this.aplicandoPermisos = false;
                    })
            );
        }

        public retirarPermiso(claim: claimDto) {
            this.aplicandoPermisos = true;
            this.usuariosService.retirarPermiso(this.usuarioRecuperado.nombre, claim.id,
                new common.callbackLite(
                    value => {
                        for (var i = 0; i < this.permisosOtorgados.length; i++) {
                            if (this.permisosOtorgados[i].id === claim.id) {
                                this.permisosOtorgados.splice(i, 1);
                                break;
                            }
                        }

                        if (this.permisosOtorgados.length === 0) {
                            this.permisosOtorgados.push(
                                new claimDto('rol-invitado', 'Invitado',
                                    'Los invitados pueden ver los trabajos realizados a productores de su organización.')
                            );
                        }

                        this.toasterLite.success('Permiso retirado');
                        this.aplicandoPermisos = false;
                    },
                    reason => {
                        this.toasterLite.error('No se pudo retirar el permiso');

                        this.aplicandoPermisos = false;
                    })
            );
        }

        private inicializarEdicionDeInfoBasica(usuarioRecuperado: usuarioInfoBasica) {
            this.usuarioRecuperado = usuarioRecuperado;
            this.usuarioEditado = new usuarioInfoBasica(
                usuarioRecuperado.nombre,
                usuarioRecuperado.nombreParaMostrar,
                usuarioRecuperado.avatarUrl);
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