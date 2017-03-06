/// <reference path="../_all.ts" />

module login {

    export class credencialesDto {
        constructor(
            public usuario: string,
            public password: string)
        { }
    }

    export class loginResult {
        constructor(
        )
        { }
    }
}