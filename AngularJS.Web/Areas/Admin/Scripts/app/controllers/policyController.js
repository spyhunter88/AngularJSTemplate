'use strict';

app.controller('policyController', function ($scope, accountApi, ngToast, dialogs) {

    var _init = function () {
        $scope.type = 'user';
        $scope.types = [{ data: 'user', vis: 'By User' }, { data: 'role', vis: 'By Role' }];

        $scope.users = [];
        $scope.roles = [];

        // Get User and Roles
        accountApi.getAccounts().then(
            function (data) {
                $scope.users = data;
                console.log(data);
            }, function (err) {
                ngToast.create('Error while loading user list!');
            });
        accountApi.getRoles().then(
            function (data) {
                $scope.roles = data;
            }, function (err) {
                ngToast.create('Error while loading role list!');
            });

        // Column definition
        $scope.objectActionColumnDef = [
            { columnHeaderDisplayName: 'Object', displayProperty: 'object', columnSearchProperty: 'object', visible: true },
            { columnHeaderDisplayName: 'Status', displayProperty: 'status', columnSearchProperty: 'status', visible: true },
            { columnHeaderDisplayName: 'Action', displayProperty: 'action', columnSearchProperty: 'action', visible: true },
            {
                columnHeaderTemplate: '<input type="checkbox" ng-click="setPublicEnabled(\'objectAction\')"/> Public Enabled',
                template: '<input type="checkbox" ng-model="item.publicEnabled"/>',
                visible: true
            }
        ];

        $scope.objectConfigColumnDef = [
            { columnHeaderDisplayName: 'Object', displayProperty: 'object', columnSearchProperty: 'object', visible: true },
            { columnHeaderDisplayName: 'Status', displayProperty: 'status', columnSearchProperty: 'status', visible: true },
            { columnHeaderDisplayName: 'Object Field', displayProperty: 'objectField', columnSearchProperty: 'objectField', visible: true },
            { columnHeaderDisplayName: 'Field Property', displayProperty: 'fieldProperty', columnSearchProperty: 'fieldProperty', visible: true },
            {
                columnHeaderTemplate: '<input type="checkbox" ng-click="setPublicEnabled(\'objectConfig\')"/> Public Enabled',
                template: '<input type="checkbox" ng-model="item.publicEnabled"/>',
                visible: true
            }
        ];
    };

    var _getPolicy = function (userId, roleId) {

    };

    var _savePolicy = function () {

    };

    var _setPublicEnabled = function (type) {

    };

    $scope.getPolicy = _getPolicy;
    $scope.savePolicy = _savePolicy;

    $scope.setPublicEnabled = _setPublicEnabled;

    // Initialize
    _init();
});