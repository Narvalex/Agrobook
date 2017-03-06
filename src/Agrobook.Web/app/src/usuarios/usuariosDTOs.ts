/// <reference path="../_all.ts" />

module UsuariosArea {
    export class UsuarioDto {
        constructor(
            public avatarUrl: string,
            public nombreDeUsuario: string,
            public password: string
        ) { }
    }
}