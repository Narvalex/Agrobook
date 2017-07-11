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
            public loginExitoso: boolean,
            public usuario: string,
            public nombreParaMostrar: string,
            public token: string,
            public avatarUrl: string,
            public claims: string[]
        )
        { }
    }
}