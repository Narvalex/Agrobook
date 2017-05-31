/// <reference path="../_all.ts" />

module archivosArea {
    export interface IWindowFileReady extends Window
    {
        File: File,
        FileList: FileList,
        FileReader: FileReader
    }

    export class sidenavController {
        static $inject = ['$mdSidenav', 'toasterLite', '$rootScope', 'config'];

        constructor(
            private $mdSidenav: angular.material.ISidenavService,
            private toasterLite: common.toasterLite,
            private $rootScope: ng.IRootScopeService,
            private config: common.config
        ) {
            this.$rootScope.$on(this.config.eventIndex.archivos.productorSeleccionado, (e, args) => {
                this.idProductor = args;
            });

            this.unsafeInitFileUpload();
        }
        
        idProductor: string;

        toggleSideNav(): void {
            this.$mdSidenav('left').toggle();
        }

        nuevoArchivo() {
            this.toasterLite.info(`nuevo archivo para ${this.idProductor}!`);
        }

        /**
         * File Upload stuff
         */
        unsafeInitFileUpload() {
            // Credits: https://www.sitepoint.com/html5-file-drag-and-drop/
            if (document.readyState !== "complete") {
                setTimeout(arguments.callee, 100);
                return;
            }

            let w = window as IWindowFileReady;
            var canInit = w.File && w.FileList && w.FileReader;
            if (!canInit) {
                this.toasterLite.error('File Upload nor available!', this.toasterLite.delayForever);
                return;
            }

            // initialize
            let fileSelect = getById("fileselect");
            let filedrag = getById("filedrag");
            let submitbutton = getById("submitbutton");


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


            function getById(id: string) {
                return document.getElementById(id);
            }

            function output(message: string) {
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
                    let f = files[i];
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
        }
    }
}
