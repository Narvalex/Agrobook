/// <reference path="../../_all.ts" />
/*
 * Original http://jsfiddle.net/vishalvasani/4hqVu/
 */
var archivosArea;
(function (archivosArea) {
    var uploadCenterController = (function () {
        function uploadCenterController($mdDialog, $scope, $timeout) {
            var _this = this;
            this.$mdDialog = $mdDialog;
            this.$scope = $scope;
            this.$timeout = $timeout;
            this.title = 'Centro de carga de archivos';
            this.originalTitle = this.title;
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
            function dragEnter(e) {
                e.stopPropagation();
                e.preventDefault();
                scope.title = 'Recibiendo archivo...';
                scope.$scope.$apply();
            }
            function dragLeave(e) {
                e.stopPropagation();
                e.preventDefault();
                scope.title = scope.originalTitle;
                scope.$scope.$apply();
            }
            var area = document.getElementById('dragAndDropArea');
            area.addEventListener("dragenter", dragEnter, false);
            area.addEventListener("dragleave", dragLeave, false);
        };
        return uploadCenterController;
    }());
    uploadCenterController.$inject = ['$mdDialog', '$scope', '$timeout'];
    archivosArea.uploadCenterController = uploadCenterController;
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=uploadCenterController.js.map