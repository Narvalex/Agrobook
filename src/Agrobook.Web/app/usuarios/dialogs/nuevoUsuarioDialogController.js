/// <reference path="../../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var nuevoUsuarioDialogController = (function () {
        function nuevoUsuarioDialogController($mdDialog, usuariosService, usuariosQueryService, toasterLite, config) {
            this.$mdDialog = $mdDialog;
            this.usuariosService = usuariosService;
            this.usuariosQueryService = usuariosQueryService;
            this.toasterLite = toasterLite;
            this.config = config;
            this.claimsLoaded = false;
            this.avatarUrls = [];
            this.claimsSeleccionados = [];
            this.bloquearSubmit = false;
            this.setDefaultSubmitText();
            this.avatarUrls = config.avatarUrls;
            this.obtenerListaDeClaims();
        }
        nuevoUsuarioDialogController.prototype.cancelar = function () {
            this.$mdDialog.cancel();
        };
        nuevoUsuarioDialogController.prototype.agregarClaim = function (claim) {
            for (var i = 0; i < this.claimsSeleccionados.length; i++) {
                if (this.claimsSeleccionados[i].id === claim.id)
                    return;
            }
            this.claimsSeleccionados.push(claim);
        };
        nuevoUsuarioDialogController.prototype.quitarClaim = function (claim) {
            for (var i = 0; i < this.claimsSeleccionados.length; i++) {
                if (this.claimsSeleccionados[i].id == claim.id) {
                    this.claimsSeleccionados.splice(i, 1);
                    break;
                }
            }
        };
        nuevoUsuarioDialogController.prototype.crearNuevoUsuario = function () {
            var _this = this;
            var nombre = this.usuario.nombreDeUsuario;
            // Validacion
            if (nombre.indexOf(' ') !== -1) {
                this.toasterLite.error('El nombre de usuario no debe contener espacios en blanco');
                return;
            }
            this.setWorkingText();
            this.bloquearSubmit = true;
            if (this.usuario.claims === undefined || this.usuario.claims.length === 0)
                // probablemente con el teclado selecciono el claim!
                this.agregarClaim(this.claimSelected);
            this.usuario.claims = this.claimsSeleccionados.map(function (c) { return c.id; });
            this.usuariosService.crearNuevoUsuario(this.usuario, function (value) {
                _this.toasterLite.success('El usuario ' + nombre + ' fue creado exitosamente');
                _this.$mdDialog.hide(_this.usuario);
            }, function (reason) {
                _this.setDefaultSubmitText();
                _this.bloquearSubmit = false;
                _this.toasterLite.error('Ocurrió un error y no se pudo crear el usuario');
            });
        };
        //****************************
        // Interfal stuff
        //****************************
        nuevoUsuarioDialogController.prototype.obtenerListaDeClaims = function () {
            var _this = this;
            this.usuariosQueryService.obtenerListaDeClaims(function (response) {
                _this.claimsLoaded = true;
                _this.claims = response.data;
            }, function (reason) { _this.toasterLite.error('Ocurrió un error al recuperar lista de claims', _this.toasterLite.delayForever); });
        };
        nuevoUsuarioDialogController.prototype.setDefaultSubmitText = function () {
            this.submitLabel = 'Crear nuevo usuario';
        };
        nuevoUsuarioDialogController.prototype.setWorkingText = function () {
            this.submitLabel = 'Creando nuevo usuario...';
        };
        return nuevoUsuarioDialogController;
    }());
    nuevoUsuarioDialogController.$inject = ['$mdDialog', 'usuariosService', 'usuariosQueryService', 'toasterLite', 'config'];
    usuariosArea.nuevoUsuarioDialogController = nuevoUsuarioDialogController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=nuevoUsuarioDialogController.js.map