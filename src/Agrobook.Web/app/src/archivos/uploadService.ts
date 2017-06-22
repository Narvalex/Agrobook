/// <reference path="../_all.ts" />

module archivosArea {
    export class uploadService extends common.httpLite {
        static $inject = ['$http', 'toasterLite'];

        constructor(
            private $http: ng.IHttpService,
            private toasterLite: common.toasterLite
        ) {
            super($http, 'archivos/upload');
        }

        public files = [];
        public progress: any;

        public scopeListener: ng.IScope;

        public prepareFiles(files: any[]) {
            for (var i = 0; i < files.length; i++) {
                this.files.push(files[i]);
            }
        }

        public uploadFile(file: File) {
            var self = this;
            function progress(e) {
                try {
                    if (!self.scopeListener)
                        return;

                    self.scopeListener.$apply(() => {
                        if (e.lengthComputable) {
                            self.progress = Math.round(e.loaded * 100 / e.total);
                        }
                        else {
                            self.progress = 'unable to compute';
                        }
                    });
                } catch (e) {
                    console.log('error in progress handler');
                }
            }

            function load(e) {
                self.toasterLite.success('El archivo fue cargado exitosamente');
            }

            function error(e) {
                self.toasterLite.error('Error al cargar archivo');
            }

            function abort(e) {
                self.toasterLite.info('Carga abortada');
            }

            function timeout(e) {
                console.log("timeout");
            }

            function readyStateChange(e) {
                console.log("ready state change");
            }

            function loadStart(e) {
                console.log("load start");
            }

            function loadEnd(e) {
                console.log("load end");
            }

            var form = document.forms.namedItem('uploadForm');
            var formData = new FormData(form);
            formData.append('uploadedFile', file);

            var xhr = new XMLHttpRequest();
            xhr.upload.addEventListener("progress", progress, false);
            xhr.upload.addEventListener("load", load, false);
            xhr.addEventListener("error", error, false);
            xhr.addEventListener("abort", abort, false);
            xhr.addEventListener("timeout", timeout, false);
            xhr.addEventListener("readystatechange", readyStateChange, false);
            xhr.addEventListener("loadstart", loadStart, false);
            xhr.addEventListener("loadend", loadEnd, false);
            xhr.open("POST", "./archivos/upload", true);
            try {
                xhr.send(formData);
            } catch (e) {
                console.log('error on send');
            }
        }
    }
}