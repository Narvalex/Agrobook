/// <reference path="../../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var nuevoGrupoDialogController = (function () {
        function nuevoGrupoDialogController($mdDialog, toasterLite, $rootScope, usuariosService) {
            this.$mdDialog = $mdDialog;
            this.toasterLite = toasterLite;
            this.$rootScope = $rootScope;
            this.usuariosService = usuariosService;
            this.bloquearSubmit = false;
            this.orgSeleccionada = $rootScope.gruposController.orgSeleccionada;
            this.setDefaultSubmitText();
        }
        nuevoGrupoDialogController.prototype.crearNuevoGrupo = function () {
            var _this = this;
            this.usuariosService.crearNuevoGrupo(this.orgSeleccionada.id, this.nuevoGrupo, function (response) {
                _this.toasterLite.success("Nuevo grupo " + _this.nuevoGrupo + " creado para " + _this.orgSeleccionada.display + ".");
                _this.$mdDialog.hide(response.data);
            }, function (reason) { _this.toasterLite.error('Ocurrio un error!'); });
        };
        nuevoGrupoDialogController.prototype.cancelar = function () {
            this.$mdDialog.cancel();
        };
        // Internal
        //********************
        nuevoGrupoDialogController.prototype.setDefaultSubmitText = function () {
            this.submitLabel = 'Crear nuevo grupo';
        };
        return nuevoGrupoDialogController;
    }());
    nuevoGrupoDialogController.$inject = ['$mdDialog', 'toasterLite', '$rootScope', 'usuariosService'];
    usuariosArea.nuevoGrupoDialogController = nuevoGrupoDialogController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=nuevoGrupoDialogController.js.map