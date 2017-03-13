/// <reference path="../../_all.ts" />
var common;
(function (common) {
    var userMenuWidgetController = (function () {
        function userMenuWidgetController($mdPanel) {
            this.$mdPanel = $mdPanel;
        }
        userMenuWidgetController.prototype.mostrarMenu = function ($event) {
            var panelConfig;
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
        };
        return userMenuWidgetController;
    }());
    userMenuWidgetController.$inject = ['$mdPanel'];
    common.userMenuWidgetController = userMenuWidgetController;
    var panelMenuController = (function () {
        function panelMenuController(mdPanelRef) {
            this.mdPanelRef = mdPanelRef;
        }
        return panelMenuController;
    }());
    panelMenuController.$inject = ['mdPanelRef', '$timeout'];
})(common || (common = {}));
//# sourceMappingURL=widgetController.js.map