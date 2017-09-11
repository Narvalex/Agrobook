/// <reference path="../../_all.ts" />

module common {
    // https://weblogs.asp.net/dwahlin/creating-custom-angularjs-directives-part-i-the-fundamentals
    // https://docs.angularjs.org/guide/directive

    export function filesWidgetDirectiveFactory() : ng.IDirective {
        return {
            restrict: 'EA', //E = element, A = attribute, C = class, M = comment
            scope: {
                coleccionId: '=', // a que coleccion pertenece el archivo. {archivos}-{prodId}
                header: '='
            },
            templateUrl: './dist/common/files/files-widget.html',
            controller: filesWidgetController //Embed a custom controller in the directive,
        };
    }

    class filesWidgetController  {
        static $inject = ['$scope', 'toasterLite', 'localStorageLite', 'config'];
        private states: { pending: string, uploading: string, uploaded: string, uploadFailed: string }

        constructor(
            private $scope: ng.IScope,
            private toasterLite: common.toasterLite,
            private localStorageLite: common.localStorageLite,
            private config: common.config
        ) {
            var vm = this.$scope;
            vm.toasterLite = this.toasterLite;
            vm.loginInfo = this.localStorageLite.get<login.loginResult>(this.config.repoIndex.login.usuarioActual);
            vm.states = { pending: 'pending', uploading: 'uploading', uploaded: 'uploaded', uploadFailed: 'uploadFailed' };
            vm.fileInputId = vm.coleccionId + 'fileInputId';
            vm.addFiles = this.addFiles;
            vm.prepareFiles = this.prepareFiles;
            vm.removeFile = this.removeFile;
            vm.uploadFile = this.uploadFile;
            vm.setIconUrlAndSvgs = this.setIconUrlAndSvgs;
            vm.units = [];
        }

        // two-way binding
        scope: ng.IScope;
        title: string
        coleccionId: string;

        // object 
        loginInfo: login.loginResult;
        fileInputId: string;

        // lists
        units: fileUnit[];

        // angular typing
        $apply(action: () => any) {
        }

        addFiles() {
            document.getElementById(this.fileInputId).click();
        }

        prepareFiles(element: HTMLInputElement) {
            // reset input first
            let container = element.parentElement;
            let content = container.innerHTML;
            container.innerHTML = content;

            // try load to current list;
            var vm = this;
            this.$apply(() => {
                let files = element.files;
                for (var i = 0; i < files.length; i++) {
                    let file = files[i];

                    let alreadyExists = false;
                    let newName = file.name ? file.name : file.webkitRelativePath;
                    for (var j = 0; j < vm.units.length; j++) {
                        let existing = vm.units[j];
                        if (existing.name === newName) {
                            console.log('File "' + newName + '" was not added because already exists!');
                            alreadyExists = true;
                            break;
                        }
                    }

                    if (alreadyExists) continue;

                    let unit = new fileUnit(newName, vm.states.pending, file, file.size,
                        file.size > 1024 * 1024 ? `${(file.size / 1024 / 1024).toFixed(1)} MB` : `${(file.size / 1024).toFixed(1)} KB`);
                    vm.setIconUrlAndSvgs(unit);
                    vm.units.push(unit);

                    console.log('File "' + newName + '" was added');
                }
            });
        }

        removeFile(unit: fileUnit) {
            if (unit.state === this.states.uploading) {
                this.toasterLite.error('No se puede remover una carga en proceso');
                return;
            }

            for (var i = 0; i < this.units.length; i++) {
                let current = this.units[i];
                if (current.name === unit.name) {
                    this.units.splice(i, 1);
                    break;
                }
            }
        }

        setIconUrlAndSvgs(unit: fileUnit) {
            var deconstructed = unit.name.split('.');
            var ext = deconstructed.pop().toLowerCase();

            if (ext === 'jpg' || ext === 'jpeg' || ext === 'png') {
                unit.isAPicture = true;
                unit.iconSvg = 'picture';

                var vm = this;
                let reader = new FileReader();
                reader.onload = (e: any) => {
                    vm.$apply(() => {
                        unit.iconUrl = e.target.result;
                    });
                };

                reader.readAsDataURL(unit.file);
            }
            else {
                unit.isAPicture = false;
                unit.iconUrl = './assets/img/fileIcons/file.png';
                let iconSvg: string;
                if (ext === 'pdf') {
                    iconSvg = 'pdf';
                }
                else if (ext === 'jpg' || ext === 'jpeg' || ext === 'png') {
                    iconSvg = 'picture';
                }
                else if (ext === 'kmz' || ext === 'kml') {
                    iconSvg = 'google-earth';
                }
                else if (ext === 'xls' || ext === 'xlsx') {
                    iconSvg = 'excel';
                }
                else if (ext === 'doc' || ext === 'docx' || ext === 'text' || ext === 'txt' || ext === 'rtf') {
                    iconSvg = 'word';
                }
                else if (ext === 'ppt' || ext === 'pptx') {
                    iconSvg = 'powerPoint';
                }
                else {
                    iconSvg = 'generic-file';
                }
                unit.iconSvg = iconSvg;
            }
        }

