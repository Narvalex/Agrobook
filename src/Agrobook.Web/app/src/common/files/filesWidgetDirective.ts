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
        private states: { pending: string, uploading: string }

        constructor(
            private $scope: ng.IScope,
            private toasterLite: common.toasterLite,
            private localStorageLite: common.localStorageLite,
            private config: common.config
        ) {
            var vm = this.$scope;
            vm.toasterLite = this.toasterLite;
            vm.loginInfo = this.localStorageLite.get<login.loginResult>(this.config.repoIndex.login.usuarioActual);
            vm.states = { pending: 'pending', uploading: 'uploading' };
            vm.fileInputId = vm.coleccionId + 'fileInputId';
            vm.addFiles = this.addFiles;
            vm.prepareFiles = this.prepareFiles;
            vm.removeFile = this.removeFile;
            vm.uploadFile = this.uploadFile;
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
                    let newName = file.name; // file.webkitRelativePath could be a name too
                    for (var j = 0; j < vm.units.length; j++) {
                        let existing = vm.units[j];
                        if (existing.name === newName) {
                            console.log('File "' + newName + '" was not added because already exists!');
                            alreadyExists = true;
                            break;
                        }
                    }

                    if (alreadyExists) continue;

                    let unit = new fileUnit(newName, vm.states.pending, file);
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
                vm.toasterLite.error('timeout');
            }

            function readyStateChange(e) {
                vm.toasterLite.error('ready state change');
            }

            function loadStart(e) {
                vm.toasterLite.error('load start');
            }

            function loadEnd(e) {
                vm.toasterLite.error('load end');
            }
        }
    }

    export class fileUnit {
        constructor(
            // icon an such later
            public name: string,
            public state: string,
            public file: File,

            // Presets
            public progress: any = 0,
            public waitingServer: boolean = false
        ) {
        }
    }
}