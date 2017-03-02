/// <reference path="../../_all.ts" />
var UsuariosArea;
(function (UsuariosArea) {
    var nuevoUsuarioDialogController = (function () {
        function nuevoUsuarioDialogController($mdDialog) {
            this.$mdDialog = $mdDialog;
        }
        nuevoUsuarioDialogController.prototype.cancelar = function () {
            this.$mdDialog.cancel();
        };
        return nuevoUsuarioDialogController;
    }());
    UsuariosArea.nuevoUsuarioDialogController = nuevoUsuarioDialogController;
})(UsuariosArea || (UsuariosArea = {}));
//# sourceMappingURL=nuevoUsuarioDialogController.js.map