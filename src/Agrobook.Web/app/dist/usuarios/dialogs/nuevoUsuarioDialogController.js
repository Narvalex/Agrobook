/// <reference path="../../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var nuevoUsuarioDialogController = (function () {
        function nuevoUsuarioDialogController($mdDialog, usuariosService, toasterLite, config) {
            this.$mdDialog = $mdDialog;
            this.usuariosService = usuariosService;
            this.toasterLite = toasterLite;
            this.config = config;
            this.avatarUrls = [];
            this.tiposDeCuenta = [
                { tipo: 'Admin', desc: 'Administrador' },
                { tipo: 'Tecnico', desc: 'Técnico' },
                { tipo: 'Productor', desc: 'Productor' }
            ];
            this.bloquearSubmit = false;
            this.setDefaultSubmitText();
            this.avatarUrls = config.avatarUrls;
        }
        nuevoUsuarioDialogController.prototype.cancelar = function () {
            this.$mdDialog.cancel();
        };
        nuevoUsuarioDialogController.prototype.crearNuevoUsuario = function () {
            var _this = this;
            var nombre = this.usuario.nombreDeUsuario;
            this.setWorkingText();
            this.bloquearSubmit = true;
            this.usuario.claims = [this.tipoDeCuenta.tipo];
            this.usuariosService.crearNuevoUsuario(this.usuario, function (value) {
                _this.toasterLite.success('El usuario ' + nombre + ' fue creado exitosamente');
                _this.$mdDialog.hide(_this.usuario);
            }, function (reason) {
                _this.setDefaultSubmitText();
                _this.bloquearSubmit = false;
                _this.toasterLite.error('Ocurrió un error y no se pudo crear el usuario');
            });
        };
        nuevoUsuarioDialogController.prototype.setDefaultSubmitText = function () {
            this.submitLabel = 'Crear nuevo usuario';
        };
        nuevoUsuarioDialogController.prototype.setWorkingText = function () {
            this.submitLabel = 'Creando nuevo usuario...';
        };
        return nuevoUsuarioDialogController;
    }());
    nuevoUsuarioDialogController.$inject = ['$mdDialog', 'usuariosService', 'toasterLite', 'config'];
    usuariosArea.nuevoUsuarioDialogController = nuevoUsuarioDialogController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=nuevoUsuarioDialogController.js.map