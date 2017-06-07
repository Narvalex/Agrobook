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
                e.stopPropagation();
                e.preventDefault();
            }
            function drop(e) {
                console.log('Drop event: ', JSON.parse(JSON.stringify(e.dataTransfer)));
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