/// <reference path="../../../_all.ts" />

module apArea {
    export class prodTabParcelasController {
        static $inject = [];

        constructor(
        ) {
            this.creandoNuevaParcela = false;
        }

        // Estados
        creandoNuevaParcela: boolean;

        // Objetos

        // Listas

        // Api
        habilitarCreacionDeNuevaParcela() {
            this.creandoNuevaParcela = true;
            setTimeout(() =>
                document.getElementById('nuevaParcelaInput').focus(), 0);
        }

        cancelarCreacionDeNuevaParcela() {
            this.creandoNuevaParcela = false;
        }

        // Privados
        
    }
}