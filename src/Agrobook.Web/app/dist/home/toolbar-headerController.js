/// <reference path="../_all.ts" />
var HomeArea;
(function (HomeArea) {
    var ToolbarHeaderController = (function () {
        function ToolbarHeaderController(loginWriteService, config, $mdPanel) {
            this.loginWriteService = loginWriteService;
            this.config = config;
            this.$mdPanel = $mdPanel;
        }
        ToolbarHeaderController.prototype.onInputKeyPress = function ($event) {
            if ($event.keyCode == this.config.keyCodes.enter)
                this.login();
        };
        ToolbarHeaderController.prototype.login = function () {
            if (this.usuario == undefined || this.usuario == '') {
                window.alert('Por favor ingrese su usuario');
                return;
            }
            if (this.password == undefined || this.password == '') {
                window.alert('Por favor ingrese su contraseña');
                return;
            }
            this.loginWriteService.tryLogin(new login.credencialesDto(this.usuario, this.password), function (value) {
                console.log('Logueado!');
                location.href = "areas/usuarios.html";
            }, function (reason) { return console.log(reason); });
        };
        ToolbarHeaderController.prototype.showMenu = function ($event) {
            var position = this.$mdPanel
                .newPanelPosition()
                .relativeTo('menu-btn')
                .addPanelPosition(this.$mdPanel.xPosition.ALIGN_START, this.$mdPanel.yPosition.BELOW);
        };
        return ToolbarHeaderController;
    }());
    ToolbarHeaderController.$inject = ['loginWriteService', 'config', '$mdPanel'];
    HomeArea.ToolbarHeaderController = ToolbarHeaderController;
})(HomeArea || (HomeArea = {}));
//# sourceMappingURL=toolbar-headerController.js.map