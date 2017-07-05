﻿/// <reference path="../_all.ts" />

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

        download(idProductor: string, nombre: string, extension: string) {
            // Could be improved here: https://stackoverflow.com/questions/24080018/download-file-from-an-asp-net-web-api-method-using-angularjs

            window.open(`./archivos/query/download/${idProductor}/${nombre}/${extension}`, '_blank', '');
        }
    }
}