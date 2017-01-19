/// <reference path="../_all.ts"/>

module ContactManagerApp {

    export interface IUserService {
        loadAllUsers(): ng.IPromise<User[]>;
        selectedUser: User;
    }

    export class UserService implements IUserService {

        static $inject = ['$q']

        constructor(private $q: ng.IQService) { }

        selectedUser: User = null;

        loadAllUsers(): ng.IPromise<User[]> {
            return this.$q.when(this.users);
        }

        private users: User[] = [
            {
                name: 'Erick Riley',
                avatar: 'svg-1',
                bio: 'Long bio here.....',
                notes: [
                    { title: 'Pay back dinner', date: new Date('12-12-12')},
                    { title: 'Pay back dinner 2', date: new Date('13-13-13') },
                ]
            },
            {
                name: '2 Erick Riley',
                avatar: 'svg-1',
                bio: 'Long bio here.....',
                notes: [
                    { title: 'Pay back dinner', date: new Date('12-12-12') },
                    { title: 'Pay back dinner 2', date: new Date('13-13-13') },
                ]
            }
        ];
    }
}