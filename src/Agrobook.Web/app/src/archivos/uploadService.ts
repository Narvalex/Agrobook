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

        public uploadUnits: uploadUnit[] = [];
        public progress: any;

        private scope: ng.IScope;

        public setScope(scope: ng.IScope) {
            this.scope = scope;
            for (var i = 0; i < this.uploadUnits.length; i++) {
                this.uploadUnits[i].setNewScope(scope);
            }
        }

        public prepareFiles(files: File[]) {
            for (var i = 0; i < files.length; i++) {
                var file = files[i];

                var yaExiste = false;
                for (var j = 0; j < this.uploadUnits.length; j++) {
                    if (this.uploadUnits[j].file.name === file.name && this.uploadUnits[j].file.webkitRelativePath === file.webkitRelativePath) {
                        var nombre = file.name ? file.name : file.webkitRelativePath;
                        this.toasterLite.info(`El archivo ${nombre} ya está en la lista.`);
                        yaExiste = true;
                        break;
                    }
                }

                if (yaExiste) continue;

                var unit = new uploadUnit(files[i], this.scope, this.toasterLite);
                this.uploadUnits.push(unit);
            }
        }

        public removeFile(file: File) {
            for (var i = 0; i < this.uploadUnits.length; i++) {
                if (this.uploadUnits[i].file.name === file.name && this.uploadUnits[i].file.webkitRelativePath === file.webkitRelativePath) {
                    this.uploadUnits[i].xhr.abort();
                    this.uploadUnits.splice(i, 1);
                    break;
                }
            }
        }
    }

    export class uploadUnit {
        constructor(
            public file: File,
            private scope: ng.IScope,
            private toasterLite: common.toasterLite
        ) {
            this.progress = 0;
        }

        public xhr: XMLHttpRequest;
        public progress: any;
        public success: boolean = false;
        public failed: boolean = false;
        public uploading: boolean = false;

        private successNotified = false;
        private failureNotified = false;

        public setNewScope(scope: ng.IScope) {
            this.scope = scope;
        }

        public startUpload() {
            var self = this;
            function progress(e) {
                try {
                    if (!self.scope)
                        return;

                    updateProgress(e);
                    self.scope.$apply(() => updateProgress(e));
                } catch (e) {
                    console.log('error in progress handler');
                }
            }

            function updateProgress(e) {
                if (e.lengthComputable) {
                    self.progress = Math.round(e.loaded * 100 / e.total);
                }
                else {
                    self.progress = 'unable to compute';
                }
            }

            function load(e) {
                console.log('El archivo fue cargado exitosamente en el portal. falta en el servidor');
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
                console.log("ready state change. status:" + e.target.status + " " + e.target.statusText);
                switch (e.target.status) {
                    case 500:
                        if (self.failureNotified) break;

                        self.failed = true;
                        self.failureNotified = true;
                        self.toasterLite.error('El archivo no se pudo alzar en el servidor');
                        break;

                    case 200:
                        if (self.successNotified) break;

                        self.success = true;
                        self.successNotified = true;
                        self.toasterLite.success('El archivo se recibio correctamente en el servidor');
                        break;

                    case 0:
                    case '':
                        break;

                    default:
                        self.toasterLite.error('Error desconocido al cargar archivo');
                        break;
                }
            }

            function loadStart(e) {
                console.log("load start");
            }

            function loadEnd(e) {
                console.log("load end");
            }

            var form = document.forms.namedItem('uploadForm');
            var formData = new FormData(form);
            formData.append('uploadedFile', self.file);

            self.xhr = new XMLHttpRequest();
            self.xhr.upload.addEventListener("progress", progress, false);
            self.xhr.upload.addEventListener("load", load, false);
            self.xhr.addEventListener("error", error, false);
            self.xhr.addEventListener("abort", abort, false);
            self.xhr.addEventListener("timeout", timeout, false);
            self.xhr.addEventListener("readystatechange", readyStateChange, false);
            self.xhr.addEventListener("loadstart", loadStart, false);
            self.xhr.addEventListener("loadend", loadEnd, false);
            self.xhr.open("POST", "./archivos/upload", true);
            try {
                self.xhr.send(formData);
            } catch (e) {
                console.log('error on send');
            }
        }
    }
}