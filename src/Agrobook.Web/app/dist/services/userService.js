/// <reference path="../_all.ts"/>
var ContactManagerApp;
(function (ContactManagerApp) {
    var UserService = (function () {
        function UserService($q) {
            this.$q = $q;
            this.selectedUser = null;
            this.users = [
                {
                    name: 'Erick Riley',
                    avatar: 'svg-1',
                    bio: 'Long bio here.....',
                    notes: [
                        { title: 'Pay back dinner', date: new Date('12-12-12') },
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
        UserService.prototype.loadAllUsers = function () {
            return this.$q.when(this.users);
        };
        return UserService;
    }());
    UserService.$inject = ['$q'];
    ContactManagerApp.UserService = UserService;
})(ContactManagerApp || (ContactManagerApp = {}));
//# sourceMappingURL=userService.js.map