/// <reference path="../../../_all.ts" />

module apArea {
    export class prodTabParcelasController {
        static $inject = ['config', 'apService', 'toasterLite'];

        constructor(
            private config: common.config,
            private apService: apService,
            private toasterLite: common.toasterLite
        ) {
            this.creandoNuevaParcela = false;
        }

        // Estados
        creandoNuevaParcela: boolean;
        intentandoRegistrarParcela: boolean;

        // Objetos
        nuevaParcela: nuevaParcelaDto;

        // Listas

        // Api
        habilitarCreacionDeNuevaParcela() {
            this.creandoNuevaParcela = true;
            setTimeout(() =>
                document.getElementById('nuevaParcelaInput').focus(), 0);
        }

        checkIfEnter($event) {
            var keyCode = $event.keyCode;
            if (keyCode === this.config.keyCodes.enter)
                this.registrarNuevaParcela();
            else if (keyCode === this.config.keyCodes.esc) {
                this.cancelarCreacionDeNuevaParcela();
            }
        }

        registrarNuevaParcela() {
            if (this.nuevaParcela.display.length === 0) {
                this.toasterLite.error("Debe especificar el nombre de la parcela");
                return;
            }
            this.intentandoRegistrarParcela = true;
            this.apService.registrarNuevaParcela(this.nuevaParcela.display,
                new common.callbackLite<parcelaDto>(
                    value => {
                        this.resetearNuevaParcelaInput();
                        this.intentandoRegistrarParcela = false;
                        this.creandoNuevaParcela = false;
                        this.toasterLite.success('Parcela creada')
                    },
                    reason => {
                        this.intentandoRegistrarParcela = false;
                        this.creandoNuevaParcela = false;
                    })
            );
        }

        cancelarCreacionDeNuevaParcela() {
            this.resetearNuevaParcelaInput();
            this.creandoNuevaParcela = false;
        }

        // Privados
        private resetearNuevaParcelaInput() {
            this.nuevaParcela = undefined;
        }
        
    }
}