/// <reference path="../_all.ts" />

module ContactManagerApp {
    export class MainController {
        static $inject = ['userService'];

        constructor(private userService: IUserService) {
            var self = this;

            this.userService
                .loadAllUsers()
                .then((users: User[]) => {
                    self.users = users;
                    console.log(self.users);
                });
        }

        users: User[] = [];
        message: string = "Hello from our controller";
    }
}