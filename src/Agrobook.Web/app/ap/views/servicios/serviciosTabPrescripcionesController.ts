/// <reference path="../../../_all.ts" />

module apArea {
    export class serviciosTabPrescripcionesController {
        static $inject = ['config', '$routeParams'];

        constructor(
            private config: common.config,
            private $routeParams: angular.route.IRouteParamsService,
        ) {
            let idServicio = this.$routeParams['idServicio'];
            this.idColeccion = `${this.config.categoriaDeArchivos.servicioPrescripciones}-${idServicio}`;
        }

        // Objetos
        idColeccion: string;
    }
}