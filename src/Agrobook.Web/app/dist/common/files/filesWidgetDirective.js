/// <reference path="../../_all.ts" />
var common;
(function (common) {
    // https://weblogs.asp.net/dwahlin/creating-custom-angularjs-directives-part-i-the-fundamentals
    // https://docs.angularjs.org/guide/directive
    function filesWidgetDirectiveFactory() {
        return {
            restrict: 'EA',
            scope: {
                idColeccion: '=',
                header: '='
            },
            templateUrl: './dist/common/files/files-widget.html',
            controller: filesWidgetController //Embed a custom controller in the directive,
        };
    }
    common.filesWidgetDirectiveFactory = filesWidgetDirectiveFactory;
    var filesWidgetController = (function () {
        function filesWidgetController($scope, toasterLite, localStorageLite, config, $http) {
            this.$scope = $scope;
            this.toasterLite = toasterLite;
            this.localStorageLite = localStorageLite;
            this.config = config;
            this.$http = $http;
            var vm = this.$scope;
            vm.toasterLite = this.toasterLite;
            vm.loginInfo = this.localStorageLite.get(this.config.repoIndex.login.usuarioActual);
            vm.states = { pending: 'pending', uploading: 'uploading', uploaded: 'uploaded', uploadFailed: 'uploadFailed' };
            vm.fileInputId = vm.idColeccion + 'fileInputId';
            vm.addFiles = this.addFiles;
            vm.prepareFiles = this.prepareFiles;
            vm.removeFile = this.removeFile;
            vm.uploadFile = this.uploadFile;
            vm.downloadFile = this.downloadFile;
            vm.setIconUrlAndSvgs = this.setIconUrlAndSvgs;
            vm.units = [];
            $http.get('archivos/query/coleccion/' + vm.idColeccion).then(function (value) {
                for (var i = 0; i < value.data.length; i++) {
                    var meta = value.data[i];
                    var unit = new fileUnit(meta.nombre, meta.extension, vm.states.uploaded, null, meta.size, meta.size > 1024 * 1024 ? (meta.size / 1024 / 1024).toFixed(1) + " MB" : (meta.size / 1024).toFixed(1) + " KB");
                    vm.setIconUrlAndSvgs(unit);
                    vm.units.push(unit);
                }
            }, function (response) {
                vm.toasterLite.error('Hubo un error al recuperar los archivos cargados');
            });
        }
        // angular typing
        filesWidgetController.prototype.$apply = function (action) {
        };
        filesWidgetController.prototype.addFiles = function () {
            document.getElementById(this.fileInputId).click();
        };
        filesWidgetController.prototype.prepareFiles = function (element) {
            // reset input first
            var container = element.parentElement;
            var content = container.innerHTML;
            container.innerHTML = content;
            // try load to current list;
            var vm = this;
            this.$apply(function () {
                var files = element.files;
                for (var i = 0; i < files.length; i++) {
                    var file = files[i];
                    var alreadyExists = false;
                    var newName = file.name ? file.name : file.webkitRelativePath;
                    for (var j = 0; j < vm.units.length; j++) {
                        var existing = vm.units[j];
                        if (existing.name === newName) {
                            console.log('File "' + newName + '" was not added because already exists!');
                            alreadyExists = true;
                            break;
                        }
                    }
                    if (alreadyExists)
                        continue;
                    var deconstructed = newName.split('.');
                    var ext = deconstructed.pop().toLowerCase();
                    var unit = new fileUnit(newName, ext, vm.states.pending, file, file.size, file.size > 1024 * 1024 ? (file.size / 1024 / 1024).toFixed(1) + " MB" : (file.size / 1024).toFixed(1) + " KB");
                    vm.setIconUrlAndSvgs(unit);
                    vm.units.push(unit);
                    console.log('File "' + newName + '" was added');
                }
            });
        };
        filesWidgetController.prototype.removeFile = function (unit) {
            if (unit.state === this.states.uploading) {
                this.toasterLite.error('No se puede remover una carga en proceso');
                return;
            }
            for (var i = 0; i < this.units.length; i++) {
                var current = this.units[i];
                if (current.name === unit.name) {
                    this.units.splice(i, 1);
                    break;
                }
            }
        };
        filesWidgetController.prototype.setIconUrlAndSvgs = function (unit) {
            var ext = unit.extension;
            if (ext === 'jpg' || ext === 'jpeg' || ext === 'png') {
                unit.isAPicture = true;
                unit.iconSvg = 'picture';
                var vm = this;
                var reader = new FileReader();
                reader.onload = function (e) {
                    vm.$apply(function () {
                        unit.iconUrl = e.target.result;
                    });
                };
                reader.readAsDataURL(unit.file);
            }
            else {
                unit.isAPicture = false;
                unit.iconUrl = './assets/img/fileIcons/file.png';
                var iconSvg = void 0;
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
        };
        filesWidgetController.prototype.downloadFile = function (unit) {
            // Could be improved here: https://stackoverflow.com/questions/24080018/download-file-from-an-asp-net-web-api-method-using-angularjs
            window.open("./archivos/query/download/" + this.idColeccion + "/" + unit.name + "/" + this.loginInfo.usuario, '_blank', '');
        };
        filesWidgetController.prototype.uploadFile = function (unit) {
            var vm = this;
            unit.state = vm.states.uploading;
            var form = document.forms.namedItem('uploadForm');
            var formData = new FormData(form);
            formData.append('uploadedFile', unit.file);
            var metadatos = new metadatosDeArchivo(unit.name, unit.extension, unit.file.type, unit.file.lastModifiedDate, unit.size, vm.idColeccion);
            formData.append('metadatos', JSON.stringify(metadatos));
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
            xhr.open("POST", "./archivos/upload", true);
            xhr.setRequestHeader("Authorization", vm.loginInfo.token);
            try {
                xhr.send(formData);
            }
            catch (e) {
                console.log('error on send');
            }
            function stopUpload() {
                if (!xhr)
                    return;
                if (unit.waitingServer) {
                    console.log('Upload can not be canceled. Is waiting for server response');
                    return;
                }
                setFailure('Carga cancelada');
                xhr.abort();
            }
            function progress(e) {
                try {
                    if (!vm || unit.state !== vm.states.uploading)
                        return;
                    updateProgress(e);
                    vm.$apply(function () { return updateProgress(e); });
                }
                catch (e) {
                    console.log('Error in progress handler', e);
                }
            }
            function updateProgress(e) {
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
                vm.$apply(function () { return unit.waitingServer = true; });
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
                        setFailure(errorNoEspecificado);
                        vm.$apply(function () {
                            setFailure(errorNoEspecificado);
                        });
                        break;
                    case 200:
                        var serialized = e.target.response;
                        if (serialized === "")
                            return;
                        var result = JSON.parse(serialized);
                        if (result.exitoso) {
                            setUploaded();
                            vm.$apply(function () {
                                setUploaded();
                            });
                            console.log('El archivo se recibio correctamente en el servidor');
                        }
                        else {
                            setFailure(elArchivoYaExiste);
                            vm.$apply(function () {
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
            function setFailure(message) {
                unit.progress = 0;
                unit.state = vm.states.uploadFailed;
                unit.waitingServer = false;
                unit.errorMessage = message;
            }
        };
        return filesWidgetController;
    }());
    filesWidgetController.$inject = ['$scope', 'toasterLite', 'localStorageLite', 'config', '$http'];
    var fileUnit = (function () {
        function fileUnit(name, extension, state, file, size, // in bytes
            formattedSize, 
            // Presets
            iconUrl, iconSvg, isAPicture, progress, waitingServer, justUploaded, errorMessage, 
            // Methods
            stopUpload) {
            if (iconUrl === void 0) { iconUrl = ''; }
            if (iconSvg === void 0) { iconSvg = ''; }
            if (isAPicture === void 0) { isAPicture = false; }
            if (progress === void 0) { progress = 0; }
            if (waitingServer === void 0) { waitingServer = false; }
            if (justUploaded === void 0) { justUploaded = false; }
            if (errorMessage === void 0) { errorMessage = ''; }
            if (stopUpload === void 0) { stopUpload = null; }
            this.name = name;
            this.extension = extension;
            this.state = state;
            this.file = file;
            this.size = size;
            this.formattedSize = formattedSize;
            this.iconUrl = iconUrl;
            this.iconSvg = iconSvg;
            this.isAPicture = isAPicture;
            this.progress = progress;
            this.waitingServer = waitingServer;
            this.justUploaded = justUploaded;
            this.errorMessage = errorMessage;
            this.stopUpload = stopUpload;
        }
        return fileUnit;
    }());
    common.fileUnit = fileUnit;
    var metadatosDeArchivo = (function () {
        function metadatosDeArchivo(nombre, extension, tipo, fecha, size, idColeccion) {
            this.nombre = nombre;
            this.extension = extension;
            this.tipo = tipo;
            this.fecha = fecha;
            this.size = size;
            this.idColeccion = idColeccion;
        }
        return metadatosDeArchivo;
    }());
    common.metadatosDeArchivo = metadatosDeArchivo;
})(common || (common = {}));
//# sourceMappingURL=filesWidgetDirective.js.map