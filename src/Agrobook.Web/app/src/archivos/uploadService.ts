/// <reference path="../_all.ts" />

module archivosArea {
    export class uploadService extends common.httpLite {
        static $inject = ['$http', 'toasterLite', 'localStorageLite', 'config'];

        constructor(
            private $http: ng.IHttpService,
            private toasterLite: common.toasterLite,
            private localStorageLite: common.localStorageLite,
            private config: common.config
        ) {
            super($http, 'archivos/upload');

            this.loginInfo = this.localStorageLite.get<login.loginResult>(this.config.repoIndex.login.usuarioActual);
        }

        private loginInfo: login.loginResult;
        public uploadUnits: uploadUnit[] = [];
        public progress: any;

        private scope: ng.IScope;

        public setScope(scope: ng.IScope) {
            this.scope = scope;
            for (var i = 0; i < this.uploadUnits.length; i++) {
                this.uploadUnits[i].setNewScope(scope);
            }
        }

        public prepareFiles(files: File[], idProductor: string) {
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

                var unit = new uploadUnit(files[i], idProductor, this.scope, this.toasterLite, this.loginInfo.token);
                this.uploadUnits.push(unit);
                unit.showPreview();
            }
        }

        public removeFile(unit: uploadUnit) {
            if (unit.uploading) {
                this.toasterLite.error('No se puede remover una carga en progreso. Debe detenerla primero');
                return;
            }

            var file = unit.file;
            for (var i = 0; i < this.uploadUnits.length; i++) {
                if (this.uploadUnits[i].file.name === file.name && this.uploadUnits[i].file.webkitRelativePath === file.webkitRelativePath) {
                    this.uploadUnits.splice(i, 1);
                    break;
                }
            }
        }

        public clear() {
            var unit: uploadUnit;
            for (var i = 0; i < this.uploadUnits.length; i++) {
                if (!this.uploadUnits[i].uploading) {
                    unit = this.uploadUnits[i];
                    break;
                }
            }

            if (unit) {
                this.removeFile(unit);
                this.clear();
            }
        }

        public thereArePendingFiles(): boolean {
            for (var i = 0; i < this.uploadUnits.length; i++) {
                if (!this.uploadUnits[i].uploading && !this.uploadUnits[i].uploaded) {
                    return true;
                }
            }

            return false;
        }

        public thereAreFilesToBeCleansed(): boolean {
            for (var i = 0; i < this.uploadUnits.length; i++) {
                if (!this.uploadUnits[i].uploading || this.uploadUnits[i].uploaded) {
                    return true;
                }
            }

            return false;
        }

        public thereAreUploadsInProcess(): boolean {
            for (var i = 0; i < this.uploadUnits.length; i++) {
                if (this.uploadUnits[i].uploading === true) {
                    return true;
                }
            }

            return false;
        }

        public uploadAll() {
            for (var i = 0; i < this.uploadUnits.length; i++) {
                if (!this.uploadUnits[i].uploading && !this.uploadUnits[i].uploaded) {
                    this.uploadUnits[i].startUpload();
                }
            }
        }

