/// <reference path="../_all.ts" />
var Home;
(function (Home) {
    var ToolbarHeaderController = (function () {
        function ToolbarHeaderController(loginWriteService) {
            this.loginWriteService = loginWriteService;
        }
        ToolbarHeaderController.prototype.login = function () {
            //location.href = "areas/usuarios.html";
            this.loginWriteService.tryLogin(new login.credencialesDto(this.usuario, this.password))
                .then(function (response) {
                console.log(response.data);
            }, function (reason) {
                console.log(reason);
            });
        };
        return ToolbarHeaderController;
    }());
    ToolbarHeaderController.$inject = ['loginWriteService'];
    Home.ToolbarHeaderController = ToolbarHeaderController;
})(Home || (Home = {}));
//# sourceMappingURL=toolbar-headerController.js.map