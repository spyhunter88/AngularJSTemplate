'use strict';

app.controller('accountController', function ($scope, accountApi, ngToast, dialogs) {
    $scope.users = [];

    $scope.userTableColumnDef = [
        {
            columnHeaderDisplayName: 'User Name',
            displayProperty: 'userName',
            columnSearchProperty: 'userName',
            visible: true
        },
        {
            columnHeaderDisplayName: 'Lockout Enabled',
            // displayProperty: 'lockoutEnabled',
            template: '<input type="checkbox" disabled ng-model="item.lockoutEnabled"/>',
            visible: true
        },
        {
            columnHeaderDisplayName: 'Email',
            displayProperty: 'email',
            columnSearchProperty: 'email',
            visible: true
        },
        {
            columnHeaderDisplayName: 'System Login',
            // displayProperty: 'systemLoginEnabled',
            template: '<input type="checkbox" disabled ng-model="item.systemLoginEnabled"/>',
            visible: true
        },
        {
            columnHeaderTemplate: '<button class="btn btn-primary btn-sm" ng-click="editAccount()"><i class="glyphicon glyphicon-plus"></i></button>',
            template: '<button class="btn btn-sm" ng-click="editAccount(item)"><i class="glyphicon glyphicon-edit"></i></button>',
            visible: true
        }
    ];

    // Call to retrieve account list from database
    accountApi.getAccounts().then(
        function (data) {
            $scope.users = data;
        }, function (err) {
            ngToast.create('Error');
        });

    var _editAccount = function (account) {
        // open edit dialog
        var modal = dialogs.create('/Areas/Admin/Content/Static/Account/edit.html?bust=' + Math.random().toString(36).slice(2), 'accountCtrl', { account: account });
        modal.result.then(
            function (data) {
                // Save the account
            }, function (err) {
                if (err != 'cancelled' && err != 'escape key press' && err != 'backdrop click')
                    ngToast.create('Error while saving account!');
            }
        );
    };

    $scope.editAccount = _editAccount;
})

.controller('accountCtrl', function ($scope, $modalInstance, data) {
    if (data.account === undefined) $scope.account = { id: 0 };
    else $scope.account = data.account;

    $scope.cancel = function () {
        $modalInstance.dismiss('cancelled');
    }; // end cancel

    $scope.save = function () {
        // TODO: take a check from server/client with remain payment
        $modalInstance.close($scope.account);
    }; // end save

    $scope.hitEnter = function (evt) {
        if (angular.equals(evt.keyCode, 13) && !(angular.equals($scope.name, null) || angular.equals($scope.name, '')))
            $scope.save();
    }; // end hitEnter
});