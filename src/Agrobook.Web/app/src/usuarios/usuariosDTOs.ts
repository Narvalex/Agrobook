/// <reference path="../_all.ts" />

module usuariosArea {
    export class UsuarioDto {
        constructor(
            public avatarUrl: string,
            public nombreDeUsuario: string,
            public nombreParaMostrar: string,
            public password: string,
            public claims: string[]
        ) { }
    }

    export class usuarioEnLista {
        constructor(
            public nombre: string,
            public nombreCompleto: string,
            public avatarUrl: string
        ) {
        }
    }
}