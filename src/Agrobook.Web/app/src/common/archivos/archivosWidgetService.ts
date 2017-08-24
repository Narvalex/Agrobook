/// <reference path="../../_all.ts" />

// this will be a sample
module common {
    export class archivosWidgetService {
        static $inject = [];

        constructor(
        ) {
        }

        selectFiles() {
            setTimeout(() => document.getElementById('awFileInputBtn').click(), 0);
        }
    }
}