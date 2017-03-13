/// <reference path="../_all.ts" />

module HomeArea {
    export class ToolbarHeaderController {

        static $inject = ['loginWriteService', 'config'];

        constructor(
            private loginWriteService: login.loginService,
            private config: common.config) {
        }

        usuario: string;

        password: string;

        onInputKeyPress($event): void {
            if ($event.keyCode == this.config.keyCodes.enter)
                this.login();
        }

        login(): void {
            if (this.usuario == undefined || this.usuario == '') {
                window.alert('Por favor ingrese su usuario');
                return;
            }

            if (this.password == undefined || this.password == '') {
                window.alert('Por favor ingrese su contraseña');
                return;
            }

            this.loginWriteService.tryLogin(
                new login.credencialesDto(this.usuario, this.password),
                value => {
                    console.log('Logueado!');
                    location.href = "areas/usuarios.html";
                },
                reason => console.log(reason));
        }
    }
}
