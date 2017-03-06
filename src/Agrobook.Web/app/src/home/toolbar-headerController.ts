/// <reference path="../_all.ts" />

module Home {
    export class ToolbarHeaderController {
        static $inject = ['loginWriteService'];

        constructor(
            private loginWriteService: login.loginService)
        { }

        usuario: string;

        password: string;

        login(): void {
            //location.href = "areas/usuarios.html";
            this.loginWriteService.tryLogin(new login.credencialesDto(this.usuario, this.password))
                .then(response => {
                    console.log(response.data);
                }, reason => {
                    console.log(reason);
                });
        }
    }
}
