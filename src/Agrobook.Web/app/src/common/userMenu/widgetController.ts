/// <reference path="../../_all.ts" />

module common {
    export class userMenuWidgetController {
        static $inject = ['config', '$mdPanel', 'loginQueryService', '$rootScope', '$scope'];

        constructor(
            private config: common.config,
            private $mdPanel: angular.material.IPanelService,
            private loginQueryService: login.loginQueryService,
            private $rootScope: angular.IRootScopeService,
            private $scope: ng.IScope
        ) {
            this.$rootScope.$on(this.config.eventIndex.login.loggedIn, (e, args) => {
                this.verificarLogueo();
            });
            this.$rootScope.$on(this.config.eventIndex.login.loggedOut, (e, args) => {
                this.verificarLogueo();
            });
            this.verificarLogueo();

            this.$scope.$on(this.config.eventIndex.usuarios.perfilActualizado,
                (e, args: common.perfilActualizado) => {
                    if (this.estaLogueado && this.usuario === args.usuario) {
                        this.nombreParaMostrar = args.nombreParaMostrar;
                        this.avatarUrl = args.avatarUrl;
                    }
                });
        }

        usuario: string;
        estaLogueado: boolean = false;
        nombreParaMostrar: string;
        avatarUrl: string;

        mostrarMenu($event: any): void {
            let panelConfig: angular.material.IPanelConfig;
            let position = this.$mdPanel
                .newPanelPosition()
                .relativeTo($event.target)
                .addPanelPosition(this.$mdPanel.xPosition.ALIGN_START, this.$mdPanel.yPosition.BELOW)
                .withOffsetX('-1px');

            panelConfig = {
                position: position,
                attachTo: angular.element(document.body),
                controller: panelMenuController,
                controllerAs: 'vm',
                templateUrl: 'dist/common/userMenu/menu-items-template.html',
                panelClass: 'menu-panel-container',
                openFrom: $event,
                clickOutsideToClose: true,
                escapeToClose: true,
                focusOnOpen: true,
                zIndex: 100
            };

            this.$mdPanel.open(panelConfig);
        }

        private verificarLogueo() {
            var result = this.loginQueryService.tryGetLocalLoginInfo();
            if (result !== undefined && result.loginExitoso) {
                this.estaLogueado = true;
                this.usuario = result.usuario;
                this.nombreParaMostrar = result.nombreParaMostrar;
                this.avatarUrl = result.avatarUrl;
            }
            else {
                this.estaLogueado = false;
            }
        }
    }

    class panelMenuController {
        static $inject = ['mdPanelRef', 'loginService'];
        private estaEnHome: boolean;

        constructor(
            private mdPanelRef: angular.material.IPanelRef,
            private loginService: login.loginService
        ) {
            this.estaEnHome = window.location.pathname == '/app/home.html';
        }

        logOut() {
            this.loginService.logOut();
            this.closeMenu();
            if (!this.estaEnHome)
                window.location.href = 'home.html';
        }

        closeMenu(): void {
            this.mdPanelRef.close();
        }

        seleccionarItem(item: menuItem): void {
            window.location.href = item.link;
        }

        menuItemList: menuItem[] = [
            new menuItem('Inicio', 'home.html'),
            new menuItem('Archivos', 'archivos.html'),
            new menuItem('Usuarios', 'usuarios.html')
        ]
    }

    class menuItem {
        constructor(
            public name: string,
            public link: string) {
        }
    }
}