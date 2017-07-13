/// <reference path="../_all.ts" />

module archivosArea {
    export class archivosQueryService extends common.httpLite {
        static $inject = ['$http', 'loginQueryService', 'config'];

        constructor(
            private $http: ng.IHttpService,
            private loginQueryService: login.loginQueryService,
            private config: common.config
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
            var userInfo = this.loginQueryService.tryGetLocalLoginInfo();
            var usuario = userInfo.usuario;
            window.open(`./archivos/query/download/${idProductor}/${nombre}/${extension}/${usuario}`, '_blank', '');
        }

        resolverIcono(ext: string) {
            var tipos = this.config.tiposDeArchivos;
            if (this.esPdf(ext)) {
                return tipos.pdf.icon;
            } 
            else if (this.esMapa(ext)) {
                return tipos.mapas.icon;
            }
            else if (this.esFoto(ext)) {
                return tipos.fotos.icon;
            }
            else if (this.esExcel(ext)) {
                return tipos.excel.icon;
            }
            else if (this.esWord(ext)) {
                return tipos.word.icon;
            }
            else if (this.esPowerPoint(ext)) {
                return tipos.powerPoint.icon;
            }
            else 
                return tipos.generico.icon;
        }

        // Es algo section

        esPdf(extension: string): boolean {
            if (extension === undefined) return false;
            extension = extension.toLowerCase();
            return extension === 'pdf';
        }

        esFoto(extension: string): boolean {
            if (extension === undefined) return false;
            extension = extension.toLowerCase();
            return extension === 'jpg'
                || extension === 'jpeg';
        }

        esMapa(extension: string): boolean {
            if (extension === undefined) return false;
            extension = extension.toLowerCase();
            return extension === 'kmz'
                || extension === 'kml';
        }

        esExcel(extension: string): boolean {
            if (extension === undefined) return false;
            extension = extension.toLowerCase();
            return extension === 'xls'
                || extension === 'xlsx';
        }

        esWord(extension: string): boolean {
            if (extension === undefined) return false;
            extension = extension.toLowerCase();
            return extension === 'doc'
                || extension === 'docx'
                || extension === 'text'
                || extension === 'rtf';
        }

        esPowerPoint(extension: string): boolean {
            if (extension === undefined) return false;
            extension = extension.toLowerCase();
            return extension === 'ppt'
                || extension === 'pptx';
        }
    }
}