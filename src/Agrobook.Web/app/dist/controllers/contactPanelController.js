/// <reference path="../_all.ts" />
var ContactManagerApp;
(function (ContactManagerApp) {
    var ContactPanelConntroller = (function () {
        function ContactPanelConntroller(userService, $mdBottomSheet) {
            this.userService = userService;
            this.$mdBottomSheet = $mdBottomSheet;
            this.user = userService.selectedUser;
        }
        return ContactPanelConntroller;
    }());
    ContactPanelConntroller.$inject = ['userService', '$mdBottomSheet'];
    ContactManagerApp.ContactPanelConntroller = ContactPanelConntroller;
})(ContactManagerApp || (ContactManagerApp = {}));
//# sourceMappingURL=contactPanelController.js.map