/// <reference path="../../_all.ts" />
/*
 * Original http://jsfiddle.net/vishalvasani/4hqVu/
 */
var archivosArea;
(function (archivosArea) {
    var uploadCenterController = (function () {
        function uploadCenterController($mdDialog, $scope, $timeout, toasterLite) {
            var _this = this;
            this.$mdDialog = $mdDialog;
            this.$scope = $scope;
            this.$timeout = $timeout;
            this.toasterLite = toasterLite;
            this.title = 'Centro de carga de archivos';
            this.originalTitle = this.title;
            this.files = [];
            this.$timeout(function () {
                _this.initDragAndDrop(_this);
            }, 0);
        }
        uploadCenterController.prototype.cerrar = function () {
            this.$mdDialog.cancel();
        };
        uploadCenterController.prototype.cargar = function (file) {
            var controller = this;
            function progress(e) {
                try {
                    controller.$scope.$apply(function () {
                        if (e.lengthComputable) {
                            controller.progress = Math.round(e.loaded * 100 / e.total);
                        }
                        else {
                            controller.progress = 'unable to compute';
                        }
                    });
                }
                catch (e) {
                    console.log('error in progress handler');
                }
            }
            function load(e) {
                controller.toasterLite.success('El archivo fue cargado exitosamente');
            }
            function error(e) {
                controller.toasterLite.error('Error al cargar archivo');
            }
            function abort(e) {
                controller.toasterLite.info('Carga abortada');
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
        //
        // Internal
        //
        uploadCenterController.prototype.initDragAndDrop = function (scope) {
            function restaurarTitulo() {
                scope.title = scope.originalTitle;
                scope.$scope.$apply();
            }
            function dragEnter(e) {
                e.stopPropagation();
                e.preventDefault();
                scope.title = 'Recibiendo archivo...';
                scope.$scope.$apply();
                //scope.toasterLite.info('Suelte los archivos aquí para cargarlos...');
            }
            function dragLeave(e) {
                e.stopPropagation();
                e.preventDefault();
                restaurarTitulo();
            }
            function dragOver(e) {
                // console.log('dragOver was called every 100 ms');
                //e.stopPropagation();
                e.preventDefault();
            }
            function drop(e) {
                e.stopPropagation();
                e.preventDefault();
                var files = e.dataTransfer.files;
                if (files.length > 0) {
                    scope.$scope.$apply(function () {
                        for (var i = 0; i < files.length; i++) {
                            scope.files.push(files[i]);
                        }
                    });
                }
                restaurarTitulo();
            }
            var area = document.getElementById('dragAndDropArea');
            area.addEventListener("dragenter", dragEnter, false);
            area.addEventListener("dragstart", dragEnter, false);
            area.addEventListener("dragleave", dragLeave, false);
            area.addEventListener("dragover", dragOver, false);
            area.addEventListener("dragend", dragLeave, false);
            area.addEventListener("drop", drop, false);
        };
        return uploadCenterController;
    }());
    uploadCenterController.$inject = ['$mdDialog', '$scope', '$timeout', 'toasterLite'];
    archivosArea.uploadCenterController = uploadCenterController;
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=uploadCenterController.js.map