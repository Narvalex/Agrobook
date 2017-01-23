/// <reference path="../_all.ts" />
var ContactManagerApp;
(function (ContactManagerApp) {
    var MainController = (function () {
        function MainController(userService, $mdSidenav) {
            this.userService = userService;
            this.$mdSidenav = $mdSidenav;
            this.users = [];
            this.message = "Hello from our controller";
            var self = this;
            this.userService
                .loadAllUsers()
                .then(function (users) {
                self.users = users;
                console.log(self.users);
            });
        }
        MainController.prototype.toggleSideNav = function () {
            this.$mdSidenav('left').toggle();
        };
        return MainController;
    }());
    MainController.$inject = ['userService', '$mdSidenav'];
    ContactManagerApp.MainController = MainController;
})(ContactManagerApp || (ContactManagerApp = {}));
//# sourceMappingURL=mainController.js.map