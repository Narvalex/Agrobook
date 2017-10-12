/// <reference path="../../../_all.ts" />

module apArea {
    export class serviciosTabPrescripcionesController {
        static $inject = ['config', '$routeParams'];

        constructor(
            private config: common.config,
            private $routeParams: angular.route.IRouteParamsService,
        ) {
            let idProd = this.$routeParams['idProd'];
            this.idColeccion = `${this.config.categoriaDeArchivos.servicioPrescripciones}-${idProd}`;
        }

        // Objetos
        idColeccion: string;
    }
}