        uploadFile(unit: fileUnit) {
            var vm = this;
            unit.state = vm.states.uploading;

            var form = document.forms.namedItem('uploadForm');
            var formData = new FormData(form);
            formData.append('uploadedFile', unit.file);
            formData.append('metadatos', JSON.stringify({ nombre: 'nombre', extension: 'extension', fecha: new Date(), desc: 'desc', size: 1000, coleccionId: 'fulano' }));

            // More info to try on edge: http://jsfiddle.net/pthoty2e/
            // Issue on edge: https://developer.microsoft.com/en-us/microsoft-edge/platform/issues/12224510/
            var xhr = new XMLHttpRequest();
            unit.stopUpload = stopUpload;
            xhr.upload.addEventListener('progress', progress, false);
            xhr.onprogress = progress;
            xhr.upload.addEventListener('load', load, false);
            xhr.addEventListener('error', error, false);
            xhr.addEventListener("abort", abort, false);
            xhr.addEventListener("timeout", timeout, false);
            xhr.addEventListener("readystatechange", readyStateChange, false);
            xhr.addEventListener("loadstart", loadStart, false);
            xhr.addEventListener("loadend", loadEnd, false);

            xhr.open("POST", "./archivos/upload/v2", true);
            xhr.setRequestHeader("Authorization", vm.loginInfo.token);
            try {
                xhr.send(formData);
            } catch (e) {
                console.log('error on send');
            }

            function stopUpload() {
                if (!xhr) return;
                if (unit.waitingServer) {
                    console.log('Upload can not be canceled. Is waiting for server response');
                    return;
                }

                xhr.abort();
                setFailure('Carga cancelada');
            }

            function progress(e: ProgressEvent) {
                try {
                    if (!vm || unit.state !== vm.states.uploading)
                        return;

                    updateProgress(e);
                    vm.$apply(() => updateProgress(e));
                } catch (e) {
                    console.log('Error in progress handler', e);
                }
            }

            function updateProgress(e: ProgressEvent) {
                if (e.lengthComputable) {
                    var value = Math.round(e.loaded * 100 / e.total);
                    unit.progress = value === 100 ? 99 : value;
                }
                else {
                    unit.progress = 'Unable to compute';
                }
            }

            function load(e) {
                console.log('El archivo fue cargado exitosamente en el portal. Falta en el servidor');
                unit.waitingServer = true;
                vm.$apply(() => unit.waitingServer = true);
            }

            function error(e) {
                vm.toasterLite.error('Error al cargar archivo');
            }

            function abort(e) {
                vm.toasterLite.error('Carga abortada');
            }

            function timeout(e) {
                vm.toasterLite.error('El servidor de carga dio timeout...');
            }

            function readyStateChange(e) {
                console.log("ready state change. status:" + e.target.status + " " + e.target.statusText);
                var errorNoEspecificado = 'Error al cargar archivo';
                var elArchivoYaExiste = 'Ya existe un archivo con ese nombre';
                switch (e.target.status) {
                    case 500:
                        setFailure(errorNoEspecificado)
                        vm.$apply(() => {
                            setFailure(errorNoEspecificado);
                        });
                        break;

                    case 200:
                        var serialized = e.target.response;
                        if (serialized === "")
                            return;
                        var result = JSON.parse(serialized) as { exitoso: boolean, yaExiste: boolean }
                        if (result.exitoso) {
                            setUploaded()
                            vm.$apply(() => {
                                setUploaded();
                            });

                            console.log('El archivo se recibio correctamente en el servidor');
                        }
                        else {
                            setFailure(elArchivoYaExiste)
                            vm.$apply(() => {
                                setFailure(elArchivoYaExiste);
                            });
                        }
                        break;

                    case 0:
                    case '':
                        break;

                    default:
                        vm.toasterLite.error('Error desconocido al cargar archivo');
                        break;
                }
            }

            function loadStart(e) {
                console.log('Load start');
            }

            function loadEnd(e) {
                console.log('Load end');
            }

            // Helpers
            function setUploaded() {
                unit.progress = 100;
                unit.state = vm.states.uploaded;
                unit.justUploaded = true,
                unit.waitingServer = false;
            }

            function setFailure(message: string) {
                unit.progress = 0;

                unit.state = vm.states.uploadFailed;
                unit.waitingServer = false;
                unit.errorMessage = message;
            }
        }
    }

    export class fileUnit {
        constructor(
            public name: string,
            public state: string,
            public file: File,
            public size: number, // in bytes
            public formattedSize: string,

            // Presets
            public iconUrl: string = '',
            public iconSvg: string = '',
            public isAPicture: boolean = false,
            public progress: any = 0,
            public waitingServer: boolean = false,
            public justUploaded: boolean = false,
            public errorMessage: string = '',
            // Methods
            public stopUpload: () => any = null,
        ) {
        }
    }
}