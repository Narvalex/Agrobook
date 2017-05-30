/// <reference path="../_all.ts" />
var archivosArea;
(function (archivosArea) {
    var prodSidenavController = (function () {
        function prodSidenavController($mdSidenav, toasterLite, archivosQueryService) {
            this.$mdSidenav = $mdSidenav;
            this.toasterLite = toasterLite;
            this.archivosQueryService = archivosQueryService;
            this.loaded = false;
            this.cargarListaDeProductores();
        }
        prodSidenavController.prototype.toggleSideNav = function () {
            this.$mdSidenav('right').toggle();
        };
        prodSidenavController.prototype.seleccionarProductor = function (productor) {
            this.productorSeleccionado = productor;
            window.location.replace('#!/archivos/' + productor.id);
            this.toggleSideNav();
        };
        // Internal
        prodSidenavController.prototype.cargarListaDeProductores = function () {
            var _this = this;
            this.archivosQueryService.obtenerListaDeProductores(function (value) {
                _this.productores = value.data;
                _this.loaded = true;
            }, function (reason) {
                _this.toasterLite.error('Hubo un error al recuperar la lista de productores.', _this.toasterLite.delayForever);
            });
        };
        return prodSidenavController;
    }());
    prodSidenavController.$inject = ['$mdSidenav', 'toasterLite', 'archivosQueryService'];
    archivosArea.prodSidenavController = prodSidenavController;
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=prodSidenavController.js.map