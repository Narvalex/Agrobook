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
            _this.files = [];
            return _this;
        }
        uploadService.prototype.prepareFiles = function (files) {
            for (var i = 0; i < files.length; i++) {
                this.files.push(files[i]);
            }
        };
        uploadService.prototype.uploadFile = function (file) {
            var self = this;
            function progress(e) {
                try {
                    if (!self.scopeListener)
                        return;
                    self.scopeListener.$apply(function () {
                        if (e.lengthComputable) {
                            self.progress = Math.round(e.loaded * 100 / e.total);
                        }
                        else {
                            self.progress = 'unable to compute';
                        }
                    });
                }
                catch (e) {
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
            }
            catch (e) {
                console.log('error on send');
            }
        };
        return uploadService;
    }(common.httpLite));
    uploadService.$inject = ['$http', 'toasterLite'];
    archivosArea.uploadService = uploadService;
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=uploadService.js.map