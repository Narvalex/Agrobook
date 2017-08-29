/// <reference path="../../_all.ts" />

/*
 * Original http://jsfiddle.net/vishalvasani/4hqVu/
 */

module archivosArea {
    export class uploadCenterController {
        static $inject = ['$mdDialog', '$scope', '$timeout', 'toasterLite', '$routeParams', 'uploadService'];

        constructor(
            private $mdDialog: ng.material.IDialogService,
            public $scope: ng.IScope,
            private $timeout: ng.ITimeoutService,
            public toasterLite: common.toasterLite,
            private $routeParams: ng.route.IRouteParamsService,
            public uploader: uploadService
        ) {
            this.$timeout(() => {
                this.initDragAndDrop(this);
            }, 0);

            this.idProductor = this.$routeParams['idProductor'];
            this.title = 'Centro de carga - ' + this.idProductor;

            this.uploader.setScope(this.$scope);

            this.$scope.prepararArchivosSeleccionados = this.prepararArchivosSeleccionados;
        }

        idProductor: string;

        title: string

        seleccionarArchivos() {
            this.$timeout(() => {
                angular.element('#fileInputBtn').trigger('click');
            }, 0);
        }

        prepararArchivosSeleccionados(element: HTMLInputElement) {
            var vm = (angular.element(this)[0] as any).vm as uploadCenterController;
            // reset suff 
            var container = document.getElementById('fileInputContainer');
            var content = container.innerHTML;
            container.innerHTML = content;
            //
            vm.$scope.$apply(scope => {
                console.log(`Se seleccionaron ${element.files.length} archivos`);
                vm.uploader.prepareFiles(element.files, vm.idProductor);
            });
        }

        cerrar() {
            this.$mdDialog.cancel();
            window.location.replace('#!/archivos/' + this.idProductor);
        }

        quitarArchivo(unit: uploadUnit) {
            this.uploader.removeFile(unit);
        }

        limpiar() {
            this.uploader.clear();
        }

        //
        // Internal
        //
        private initDragAndDrop(scope: uploadCenterController) {

            function dragEnter(e: Event) {
                e.stopPropagation();
                e.preventDefault();

                scope.$scope.$apply();
                //scope.toasterLite.info('Suelte los archivos aquí para cargarlos...');
            }

            function dragLeave(e: Event) {
                e.stopPropagation();
                e.preventDefault();


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
                        scope.uploader.prepareFiles(files, scope.idProductor);
                    });
                }
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