/// <reference path="../../_all.ts" />

/*
 * Original http://jsfiddle.net/vishalvasani/4hqVu/
 */

module archivosArea {
    export class uploadCenterController {
        static $inject = ['$mdDialog', '$scope', '$timeout', 'toasterLite'];

        constructor(
            private $mdDialog: ng.material.IDialogService,
            private $scope: ng.IScope,
            private $timeout: ng.ITimeoutService,
            private toasterLite: common.toasterLite
        ) {
            this.$timeout(() => {
                this.initDragAndDrop(this);
            }, 0);
        }

        title = 'Centro de carga de archivos';
        originalTitle = this.title;
        files = [];

        // loading
        progress: any;

        cerrar() {
            this.$mdDialog.cancel();
        }

        cargar(file: File) {

            var controller = this;

            function progress(e) {
                controller.$scope.$apply(() => {
                    if (e.lengthComputable) {
                        controller.progress = Math.round(e.loaded * 100 / e.total);
                    }
                    else {
                        controller.progress = 'unable to compute';
                    }
                });
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

            var form = document.forms.namedItem('uploadForm');
            var formData = new FormData(form);
            formData.append('uploadedFile', file);

            var xhr = new XMLHttpRequest();
            xhr.upload.addEventListener("progress", progress, false);
            xhr.upload.addEventListener("load", load, false);
            xhr.addEventListener("error", error, false);
            xhr.addEventListener("abort", abort, false);
            xhr.open("POST", "./archivos/upload");
            xhr.send(formData);
        }

        //
        // Internal
        //

        private initDragAndDrop(scope: uploadCenterController) {

            function restaurarTitulo() {
                scope.title = scope.originalTitle;
                scope.$scope.$apply();
            }

            function dragEnter(e: Event) {
                e.stopPropagation();
                e.preventDefault();

                scope.title = 'Recibiendo archivo...';
                scope.$scope.$apply();
                //scope.toasterLite.info('Suelte los archivos aquí para cargarlos...');
            }

            function dragLeave(e: Event) {
                e.stopPropagation();
                e.preventDefault();

                restaurarTitulo();
            }

            function dragOver(e: Event) {
               // console.log('dragOver was called every 100 ms');
                //e.stopPropagation();
                e.preventDefault();
            }

            function drop(e) {
                e.stopPropagation();
                e.preventDefault();

                var files = e.dataTransfer.files;
                if (files.length > 0) {
                    scope.$scope.$apply(() => {
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
        }
    }
}