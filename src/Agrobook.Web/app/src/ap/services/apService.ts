/// <reference path="../../_all.ts" />

module apArea {
    export class apService extends common.httpLite {
        static $inject = ['$http'];

        constructor(
            private $http: ng.IHttpService,
            private $q: ng.IQService
        ) {
            super($http, 'ap');
        }

        registrarNuevaParcela(
            nombre: string,
            callback: common.callbackLite<parcelaDto>
        ) {
            var dto = null;//new parcelaDto(nombre.trim(), nombre);
                 
            callback.onSuccess({
                data: dto
            });
        }

        //--------------------------------
        // Fakes
        //--------------------------------

       
    }
}
