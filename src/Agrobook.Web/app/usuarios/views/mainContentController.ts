/// <reference path="../../_all.ts" />

module usuariosArea {
    export class mainContentController {
        static $inject = ['$routeParams', 'loginService', 'loginQueryService', 'usuariosQueryService', 'toasterLite', '$scope',
            'config'];

        constructor(
            private $routeParams: angular.route.IRouteParamsService,
            private loginService: login.loginService,
            private loginQueryService: login.loginQueryService,
            private usuariosQueryService: usuariosQueryService,
            private toasterLite: common.toasterLite,
            private $scope: ng.IScope,
            private config: common.config
        ) {
            // auth
            var claims = this.config.claims;
            this.mostrarOrganizacionesYGrupos = this.loginService.autorizar([
                claims.roles.Admin,
                claims.roles.Gerente,
                claims.roles.Tecnico
            ]);


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

            this.abrirTabCorrespondiente();
            this.$scope.$on('$routeUpdate', (scope, next, current) => {
                this.abrirTabCorrespondiente();
            });
        }

        loaded: boolean = false;
        tabIndex: number;
        usuario: usuarioInfoBasica;
        mostrarOrganizacionesYGrupos: boolean = false;

        onTabSelect(tabIndex: number) {
            if (this.usuario === undefined) return;

            let tabId: string;
            switch (tabIndex) {
                case 0: tabId = 'perfil'; break
                case 1: tabId = 'grupos'; break;
                case 2: tabId = 'organizaciones'; break;
                default: tabId = "perfil"
            }

            window.location.replace('#!/usuario/' + this.usuario.nombre + '?tab=' + tabId);
        }

        abrirTabCorrespondiente() {
            let tabId = this.$routeParams['tab'];
            switch (tabId) { 
                case 'perfil': this.tabIndex = 0; break;
                case 'grupos': this.tabIndex = 1; break;
                case 'organizaciones': this.tabIndex = 2; break;
                default: this.tabIndex = 0; break;
            }
        }
    }
}