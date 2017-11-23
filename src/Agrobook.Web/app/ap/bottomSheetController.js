/// <reference path="../_all.ts" />
var apArea;
(function (apArea) {
    var bottomSheetController = (function () {
        function bottomSheetController($mdBottomSheet, $mdSidenav, loginService, config) {
            this.$mdBottomSheet = $mdBottomSheet;
            this.$mdSidenav = $mdSidenav;
            this.loginService = loginService;
            this.config = config;
            this.items = [
                new bottomSheetItem("Ir al inicio de Agricultura de Precisión", "home", './index.html#!/')
            ];
            var claims = this.config.claims;
            this.puedeVerReportes = this.loginService.autorizar([claims.roles.Gerente, claims.roles.Tecnico]);
            if (this.puedeVerReportes)
                this.items.push(new bottomSheetItem('Ver Reportes', 'description', './index.html#!/reportes'));
        }
        bottomSheetController.prototype.goTo = function (item) {
            window.location.replace(item.url);
            this.$mdSidenav('left').close();
            this.$mdBottomSheet.hide();
        };
        return bottomSheetController;
    }());
    bottomSheetController.$inject = ['$mdBottomSheet', '$mdSidenav', 'loginService', 'config'];
    apArea.bottomSheetController = bottomSheetController;
    var bottomSheetButtonController = (function () {
        function bottomSheetButtonController($mdBottomSheet) {
            this.$mdBottomSheet = $mdBottomSheet;
        }
        bottomSheetButtonController.prototype.mostrarBottomSheet = function () {
            this.$mdBottomSheet.show({
                templateUrl: './bottom-sheet.html',
                controller: 'bottomSheetController',
                controllerAs: 'vm'
            }).then(function (clickedItem) {
            }).catch(function (error) {
                // User clicked aoutside or hit escape
            });
        };
        return bottomSheetButtonController;
    }());
    bottomSheetButtonController.$inject = ['$mdBottomSheet'];
    apArea.bottomSheetButtonController = bottomSheetButtonController;
    var bottomSheetItem = (function () {
        function bottomSheetItem(display, icon, url) {
            this.display = display;
            this.icon = icon;
            this.url = url;
        }
        return bottomSheetItem;
    }());
})(apArea || (apArea = {}));
//# sourceMappingURL=bottomSheetController.js.map