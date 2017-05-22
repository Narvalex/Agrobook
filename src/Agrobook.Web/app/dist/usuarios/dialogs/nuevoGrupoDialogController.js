/// <reference path="../../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var nuevoGrupoDialogController = (function () {
        function nuevoGrupoDialogController($mdDialog, toasterLite, $rootScope) {
            this.$mdDialog = $mdDialog;
            this.toasterLite = toasterLite;
            this.$rootScope = $rootScope;
            this.bloquearSubmit = false;
            this.orgSeleccionada = $rootScope.gruposController.orgSeleccionada.display;
            this.setDefaultSubmitText();
        }
        nuevoGrupoDialogController.prototype.cancelar = function () {
            this.$mdDialog.cancel();
        };
        nuevoGrupoDialogController.prototype.setDefaultSubmitText = function () {
            this.submitLabel = 'Crear nuevo grupo';
        };
        return nuevoGrupoDialogController;
    }());
    nuevoGrupoDialogController.$inject = ['$mdDialog', 'toasterLite', '$rootScope'];
    usuariosArea.nuevoGrupoDialogController = nuevoGrupoDialogController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=nuevoGrupoDialogController.js.map