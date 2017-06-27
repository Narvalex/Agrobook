/// <reference path="../../_all.ts" />
/*
 * Original http://jsfiddle.net/vishalvasani/4hqVu/
 */
var archivosArea;
(function (archivosArea) {
    var uploadCenterController = (function () {
        function uploadCenterController($mdDialog, $scope, $timeout, toasterLite, $routeParams, uploader) {
            var _this = this;
            this.$mdDialog = $mdDialog;
            this.$scope = $scope;
            this.$timeout = $timeout;
            this.toasterLite = toasterLite;
            this.$routeParams = $routeParams;
            this.uploader = uploader;
            this.$timeout(function () {
                _this.initDragAndDrop(_this);
            }, 0);
            this.idProductor = this.$routeParams['idProductor'];
            this.title = 'Centro de carga - ' + this.idProductor;
            this.uploader.setScope(this.$scope);
            this.$scope.prepararArchivosSeleccionados = this.prepararArchivosSeleccionados;
        }
        uploadCenterController.prototype.seleccionarArchivos = function () {
            this.$timeout(function () {
                angular.element('#fileInputBtn').trigger('click');
            }, 0);
        };
        uploadCenterController.prototype.prepararArchivosSeleccionados = function (element) {
            var vm = angular.element(this)[0].vm;
            // reset suff 
            var container = document.getElementById('fileInputContainer');
            var content = container.innerHTML;
            container.innerHTML = content;
            //
            vm.$scope.$apply(function (scope) {
                console.log("Se seleccionaron " + element.files.length + " archivos");
                vm.uploader.prepareFiles(element.files);
            });
        };
        uploadCenterController.prototype.cerrar = function () {
            this.$mdDialog.cancel();
            window.location.replace('#!/archivos/' + this.idProductor);
        };
        uploadCenterController.prototype.quitarArchivo = function (unit) {
            this.uploader.removeFile(unit);
        };
        uploadCenterController.prototype.limpiar = function () {
            this.uploader.clear();
        };
        //
        // Internal
        //
        uploadCenterController.prototype.initDragAndDrop = function (scope) {
            function dragEnter(e) {
                e.stopPropagation();
                e.preventDefault();
                scope.$scope.$apply();
                //scope.toasterLite.info('Suelte los archivos aquí para cargarlos...');
            }
            function dragLeave(e) {
                e.stopPropagation();
                e.preventDefault();
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
                        scope.uploader.prepareFiles(files);
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
        };
        return uploadCenterController;
    }());
    uploadCenterController.$inject = ['$mdDialog', '$scope', '$timeout', 'toasterLite', '$routeParams', 'uploadService'];
    archivosArea.uploadCenterController = uploadCenterController;
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=uploadCenterController.js.map