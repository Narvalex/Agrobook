/// <reference path="../../../../_all.ts" />
var apArea;
(function (apArea) {
    var precioFormDialogController = (function () {
        function precioFormDialogController($mdDialog) {
            this.$mdDialog = $mdDialog;
        }
        return precioFormDialogController;
    }());
    precioFormDialogController.$inject = ['$mdDialog'];
    apArea.precioFormDialogController = precioFormDialogController;
})(apArea || (apArea = {}));
//# sourceMappingURL=precioFormDialogController.js.map