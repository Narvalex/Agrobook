/// <reference path="../_all.ts" />
var archivosArea;
(function (archivosArea) {
    var sidenavController = (function () {
        function sidenavController($mdSidenav, toasterLite, $rootScope, config) {
            var _this = this;
            this.$mdSidenav = $mdSidenav;
            this.toasterLite = toasterLite;
            this.$rootScope = $rootScope;
            this.config = config;
            this.$rootScope.$on(this.config.eventIndex.archivos.productorSeleccionado, function (e, args) {
                _this.idProductor = args;
            });
            this.unsafeInitFileUpload();
        }
        sidenavController.prototype.toggleSideNav = function () {
            this.$mdSidenav('left').toggle();
        };
        sidenavController.prototype.nuevoArchivo = function () {
            this.toasterLite.info("nuevo archivo para " + this.idProductor + "!");
        };
        /**
         * File Upload stuff
         */
        sidenavController.prototype.unsafeInitFileUpload = function () {
            // Credits: https://www.sitepoint.com/html5-file-drag-and-drop/
            if (document.readyState !== "complete") {
                setTimeout(arguments.callee, 100);
                return;
            }
            var w = window;
            var canInit = w.File && w.FileList && w.FileReader;
            if (!canInit) {
                this.toasterLite.error('File Upload nor available!', this.toasterLite.delayForever);
                return;
            }
            // initialize
            var fileSelect = getById("fileselect");
            var filedrag = getById("filedrag");
            var submitbutton = getById("submitbutton");
            // file select 
            fileSelect.addEventListener("change", fileSelectHandler, false);
            // is xhr2 available?
            var xhr = new XMLHttpRequest();
            if (!xhr.upload) {
                this.toasterLite.error('XHR not available!', this.toasterLite.delayForever);
                return;
            }
            // file drop
            filedrag.addEventListener("dragover", fileDragHover, false);
            filedrag.addEventListener("dragleave", fileDragHover, false);
            filedrag.addEventListener("drop", fileSelectHandler, false);
            filedrag.style.display = "block";
            // remove submit button
            submitbutton.style.display = "none";
            function getById(id) {
                return document.getElementById(id);
            }
            function output(message) {
                var m = getById("messages");
                m.innerHTML = message + m.innerHTML;
            }
            // file selection
            function fileSelectHandler(e) {
                // cancel event and hover styling
                fileDragHover(e);
                var files = e.target.files || e.dataTransfer.files;
                // process all File objects
                for (var i = 0; i < files.length; i++) {
                    var f = files[i];
                    parseFile(f);
                }
            }
            function fileDragHover(e) {
                e.stopPropagation();
                e.preventDefault();
                e.target.className = (e.type == "dragover" ? "hover" : "");
            }
            function parseFile(file) {
                output('file processed');
            }
        };
        return sidenavController;
    }());
    sidenavController.$inject = ['$mdSidenav', 'toasterLite', '$rootScope', 'config'];
    archivosArea.sidenavController = sidenavController;
})(archivosArea || (archivosArea = {}));
//# sourceMappingURL=sidenavController.js.map