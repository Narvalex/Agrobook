/// <reference path="../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var mainContentController = (function () {
        function mainContentController(config, $rootScope) {
            this.config = config;
            this.$rootScope = $rootScope;
            this.$inject = ['config', '$rootScope'];
            this.$rootScope.$on(this.config.eventIndex.usuarios.usuarioSeleccionado, this.onUsuarioSeleccionado);
        }
        mainContentController.prototype.onUsuarioSeleccionado = function (event, args) {
        };
        return mainContentController;
    }());
    usuariosArea.mainContentController = mainContentController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=mainContentController.js.map