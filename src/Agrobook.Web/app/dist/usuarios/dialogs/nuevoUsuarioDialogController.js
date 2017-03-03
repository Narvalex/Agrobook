/// <reference path="../../_all.ts" />
var UsuariosArea;
(function (UsuariosArea) {
    var nuevoUsuarioDialogController = (function () {
        function nuevoUsuarioDialogController($mdDialog) {
            this.$mdDialog = $mdDialog;
            this.avatarUrls = [
                '../app/assets/img/avatar/1.png',
                '../app/assets/img/avatar/2.png',
                '../app/assets/img/avatar/3.png',
                '../app/assets/img/avatar/4.png',
                '../app/assets/img/avatar/5.png',
                '../app/assets/img/avatar/6.png',
                '../app/assets/img/avatar/7.png',
                '../app/assets/img/avatar/8.png',
                '../app/assets/img/avatar/9.png'
            ];
        }
        nuevoUsuarioDialogController.prototype.cancelar = function () {
            this.$mdDialog.cancel();
        };
        nuevoUsuarioDialogController.prototype.crearNuevoUsuario = function () {
            this.$mdDialog.hide(this.usuario);
        };
        return nuevoUsuarioDialogController;
    }());
    UsuariosArea.nuevoUsuarioDialogController = nuevoUsuarioDialogController;
})(UsuariosArea || (UsuariosArea = {}));
//# sourceMappingURL=nuevoUsuarioDialogController.js.map