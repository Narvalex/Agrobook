/// <reference path="../_all.ts" />

module ContactManagerApp {

    export class ContactPanelConntroller {
        static $inject = ['userService', '$mdBottomSheet'];

        constructor(
            private userService: IUserService,
            private $mdBottomSheet: angular.material.IBottomSheetService) {

            this.user = userService.selectedUser;
        }

        user: User;
    }

}