/// <reference path="../_all.ts" />

module archivosArea {
    export class archivosQueryService extends common.httpLite {
        static $inject = ['$http'];

        constructor(
            private $http: ng.IHttpService
        ) {
            super($http, 'archivos/query');
        }

        obtenerListaDeProductores(
            onSuccess: (value: ng.IHttpPromiseCallbackArg<productorDto[]>) => void,
            onError?: (reason: any) => void
        ) {
            super.get('productores', onSuccess, onError);
        }

        obtenerArchivosDelProductor(
            idProductor: string,
            onSuccess: (value: ng.IHttpPromiseCallbackArg<archivoDto[]>) => void,
            onError?: (reason: any) => void
        ) {
            super.get('archivos-del-productor/' + idProductor, onSuccess, onError);
        }
    }
}