/// <reference path="../_all.ts" />
var homeArea;
(function (homeArea) {
    var ToolbarHeaderController = (function () {
        function ToolbarHeaderController(loginService, loginQueryService, config, $rootScope) {
            var _this = this;
            this.loginService = loginService;
            this.loginQueryService = loginQueryService;
            this.config = config;
            this.$rootScope = $rootScope;
            this.estaLogueado = false;
            this.$rootScope.$on(this.config.eventIndex.login.loggedOut, function (e, a) {
                _this.verificarSiEstaLogueado();
            });
            this.verificarSiEstaLogueado();
        }
        ToolbarHeaderController.prototype.onInputKeyPress = function ($event) {
            if ($event.keyCode == this.config.keyCodes.enter)
                this.login();
        };
        ToolbarHeaderController.prototype.login = function () {
            var _this = this;
            if (this.usuario == undefined || this.usuario == '') {
                window.alert('Por favor ingrese su usuario');
                return;
            }
            if (this.password == undefined || this.password == '') {
                window.alert('Por favor ingrese su contraseña');
                return;
            }
            this.loginService.tryLogin(new login.credencialesDto(this.usuario, this.password), function (value) {
                if (value.data.loginExitoso) {
                    _this.establecerUsuarioLogueado(value.data.nombreParaMostrar);
                }
                else {
                    window.alert("Credenciales inválidas");
                    _this.password = "";
                }
            }, function (reason) { return console.log(reason); });
        };
        ToolbarHeaderController.prototype.verificarSiEstaLogueado = function () {
            var result = this.loginQueryService.tryGetLocalLoginInfo();
            if (result === undefined || !result.loginExitoso) {
                this.estaLogueado = false;
            }
            else {
                this.establecerUsuarioLogueado(result.nombreParaMostrar);
            }
        };
        ToolbarHeaderController.prototype.establecerUsuarioLogueado = function (nombreParaMostrar) {
            this.nombreParaMostrar = nombreParaMostrar;
            this.estaLogueado = true;
        };
        return ToolbarHeaderController;
    }());
    ToolbarHeaderController.$inject = ['loginService', 'loginQueryService', 'config', '$rootScope'];
    homeArea.ToolbarHeaderController = ToolbarHeaderController;
})(homeArea || (homeArea = {}));
//# sourceMappingURL=toolbar-headerController.js.map