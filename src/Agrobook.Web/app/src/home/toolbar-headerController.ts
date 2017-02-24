/// <reference path="../_all.ts" />

module Home {
    export class ToolbarHeaderController {
        static $inject = [];

        constructor() { }

        login(): void {
            location.href = "areas/maps.html";
        }
    }
}