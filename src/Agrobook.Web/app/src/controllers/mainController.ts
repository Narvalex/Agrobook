/// <reference path="../_all.ts" />

module ContactManagerApp {
    export class MainController {
        static $inject = ['userService', '$mdSidenav', '$mdToast', '$mdDialog', '$mdMedia'];

        constructor(
            private userService: IUserService,
            private $mdSidenav: angular.material.ISidenavService,
            private $mdToast: angular.material.IToastService,
            private $mdDialog: angular.material.IDialogService,
            private $mdMedia: angular.material.IMedia) {
            var self = this;

            this.userService
                .loadAllUsers()
                .then((users: User[]) => {
                    self.users = users;
                    self.selected = users[0];
                    console.log(self.users);
                });
        }

        tabIndex: number = 0;
        searchText: string = '';
        users: User[] = [];
        selected: User = null;
        message: string = "Hello from our controller";

        toggleSideNav(): void {
            this.$mdSidenav('left').toggle();
        }

        selectUser(user: User): void {
            this.selected = user;

            var sideNav = this.$mdSidenav('left');
            if (sideNav.isOpen) {
                sideNav.close();
            }

            this.tabIndex = 0;
        }

        addUser($event) {
            var self = this;
            var useFullScreen = (this.$mdMedia('sm') || this.$mdMedia('xs'));

            this.$mdDialog.show({
                templateUrl: './dist/view/newUserDialog.html',
            }).then((user: User) => {
                self.openToast('User added');
            }, () => {
                console.log('You cancelled the dialog.');
            });;
        }

        clearNotes($event: MouseEvent) {
            var confirm = this.$mdDialog.confirm()
                .title('Are your sure your vant to delete all notes?')
                .textContent('All notes will be deleted, you can\'t undo this action.')
                .targetEvent($event)
                .ok('Yes')
                .cancel('No');

            var self = this;
            this.$mdDialog.show(confirm).then(() => {
                self.selected.notes = [];
                self.openToast('Cleared notes');
            });
        }

        removeNote(note: Note): void {
            var foundIndex = this.selected.notes.indexOf(note);
            this.selected.notes.splice(foundIndex, 1);
            this.openToast("Note was removed");
        }

        openToast(message: string): void {
            this.$mdToast.show(
                this.$mdToast.simple()
                    .textContent(message)
                    .position('top right')
                    .hideDelay(3000)
            );
        }
    }
}