/// <reference path="../../_all.ts" />

module common {
    export class userMenuWidgetController {
        static $inject = ['$mdPanel'];

        constructor(
            private $mdPanel: angular.material.IPanelService) {
        }

        mostrarMenu($event: any): void {
            let panelConfig: angular.material.IPanelConfig;
            panelConfig = {
                position: this.$mdPanel
                    .newPanelPosition()
                    .relativeTo('#menu-btn')
                    .addPanelPosition(this.$mdPanel.xPosition.ALIGN_START, this.$mdPanel.yPosition.BELOW),
                attachTo: angular.element(document.body),
                controller: panelMenuController,
                controllerAs: 'vm',
                templateUrl: 'app/dist/common/userMenu/menu-items-template.html',
                panelClass: 'menu-template',
                openFrom: $event,
                clickOutsideToClose: true,
                escapeToClose: true,
                focusOnOpen: true,
                zIndex: 2
            };

            this.$mdPanel.open(panelConfig);
        }
    }

    class panelMenuController {
        static $inject = ['mdPanelRef', '$timeout'];

        constructor(
            private mdPanelRef: angular.material.IPanelRef) { }
    }
}