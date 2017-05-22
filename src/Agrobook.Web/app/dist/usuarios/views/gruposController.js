/// <reference path="../../_all.ts" />
var usuariosArea;
(function (usuariosArea) {
    var gruposController = (function () {
        function gruposController(usuariosService, usuariosQueryService, toasterLite, $timeout, $q, $log) {
            this.usuariosService = usuariosService;
            this.usuariosQueryService = usuariosQueryService;
            this.toasterLite = toasterLite;
            this.$timeout = $timeout;
            this.$q = $q;
            this.$log = $log;
            this.simulateQuery = false;
            this.isDisabled = false;
            // list of `state` value/display objects
            this.states = this.loadAll();
            this.recuperarListaDeOrganizaciones();
        }
        gruposController.prototype.newState = function (state) {
            alert("Sorry! You'll need to create a Constitution for " + state + " first!");
        };
        // ******************************
        // Internal methods
        // ******************************
        gruposController.prototype.recuperarListaDeOrganizaciones = function () {
            var _this = this;
            this.usuariosQueryService.obtenerOrganizaciones(function (response) { _this.organizaciones = response.data; }, function (reason) { return _this.toasterLite.error('Hubo un error al recuperar lista de organizaciones', _this.toasterLite.delayForever); });
        };
        gruposController.prototype.querySearch = function (query) {
            var results = query ? this.states.filter(this.createFilterFor(query)) : this.states, deferred;
            if (this.simulateQuery) {
                deferred = this.$q.defer();
                this.$timeout(function () { deferred.resolve(results); }, Math.random() * 1000, false);
                return deferred.promise;
            }
            else {
                return results;
            }
        };
        gruposController.prototype.searchTextChange = function (text) {
            this.$log.info('Text changed to ' + text);
        };
        gruposController.prototype.selectedItemChange = function (item) {
            this.$log.info('Item changed to ' + JSON.stringify(item));
        };
        /**
         * Build `states` list of key/value pairs
         */
        gruposController.prototype.loadAll = function () {
            var allStates = 'Alabama, Alaska, Arizona, Arkansas, California, Colorado, Connecticut, Delaware,\
              Florida, Georgia, Hawaii, Idaho, Illinois, Indiana, Iowa, Kansas, Kentucky, Louisiana,\
              Maine, Maryland, Massachusetts, Michigan, Minnesota, Mississippi, Missouri, Montana,\
              Nebraska, Nevada, New Hampshire, New Jersey, New Mexico, New York, North Carolina,\
              North Dakota, Ohio, Oklahoma, Oregon, Pennsylvania, Rhode Island, South Carolina,\
              South Dakota, Tennessee, Texas, Utah, Vermont, Virginia, Washington, West Virginia,\
              Wisconsin, Wyoming';
            return allStates.split(/, +/g).map(function (state) {
                return {
                    value: state.toLowerCase(),
                    display: state
                };
            });
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
    gruposController.$inject = ['usuariosService', 'usuariosQueryService', 'toasterLite', '$timeout', '$q', '$log'];
    usuariosArea.gruposController = gruposController;
})(usuariosArea || (usuariosArea = {}));
//# sourceMappingURL=gruposController.js.map