/// <reference path="../_all.ts" />

module HomeArea {
    export class ToolbarHeaderController {
        static $inject = ['loginWriteService', 'config'];

        constructor(
            private loginWriteService: login.loginService,
            private config: Common.config)
        { }

        usuario: string;

        password: string;

        onInputKeyPress($event): void {
            if ($event.keyCode == this.config.keyCodes.enter)
                this.login();
        }

        login(): void {
            //location.href = "areas/usuarios.html";)
            if (this.usuario == undefined || this.usuario == '') {
                window.alert('Por favor ingrese su usuario');
                return;
            }

            if (this.password == undefined || this.password == '') {
                window.alert('Por favor ingrese su contraseña');
                return;
            }

            this.loginWriteService.tryLogin(new login.credencialesDto(this.usuario, this.password))
                .then(response => {
                    console.log(response.data);
                }, reason => {
                    console.log(reason);
                });
        }
    }
}
