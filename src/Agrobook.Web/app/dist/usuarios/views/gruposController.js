/// <reference path="../../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var gruposController = (function () {
        function gruposController(usuariosService, usuariosQueryService, loginQueryService, toasterLite, $mdDialog, $timeout, $q, $log, $rootScope) {
            this.usuariosService = usuariosService;
            this.usuariosQueryService = usuariosQueryService;
            this.loginQueryService = loginQueryService;
            this.toasterLite = toasterLite;
            this.$mdDialog = $mdDialog;
            this.$timeout = $timeout;
            this.$q = $q;
            this.$log = $log;
            this.$rootScope = $rootScope;
            this.filterFromServer = false;
            this.isDisabled = false;
            // list of `organizaciones` value/display objects
            this.organizaciones = [];
            this.recuperarListaDeOrganizaciones();
            this.$rootScope.gruposController = {};
        }
        // ******************************
        // Public methods
        // ******************************
        gruposController.prototype.crearOrganizacion = function () {
            var idUsuario = this.loginQueryService.tryGetLocalLoginInfo().usuario;
            window.location.replace('#!/usuario/' + idUsuario + '?tab=organizaciones');
        };
        gruposController.prototype.crearNuevoGrupo = function ($event) {
            var _this = this;
            this.$rootScope.gruposController.orgSeleccionada = this.orgSeleccionada;
            this.$mdDialog.show({
                templateUrl: '../app/dist/usuarios/dialogs/nuevo-grupo-dialog.html',
                parent: angular.element(document.body),
                targetEvent: $event,
                controller: usuariosArea.nuevoGrupoDialogController,
                controllerAs: 'vm',
                clickOutsideToClose: true
            }).then(function (nuevoGrupo) {
                // Agregar nuevo grupo a la lista, si fue exitosa
            }, function () {
                _this.toasterLite.info('Creación de grupo cancelada');
            });
        };
        //********************************
        // Internal
        //********************************
        gruposController.prototype.noSePuedeCrearGrupo = function () {
            return this.orgSeleccionada === null || this.orgSeleccionada === undefined;
        };
        // ******************************
        // Autocomplete stuff
        // ******************************
        gruposController.prototype.recuperarListaDeOrganizaciones = function () {
            var _this = this;
            this.usuariosQueryService.obtenerOrganizaciones(function (response) {
                _this.organizaciones = response.data.map(function (org) {
                    return {
                        value: org.id,
                        display: org.display
                    };
                });
            }, function (reason) { return _this.toasterLite.error('Hubo un error al recuperar lista de organizaciones', _this.toasterLite.delayForever); });
        };
        gruposController.prototype.querySearch = function (query) {
            var results = query ? this.organizaciones.filter(this.createFilterFor(query)) : this.organizaciones, deferred;
            if (this.filterFromServer) {
                // this just simulates from Server. Add your server filtering here.
                deferred = this.$q.defer();
                this.$timeout(function () { deferred.resolve(results); }, Math.random() * 1000, false);
                return deferred.promise;
            }
            else {
                return results;
            }
        };
        gruposController.prototype.searchTextChange = function (text) {
            //this.$log.info('Text changed to ' + text);
        };
        gruposController.prototype.selectedItemChange = function (item) {
            this.$log.info('Item changed to ' + JSON.stringify(item));
        };
        /**
         * Create filter function for a query string
         */
        gruposController.prototype.createFilterFor = function (query) {
            var lowercaseQuery = angular.lowercase(query);
            return function filterFn(state) {
                return (state.value.indexOf(lowercaseQuery) === 0);
            };
        };
        return gruposController;
    }());
    gruposController.$inject = ['usuariosService', 'usuariosQueryService', 'loginQueryService', 'toasterLite',
        '$mdDialog', '$timeout', '$q', '$log', '$rootScope'];
    usuariosArea.gruposController = gruposController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=gruposController.js.map