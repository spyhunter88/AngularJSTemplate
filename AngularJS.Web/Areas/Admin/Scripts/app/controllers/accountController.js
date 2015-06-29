'use strict';

app.controller('accountController', function ($scope, accountApi, ngToast, dialogs) {
    
    var _init = function() {
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
                columnHeaderTemplate: '<button class="btn btn-primary btn-sm" ng-click="editAccount()"><i class="glyphicon glyphicon-plus"></i></button>'
                        + '<button class="btn btn-sm" ng-click="getAccounts()"><i class="glyphicon glyphicon-refresh"></i></button>',
                template: '<button class="btn btn-sm" ng-click="editAccount(item)"><i class="glyphicon glyphicon-edit"></i></button>',
                visible: true
            }
        ];
    };
    
    var _getAccounts = function() {
        // Call to retrieve account list from database
        accountApi.getAccounts().then(
            function (data) {
                $scope.users = data;
            }, function (err) {
                ngToast.create('Error while loading user list!');
            });
    };
    
    var _editAccount = function (account) {
        // open edit dialog
        var modal = dialogs.create('/Areas/Admin/Content/Static/Account/edit.html?bust=' + Math.random().toString(36).slice(2), 'accountCtrl', { account: account });
        modal.result.then(
            function (data) {
                // Save the account
                if (data.id == 0) {
                    accountApi.createAccount(data).then(
                        function(data) {
                            console.log(data);
                            ngToast.create('Success!');
                        }, function(err) {
                            ngToast.create('Error: ' + err);
                        });
                } else {
                    accountApi.saveAccount(data, 'save').then(
                        function(data) {
                            console.log(data);
                        }, function (err) {
                            ngToast.create('Error: ' + err);
                        }
                    );
                }
            }, function (err) {
                if (err != 'cancelled' && err != 'escape key press' && err != 'backdrop click')
                    ngToast.create('Error while saving account!');
            }
        );
    };

    $scope.editAccount = _editAccount;
    $scope.getAccounts = _getAccounts;

    // Initialize
    _init();
    _getAccounts();
})

.controller('accountCtrl', function ($scope, $modalInstance, data) {
    if (data.account === undefined) $scope.account = { id: 0 };
    else $scope.account = data.account;

    $scope.cancel = function () {
        $modalInstance.dismiss('cancelled');
    }; // end cancel

    $scope.save = function () {
        if ($scope.account.newPassword != $scope.account.confirmPassword) ngToast.create('Password does not match!');
        else $modalInstance.close($scope.account);
    }; // end save

    $scope.hitEnter = function (evt) {
        if (angular.equals(evt.keyCode, 13) && !(angular.equals($scope.name, null) || angular.equals($scope.name, '')))
            $scope.save();
    }; // end hitEnter
});