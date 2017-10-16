/// <reference path="../_all.ts" />

module homeArea {
    export class mainContentController {
        static $inject = [];

        constructor(
        ) {
            var date = new Date();
            this.currentYear = date.getFullYear();
        }

        currentYear: number;

        goTo(url: string) {
            //  this is the right way, in other to be able to go back 
            window.location.href = url;
        }
    }
}