/// <reference path="../_all.ts" />

module homeArea {
    export class ToolbarHeaderController {

        static $inject = ['loginService', 'loginQueryService', 'config', '$rootScope'];

        constructor(
            private loginService: login.loginService,
            private loginQueryService: login.loginQueryService,
            private config: common.config,
            private $rootScope: angular.IRootScopeService
        ) {
            this.$rootScope.$on(this.config.eventIndex.login.loggedOut, (e, a) => {
                this.verificarSiEstaLogueado();
            });

            this.verificarSiEstaLogueado();
        }

        estaLogueado: boolean = false;

        nombreParaMostrar: string;

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

            this.loginService.tryLogin(
                new login.credencialesDto(this.usuario, this.password),
                value => {
                    if (value.data.loginExitoso) {
                        this.establecerUsuarioLogueado(value.data.nombreParaMostrar);
                    }
                    else {
                        window.alert("Credenciales inválidas");
                        this.password = "";
                    }
                },
                reason => console.log(reason));
        }

        private verificarSiEstaLogueado(): void {
            var result = this.loginQueryService.tryGetLocalLoginInfo();
            if (result === undefined || !result.loginExitoso) {
                this.estaLogueado = false;
            }
            else {
                this.establecerUsuarioLogueado(result.nombreParaMostrar);
            }
        }

        private establecerUsuarioLogueado(nombreParaMostrar: string) {
            this.nombreParaMostrar = nombreParaMostrar;
            this.estaLogueado = true;
        }
    }
}
