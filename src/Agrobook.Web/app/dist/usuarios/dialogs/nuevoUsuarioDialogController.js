/// <reference path="../../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var nuevoUsuarioDialogController = (function () {
        function nuevoUsuarioDialogController($mdDialog, usuariosService) {
            this.$mdDialog = $mdDialog;
            this.usuariosService = usuariosService;
            this.avatarUrls = [
                './assets/img/avatar/1.png',
                './assets/img/avatar/2.png',
                './assets/img/avatar/3.png',
                './assets/img/avatar/4.png',
                './assets/img/avatar/5.png',
                './assets/img/avatar/6.png',
                './assets/img/avatar/7.png',
                './assets/img/avatar/8.png',
                './assets/img/avatar/9.png'
            ];
        }
        nuevoUsuarioDialogController.prototype.cancelar = function () {
            this.$mdDialog.cancel();
        };
        nuevoUsuarioDialogController.prototype.crearNuevoUsuario = function () {
            this.usuariosService.crearNuevoUsuario(this.usuario, function (value) {
            }, function (reason) {
            });
            this.$mdDialog.hide(this.usuario);
        };
        return nuevoUsuarioDialogController;
    }());
    nuevoUsuarioDialogController.$inject = ['$mdDialog', 'usuariosService'];
    usuariosArea.nuevoUsuarioDialogController = nuevoUsuarioDialogController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=nuevoUsuarioDialogController.js.map