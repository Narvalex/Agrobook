/// <reference path="../../../_all.ts" />

module apArea {
    export class prodTabParcelasController {
        static $inject = ['config', 'apService', 'apQueryService', 'toasterLite', '$routeParams'];

        constructor(
            private config: common.config,
            private apService: apService,
            private apQueryService: apQueryService,
            private toasterLite: common.toasterLite,
            private $routeParams: angular.route.IRouteParamsService
        ) {
            this.creandoNuevaParcela = false;

            this.idProd = this.$routeParams['idProd'];

            this.obtenerParcelasDelProd();
        }

        // Estados
        creandoNuevaParcela: boolean;
        intentandoRegistrarParcela: boolean;

        // Objetos
        idProd: string;
        nuevaParcela: nuevaParcelaDto;

        // Listas
        parcelas: parcelaDto[];

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
            this.apService.registrarNuevaParcela(this.nuevaParcela,
                new common.callbackLite<parcelaDto>(
                    value => {
                        this.resetearNuevaParcelaInput();
                        this.intentandoRegistrarParcela = false;
                        this.creandoNuevaParcela = false;
                        this.parcelas.push(value.data);
                        this.toasterLite.success('Parcela creada')
                    },
                    reason => {
                        this.intentandoRegistrarParcela = false;
                        this.creandoNuevaParcela = false;
                    })
            );
        }

        cancelarCreacionDeNuevaParcela() {
            this.creandoNuevaParcela = false;
            this.resetearNuevaParcelaInput();
        }

        // Privados
        private resetearNuevaParcelaInput() {
            this.nuevaParcela = undefined;
        }

        private obtenerParcelasDelProd() {
            this.apQueryService.gerParcelasDelProd(this.idProd,
                new common.callbackLite<parcelaDto[]>(
                    response => {
                        this.parcelas = response.data;
                    },
                    reason => { })
            );
        }
    }
}