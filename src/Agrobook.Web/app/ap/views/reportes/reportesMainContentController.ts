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

        getReporte(url: string) {
            window.open(url, '_blank', '');
        }
    }
}