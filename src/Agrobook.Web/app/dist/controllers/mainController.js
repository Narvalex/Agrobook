/// <reference path="../_all.ts" />
var ContactManagerApp;
(function (ContactManagerApp) {
    var MainController = (function () {
        function MainController(userService, $mdSidenav, $mdToast, $mdDialog, $mdMedia, $mdBottomSheet) {
            this.userService = userService;
            this.$mdSidenav = $mdSidenav;
            this.$mdToast = $mdToast;
            this.$mdDialog = $mdDialog;
            this.$mdMedia = $mdMedia;
            this.$mdBottomSheet = $mdBottomSheet;
            this.tabIndex = 0;
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
                self.userService.selectedUser = self.selected;
                console.log(self.users);
            });
        }
        MainController.prototype.toggleSideNav = function () {
            this.$mdSidenav('left').toggle();
        };
        MainController.prototype.selectUser = function (user) {
            this.selected = user;
            this.userService.selectedUser = user;
            var sideNav = this.$mdSidenav('left');
            if (sideNav.isOpen) {
                sideNav.close();
            }
            this.tabIndex = 0;
        };
        MainController.prototype.showContactOptions = function ($event) {
        };
        MainController.prototype.addUser = function ($event) {
            var self = this;
            var useFullScreen = (this.$mdMedia('sm') || this.$mdMedia('xs'));
            this.$mdDialog.show({
                templateUrl: './dist/view/newUserDialog.html',
            }).then(function (user) {
                self.openToast('User added');
            }, function () {
                console.log('You cancelled the dialog.');
            });
            ;
        };
        MainController.prototype.clearNotes = function ($event) {
            var confirm = this.$mdDialog.confirm()
                .title('Are your sure your vant to delete all notes?')
                .textContent('All notes will be deleted, you can\'t undo this action.')
                .targetEvent($event)
                .ok('Yes')
                .cancel('No');
            var self = this;
            this.$mdDialog.show(confirm).then(function () {
                self.selected.notes = [];
                self.openToast('Cleared notes');
            });
        };
        MainController.prototype.removeNote = function (note) {
            var foundIndex = this.selected.notes.indexOf(note);
            this.selected.notes.splice(foundIndex, 1);
            this.openToast("Note was removed");
        };
        MainController.prototype.openToast = function (message) {
            this.$mdToast.show(this.$mdToast.simple()
                .textContent(message)
                .position('top right')
                .hideDelay(3000));
        };
        return MainController;
    }());
    MainController.$inject = [
        'userService',
        '$mdSidenav',
        '$mdToast',
        '$mdDialog',
        '$mdMedia',
        '$mdBottomSheet'
    ];
    ContactManagerApp.MainController = MainController;
})(ContactManagerApp || (ContactManagerApp = {}));
//# sourceMappingURL=mainController.js.map