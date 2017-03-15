/// <reference path="_all.ts" />

module common {
    export class localStorageLite {

        static $inject = [];

        constructor() { }

        save(key: string, payload: any): void {
            var serialized = JSON.stringify(payload);
            localStorage[key] = serialized;
        }

        get<T>(key: string): T {
            var payload = localStorage[key];
            if (payload === undefined)
                return undefined;

            var object = JSON.parse(payload);
            return object as T;
        }

        delete(key: string) {
            localStorage.removeItem(key);
        }
    }
}