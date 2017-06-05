/// <reference path="../_all.ts" />
var archivosArea;
(function (archivosArea) {
    var sidenavController = (function () {
        function sidenavController($mdSidenav, toasterLite, $rootScope, config, $mdDialog) {
            var _this = this;
            this.$mdSidenav = $mdSidenav;
            this.toasterLite = toasterLite;
            this.$rootScope = $rootScope;
            this.config = config;
            this.$mdDialog = $mdDialog;
            this.$rootScope.$on(this.config.eventIndex.archivos.productorSeleccionado, function (e, args) {
                _this.idProductor = args;
            });
        }
        sidenavController.prototype.toggleSideNav = function () {
            this.$mdSidenav('left').toggle();
        };
        sidenavController.prototype.mostrarUploadCenter = function ($event) {
            //this.toasterLite.info(`nuevo archivo para ${this.idProductor}!`);
            this.$mdDialog.show({
                templateUrl: '../app/dist/archivos/dialogs/upload-center-dialog.html',
                parent: angular.element(document.body),
                targetEvent: $event,
                clickOutsideToClose: false,
                fullscreen: true
            })
                .then(function (answer) {
                console.log('Modal aceptado...');
            }, function () {
            });
        };
        return sidenavController;
    }());
    sidenavController.$inject = ['$mdSidenav', 'toasterLite', '$rootScope', 'config', '$mdDialog'];
    archivosArea.sidenavController = sidenavController;
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=sidenavController.js.map