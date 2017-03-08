/// <reference path="../_all.ts" />
var HomeArea;
(function (HomeArea) {
    var ToolbarHeaderController = (function () {
        function ToolbarHeaderController(loginWriteService, config) {
            this.loginWriteService = loginWriteService;
            this.config = config;
        }
        ToolbarHeaderController.prototype.onInputKeyPress = function ($event) {
            if ($event.keyCode == this.config.keyCodes.enter)
                this.login();
        };
        ToolbarHeaderController.prototype.login = function () {
            //location.href = "areas/usuarios.html";)
            if (this.usuario == undefined || this.usuario == '') {
                window.alert('Por favor ingrese su usuario');
                return;
            }
            if (this.password == undefined || this.password == '') {
                window.alert('Por favor ingrese su contraseña');
                return;
            }
            this.loginWriteService.tryLogin(new login.credencialesDto(this.usuario, this.password))
                .then(function (response) {
                console.log(response.data);
            }, function (reason) {
                console.log(reason);
            });
        };
        return ToolbarHeaderController;
    }());
    ToolbarHeaderController.$inject = ['loginWriteService', 'config'];
    HomeArea.ToolbarHeaderController = ToolbarHeaderController;
})(HomeArea || (HomeArea = {}));
//# sourceMappingURL=toolbar-headerController.js.map