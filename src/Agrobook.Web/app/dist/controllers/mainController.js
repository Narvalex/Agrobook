/// <reference path="../_all.ts" />
var ContactManagerApp;
(function (ContactManagerApp) {
    var MainController = (function () {
        function MainController(userService, $mdSidenav) {
            this.userService = userService;
            this.$mdSidenav = $mdSidenav;
            this.searchText = '';
            this.users = [];
            this.selected = null;
            this.message = "Hello from our controller";
            var self = this;
            this.userService
                .loadAllUsers()
                .then(function (users) {
                self.users = users;
                self.selected = users[0];
                console.log(self.users);
            });
        }
        MainController.prototype.toggleSideNav = function () {
            this.$mdSidenav('left').toggle();
        };
        MainController.prototype.selectUser = function (user) {
            this.selected = user;
            var sideNav = this.$mdSidenav('left');
            if (sideNav.isOpen) {
                sideNav.close();
            }
        };
        return MainController;
    }());
    MainController.$inject = ['userService', '$mdSidenav'];
    ContactManagerApp.MainController = MainController;
})(ContactManagerApp || (ContactManagerApp = {}));
//# sourceMappingURL=mainController.js.map