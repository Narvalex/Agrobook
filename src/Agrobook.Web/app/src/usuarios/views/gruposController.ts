/// <reference path="../../_all.ts" />

module usuariosArea {
    export class gruposController {
        static $inject = ['usuariosService', 'usuariosQueryService', 'toasterLite', '$timeout', '$q', '$log'];

        constructor(
            private usuariosService: usuariosService,
            private usuariosQueryService: usuariosQueryService,
            private toasterLite: common.toasterLite,
            private $timeout: ng.ITimeoutService,
            private $q: ng.IQService,
            private $log: ng.ILogService
        ) {
            this.recuperarListaDeOrganizaciones();
        }

        simulateQuery = false;
        isDisabled = false;

        // list of `state` value/display objects
        states = this.loadAll();
        organizaciones: organizacionDto[];

        newState(state) {
            alert("Sorry! You'll need to create a Constitution for " + state + " first!");
        }

        // ******************************
        // Internal methods
        // ******************************

        private recuperarListaDeOrganizaciones() {
            this.usuariosQueryService.obtenerOrganizaciones(
                response => { this.organizaciones = response.data; },
                reason => this.toasterLite.error('Hubo un error al recuperar lista de organizaciones', this.toasterLite.delayForever)
            );
        }

        querySearch(query) {
            var results = query ? this.states.filter(this.createFilterFor(query)) : this.states,
                deferred;
            if (this.simulateQuery) {
                deferred = this.$q.defer();
                this.$timeout(function () { deferred.resolve(results); }, Math.random() * 1000, false);
                return deferred.promise;
            } else {
                return results;
            }
        }

        searchTextChange(text) {
            this.$log.info('Text changed to ' + text);
        }

        selectedItemChange(item) {
            this.$log.info('Item changed to ' + JSON.stringify(item));
        }

        /**
         * Build `states` list of key/value pairs
         */
        loadAll() {
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
        }

        /**
         * Create filter function for a query string
         */
        createFilterFor(query) {
            var lowercaseQuery = angular.lowercase(query);

            return function filterFn(state) {
                return (state.value.indexOf(lowercaseQuery) === 0);
            };

        }
    }
}