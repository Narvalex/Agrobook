/// <reference path="../../_all.ts" />

/*
 * Original http://jsfiddle.net/vishalvasani/4hqVu/
 */

module archivosArea {
    export class uploadCenterController {
        static $inject = ['$mdDialog', '$scope', '$timeout'];

        constructor(
            private $mdDialog: ng.material.IDialogService,
            private $scope: ng.IScope,
            private $timeout: ng.ITimeoutService
        ) {
            this.$timeout(() => {
                this.initDragAndDrop(this);
            }, 0);
        }

        title = 'Centro de carga de archivos';
        originalTitle = this.title;

        cerrar() {
            this.$mdDialog.cancel();
        }

        //
        // Internal
        //

        private initDragAndDrop(scope: uploadCenterController) {
            function dragEnter(e: Event) {
                e.stopPropagation();
                e.preventDefault();

                scope.title = 'Recibiendo archivo...';
                scope.$scope.$apply();
            }

            function dragLeave(e: Event) {
                e.stopPropagation();
                e.preventDefault();

                scope.title = scope.originalTitle;
                scope.$scope.$apply();
            }

            var area = document.getElementById('dragAndDropArea');
            area.addEventListener("dragenter", dragEnter, false);
            area.addEventListener("dragleave", dragLeave, false);
        }
    }
}