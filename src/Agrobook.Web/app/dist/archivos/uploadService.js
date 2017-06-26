/// <reference path="../_all.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var archivosArea;
(function (archivosArea) {
    var uploadService = (function (_super) {
        __extends(uploadService, _super);
        function uploadService($http, toasterLite) {
            var _this = _super.call(this, $http, 'archivos/upload') || this;
            _this.$http = $http;
            _this.toasterLite = toasterLite;
            _this.uploadUnits = [];
            return _this;
        }
        uploadService.prototype.setScope = function (scope) {
            this.scope = scope;
            for (var i = 0; i < this.uploadUnits.length; i++) {
                this.uploadUnits[i].setNewScope(scope);
            }
        };
        uploadService.prototype.prepareFiles = function (files) {
            for (var i = 0; i < files.length; i++) {
                var file = files[i];
                var yaExiste = false;
                for (var j = 0; j < this.uploadUnits.length; j++) {
                    if (this.uploadUnits[j].file.name === file.name && this.uploadUnits[j].file.webkitRelativePath === file.webkitRelativePath) {
                        var nombre = file.name ? file.name : file.webkitRelativePath;
                        this.toasterLite.info("El archivo " + nombre + " ya est\u00E1 en la lista.");
                        yaExiste = true;
                        break;
                    }
                }
                if (yaExiste)
                    continue;
                var unit = new uploadUnit(files[i], this.scope, this.toasterLite);
                this.uploadUnits.push(unit);
            }
        };
        uploadService.prototype.removeFile = function (unit) {
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
        };
        uploadService.prototype.clear = function () {
            var unit;
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
        };
        uploadService.prototype.thereArePendingFiles = function () {
            for (var i = 0; i < this.uploadUnits.length; i++) {
                if (!this.uploadUnits[i].uploading && !this.uploadUnits[i].uploaded) {
                    return true;
                }
            }
            return false;
        };
        uploadService.prototype.thereAreFilesToBeCleansed = function () {
            for (var i = 0; i < this.uploadUnits.length; i++) {
                if (!this.uploadUnits[i].uploading || this.uploadUnits[i].uploaded) {
                    return true;
                }
            }
            return false;
        };
        uploadService.prototype.thereAreUploadsInProcess = function () {
            for (var i = 0; i < this.uploadUnits.length; i++) {
                if (this.uploadUnits[i].uploading === true) {
                    return true;
                }
            }
            return false;
        };
        uploadService.prototype.uploadAll = function () {
            for (var i = 0; i < this.uploadUnits.length; i++) {
                if (!this.uploadUnits[i].uploading && !this.uploadUnits[i].uploaded) {
                    this.uploadUnits[i].startUpload();
                }
            }
        };
        uploadService.prototype.stopAll = function () {
            for (var i = 0; i < this.uploadUnits.length; i++) {
                if (this.uploadUnits[i].uploading && !this.uploadUnits[i].uploaded) {
                    this.uploadUnits[i].stopUpload();
                }
            }
        };
        return uploadService;
    }(common.httpLite));
    uploadService.$inject = ['$http', 'toasterLite'];
    archivosArea.uploadService = uploadService;
    var uploadUnit = (function () {
        function uploadUnit(file, scope, toasterLite) {
            this.file = file;
            this.scope = scope;
            this.toasterLite = toasterLite;
            this.uploaded = false;
            this.failed = false;
            this.uploading = false;
            this.progress = 0;
        }
        uploadUnit.prototype.setNewScope = function (scope) {
            this.scope = scope;
        };
        uploadUnit.prototype.stopUpload = function () {
            if (!this.xhr)
                return;
            this.uploading = false;
            this.xhr.abort();
            this.progress = 0;
        };
        uploadUnit.prototype.startUpload = function () {
            var self = this;
            self.uploading = true;
            function progress(e) {
                try {
                    if (!self.scope || !self.uploading)
                        return;
                    updateProgress(e);
                    self.scope.$apply(function () { return updateProgress(e); });
                }
                catch (e) {
                    console.log('error in progress handler');
                }
            }
            function updateProgress(e) {
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
            function setUploaded() {
                self.progress = 100;
                self.uploaded = true;
                self.failed = false;
                self.uploading = false;
            }
            function setFailure() {
                self.failed = true;
                self.uploaded = false;
                self.uploading = false;
                self.progress = 0;
            }
            function readyStateChange(e) {
                console.log("ready state change. status:" + e.target.status + " " + e.target.statusText);
                switch (e.target.status) {
                    case 500:
                        setFailure();
                        self.scope.$apply(function () {
                            setFailure();
                        });
                        break;
                    case 200:
                        setUploaded();
                        self.scope.$apply(function () {
                            setUploaded();
                        });
                        console.log('El archivo se recibio correctamente en el servidor');
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
            }
            catch (e) {
                console.log('error on send');
            }
        };
        return uploadUnit;
    }());
    archivosArea.uploadUnit = uploadUnit;
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=uploadService.js.map