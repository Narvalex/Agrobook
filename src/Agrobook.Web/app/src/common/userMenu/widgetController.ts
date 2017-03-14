/// <reference path="../../_all.ts" />

module common {
    export class userMenuWidgetController {
        static $inject = ['$mdPanel'];

        constructor(
            private $mdPanel: angular.material.IPanelService) {
        }

        mostrarMenu($event: any): void {
            let panelConfig: angular.material.IPanelConfig;
            let position = this.$mdPanel
                .newPanelPosition()
                .relativeTo($event.target)
                .addPanelPosition(this.$mdPanel.xPosition.ALIGN_START, this.$mdPanel.yPosition.BELOW)
                .withOffsetX('-75px');

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
    }

    class panelMenuController {
        static $inject = ['mdPanelRef'];

        constructor(
            private mdPanelRef: angular.material.IPanelRef) { }

        closeMenu(): void {
            this.mdPanelRef.close();
        }

        seleccionarItem(item: menuItem): void {
            window.location.href = item.link;
        }

        menuItemList: menuItem[] = [
            new menuItem('Inicio', 'home.html'),
            new menuItem('Usuarios', 'usuarios.html'),
            new menuItem('Siscole', 'http://ti.fecoprod.com.py/siscole')
        ]
    }

    class menuItem {
        constructor(
            public name: string,
            public link: string) {
        }
    }
}