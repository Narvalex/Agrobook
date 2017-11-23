/// <reference path="../../../_all.ts" />

module apArea {
    export class reportesMainContentController {
        static $inject = ['config', 'loginService', 'toasterLite']

        constructor(
            private config: common.config,
            private loginService: login.loginService,
            private toasterLite: common.toasterLite
        ) {
        }

        getReporteListaDeProductores() {
            window.open('./report/lista-de-productores', '_blank', '');
        }
    }
}