/// <reference path="../_all.ts" />
var apArea;
(function (apArea) {
    var bottomSheetController = (function () {
        function bottomSheetController($mdBottomSheet) {
            this.$mdBottomSheet = $mdBottomSheet;
            this.items = [
                new bottomSheetItem("Ver Planilla General de Servicios", "planillaGeneral", "add"),
                new bottomSheetItem("Ver Resumen de Costos", "costos", "add")
            ];
        }
        return bottomSheetController;
    }());
    bottomSheetController.$inject = ['$mdBottomSheet'];
    apArea.bottomSheetController = bottomSheetController;
    var bottomSheetButtonController = (function () {
        function bottomSheetButtonController($mdBottomSheet) {
            this.$mdBottomSheet = $mdBottomSheet;
        }
        bottomSheetButtonController.prototype.mostrarBottomSheet = function () {
            this.$mdBottomSheet.show({
                templateUrl: './dist/ap/bottom-sheet.html',
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
        function bottomSheetItem(display, id, icon) {
            this.display = display;
            this.id = id;
            this.icon = icon;
        }
        return bottomSheetItem;
    }());
})(apArea || (apArea = {}));
//# sourceMappingURL=bottomSheetController.js.map