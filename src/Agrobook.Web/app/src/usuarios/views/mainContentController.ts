﻿/// <reference path="../../_all.ts" />

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
            this.abrirElTabQueCorrespondeDesdeUrl();

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

            this.$scope.$on('$routeUpdate', (scope, next, current) => {
                this.abrirElTabQueCorrespondeDesdeUrl();
            });
        }

        loaded: boolean = false;
        tabIndex: number;
        usuario: usuarioInfoBasica;

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

        abrirElTabQueCorrespondeDesdeUrl() {
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