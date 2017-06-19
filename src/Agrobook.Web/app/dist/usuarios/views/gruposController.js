/// <reference path="../../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var gruposController = (function () {
        function gruposController(usuariosService, usuariosQueryService, loginQueryService, toasterLite, $mdDialog, $timeout, $q, $log, $rootScope, $routeParams) {
            this.usuariosService = usuariosService;
            this.usuariosQueryService = usuariosQueryService;
            this.loginQueryService = loginQueryService;
            this.toasterLite = toasterLite;
            this.$mdDialog = $mdDialog;
            this.$timeout = $timeout;
            this.$q = $q;
            this.$log = $log;
            this.$rootScope = $rootScope;
            this.$routeParams = $routeParams;
            // loading org
            this.loaded = false;
            // loading grupos for an org
            this.gruposLoaded = true;
            this.filterFromServer = false;
            this.isDisabled = false;
            // list of `organizaciones` value/display objects
            this.organizaciones = [];
            // list of grupos
            this.grupos = [];
            this.idUsuario = this.$routeParams['idUsuario'];
            if (this.idUsuario === undefined)
                this.idUsuario = this.loginQueryService.tryGetLocalLoginInfo().usuario;
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
            this.usuariosQueryService.obtenerOrganizacionesDelUsuario(this.idUsuario, function (response) {
                _this.organizaciones = response.data;
                _this.loaded = true;
            }, function (reason) { return _this.toasterLite.error('Hubo un error al recuperar lista de organizaciones', _this.toasterLite.delayForever); });
        };
        gruposController.prototype.querySearch = function (query) {
            var lowercaseQuery = angular.lowercase(query);
            var results = query
                ? this.organizaciones.filter(function (org) {
                    var coincideConId = (angular.lowercase(org.id).indexOf(lowercaseQuery) > -1);
                    var coincideConDisplay = (angular.lowercase(org.display).indexOf(lowercaseQuery) > -1);
                    return coincideConId || coincideConDisplay;
                })
                : this.organizaciones;
            return results;
        };
        gruposController.prototype.searchTextChange = function (text) {
            //this.$log.info('Text changed to ' + text);
        };
        gruposController.prototype.selectedItemChange = function (org) {
            var _this = this;
            if (org === undefined) {
                // dejo en blanco el filtro
                this.grupos = [];
                return;
            }
            this.gruposLoaded = false;
            this.usuariosQueryService.obtenerGrupos(org.id, function (value) {
                _this.gruposLoaded = true;
                _this.grupos = value.data;
            }, function (reason) { _this.toasterLite.error('Error al cargar grupos', _this.toasterLite.delayForever); });
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
        '$mdDialog', '$timeout', '$q', '$log', '$rootScope', '$routeParams'];
    usuariosArea.gruposController = gruposController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=gruposController.js.map