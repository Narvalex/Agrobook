/// <reference path="../../_all.ts" />

module apArea {
    export class apService extends common.httpLite {
        static $inject = ['$http', 'fakeDb'];

        constructor(
            private $http: ng.IHttpService,
            private fakeDb: fakeDb
        ) {
            super($http, 'ap');
        }

        registrarNuevaParcela(
            dto: nuevaParcelaDto,
            callback: common.callbackLite<parcelaDto>
        ) {
            var data = new parcelaDto(dto.display.trim(), dto.idProd, dto.display, dto.hectareas);

            this.fakeDb.parcelas.push(data);
                 
            callback.onSuccess({
                data: data
            });
        }

        //--------------------------------
        // Fakes
        //--------------------------------

       
    }
}
