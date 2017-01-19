/// <reference path="_all.ts" />
var ContactManagerApp;
(function (ContactManagerApp) {
    angular.module("contactManagerApp", ['ngMaterial'])
        .service('userService', ContactManagerApp.UserService)
        .controller("mainController", ContactManagerApp.MainController);
})(ContactManagerApp || (ContactManagerApp = {}));
//# sourceMappingURL=boot.js.map