        public stopAll() {
            for (var i = 0; i < this.uploadUnits.length; i++) {
                if (this.uploadUnits[i].uploading && !this.uploadUnits[i].uploaded) {
                    this.uploadUnits[i].stopUpload();
                }
            }
        }
    }

    export class uploadUnit {
        constructor(
            public file: File,
            private idProductor: string,
            private scope: ng.IScope,
            private toasterLite: common.toasterLite,
            private authToken: string
        ) {
            // this.progress = 0;
            var deconstruido = file.name.split('.');
            var extension = deconstruido.pop();
            var nombre = deconstruido.join('.');

            this.metadatos = {
                nombre: nombre,
                extension: extension,
                fecha: file.lastModifiedDate,
                desc: '',
                size: file.size, // in bytes
                idProductor: this.idProductor
            }
        }

        public xhr: XMLHttpRequest;
        public progress: any;
        public uploaded: boolean = false;
        public failed: boolean = false;
        public uploading: boolean = false;
        public editMode: boolean = false;
        public blockEdition: boolean = false;
        public esperandoAlServidor: boolean = false;
        public errorMessage: string;

        public iconUrl = '';

        public metadatos: { nombre: string, extension: string, fecha: Date, desc: string, size: number, idProductor: string };

        public toggleEditMode() {
            this.editMode = !this.editMode;
        }

        public setNewScope(scope: ng.IScope) {
            this.scope = scope;
        }

        public showPreview() {
            // Establecer preview
            var self = this;
            var nombre = this.metadatos.nombre;
            var elementId = '#preview-' + nombre;
            let ext = this.metadatos.extension;
            if (ext === 'jpg' || ext === 'JPG'
                || ext === 'png' || ext === 'PNG'
            ) {
                let reader = new FileReader();
                reader.onload = (e: any) => {
                    this.scope.$apply(() => {
                        //$(elementId).attr('src', e.target.result);
                        self.iconUrl = e.target.result;
                    });
                }

                reader.readAsDataURL(this.file);
            }
            else {
                var url = './assets/img/fileIcons/file.png';
                self.iconUrl = url;
            }
        }

        public stopUpload() {
            if (!this.xhr) return;
            if (this.esperandoAlServidor) {
                var message = 'No de detuvo la carga de ' + this.metadatos.nombre + ' porque esta esperando respuesta del servidor';
                this.toasterLite.info(message);
                console.log(message);
                return;
            }

            this.uploading = false;
            this.blockEdition = false;
            this.xhr.abort();
            this.progress = 0;
        }

        public startUpload() {
            var self = this;
            self.uploading = true;
            self.failed = false;
            self.blockEdition = true;

            function progress(e: ProgressEvent) {
                try {
                    if (!self.scope || !self.uploading)
                        return;

                    updateProgress(e);
                    self.scope.$apply(() => updateProgress(e));
                } catch (e) {
                    console.log('error in progress handler');
                }
            }

            function updateProgress(e: ProgressEvent) {
                if (e.lengthComputable) {
                    var value = Math.round(e.loaded * 100 / e.total);
                    self.progress = value === 100 ? 99 : value;
                }
                else {
                    self.progress = 'unable to compute';
                }
            }

            function load(e) {
                console.log('El archivo fue cargado exitosamente en el portal. falta en el servidor');
                self.scope.$apply(() => setEsperandoAlServidor());
                setEsperandoAlServidor();

            }

            function error(e) {
                self.toasterLite.error('Error al cargar archivo');
            }

            function abort(e) {
                console.log('Carga abortada');
            }

            function timeout(e) {
                console.log("timeout");
            }

            function setEsperandoAlServidor() {
                self.esperandoAlServidor = true;
            }

            function setUploaded() {
                self.progress = 100;
                self.uploaded = true;
                self.failed = false;
                self.uploading = false;
                self.esperandoAlServidor = false;
            }

            function setFailure(message: string) {
                self.failed = true;
                self.uploaded = false;
                self.uploading = false;
                self.progress = 0;
                self.blockEdition = false;
                self.esperandoAlServidor = false;
                self.errorMessage = message;
            }

            function readyStateChange(e) {
                console.log("ready state change. status:" + e.target.status + " " + e.target.statusText);
                var errorComun = 'Error al cargar archivo';
                var elArchivoYaExiste = 'Ya existe un archivo con ese nombre';
                switch (e.target.status) {
                    case 500:
                        setFailure(errorComun)
                        self.scope.$apply(() => {
                            setFailure(errorComun);
                        });
                        break;

                    case 200:
                        var serialized = e.target.response;
                        if (serialized === "")
                            return;
                        var result = JSON.parse(serialized) as { exitoso: boolean, yaExiste: boolean }
                        if (result.exitoso) {
                            setUploaded()
                            self.scope.$apply(() => {
                                setUploaded();
                            });

                            console.log('El archivo se recibio correctamente en el servidor');
                        }
                        else {
                            setFailure(elArchivoYaExiste)
                            self.scope.$apply(() => {
                                setFailure(elArchivoYaExiste);
                            });
                        }
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
            formData.append('metadatos', JSON.stringify(this.metadatos));


            // More info to try on edge: http://jsfiddle.net/pthoty2e/
            // Issue on edge: https://developer.microsoft.com/en-us/microsoft-edge/platform/issues/12224510/
            self.xhr = new XMLHttpRequest();

            self.xhr.upload.addEventListener("progress", progress, false);
            self.xhr.onprogress = progress;
            self.xhr.upload.addEventListener("load", load, false);
            self.xhr.addEventListener("error", error, false);
            self.xhr.addEventListener("abort", abort, false);
            self.xhr.addEventListener("timeout", timeout, false);
            self.xhr.addEventListener("readystatechange", readyStateChange, false);
            self.xhr.addEventListener("loadstart", loadStart, false);
            self.xhr.addEventListener("loadend", loadEnd, false);

            self.xhr.open("POST", "./archivos/upload", true);
            self.xhr.setRequestHeader("Authorization", this.authToken);
            try {
                self.xhr.send(formData);
            } catch (e) {
                console.log('error on send');
            }
        }
    }
}