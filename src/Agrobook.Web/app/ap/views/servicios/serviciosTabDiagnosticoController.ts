/// <reference path="../../../_all.ts" />

module apArea {
    export class serviciosTabDiagnosticoController {
        static $inject = ['config', '$routeParams'];

        constructor(
            private config: common.config,
            private $routeParams: angular.route.IRouteParamsService,
        ) {
            let idServicio = this.$routeParams['idServicio'];
            this.idColeccion = `${this.config.categoriaDeArchivos.servicioDiagnostico}-${idServicio}`;
        }

        // Objetos
        idColeccion: string;
    }
}