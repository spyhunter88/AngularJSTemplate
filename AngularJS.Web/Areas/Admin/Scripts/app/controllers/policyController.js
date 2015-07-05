'use strict';

app.controller('policyController', function ($scope, $filter, accountApi, ngToast, dialogs) {

    // Declare some global variable
    var whereFilter = $filter('where'); // Use when add new objectConfig or objectAction

    var _init = function () {
        $scope.type = 'user';
        $scope.types = [{ data: 'user', vis: 'By User' }, { data: 'role', vis: 'By Role' }];

        $scope.users = [];
        $scope.roles = [];
        $scope.selected = {};
        $scope.objectConfigs = [];
        $scope.objectActions = [];

        // Keep the public enabled of 2 list
        $scope.publicEnabled = {};

        // Get User and Roles
        accountApi.getAccounts().then(
            function (data) {
                $scope.users = data;
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
                columnHeaderTemplate: '<input type="checkbox" ng-model="publicEnabled.objectAction" ng-change="setPublicEnabled(\'objectAction\')"/> Public Enabled',
                template: '<input type="checkbox" ng-model="item.publicEnabled" ng-true-value="1"/>',
                visible: true
            },
            {
                columnHeaderTemplate: '<button class="btn btn-sm" ng-click="addObject(\'objectAction\')"><a class="glyphicon glyphicon-plus"></a></button>',
                template: '<button class="btn btn-sm btn-danger" ng-click="removeObject(item, \'objectAction\')"><a class="glyphicon glyphicon-remove"></a></button>',
                visible: true
            }
        ];

        $scope.objectConfigColumnDef = [
            { columnHeaderDisplayName: 'Object', displayProperty: 'object', columnSearchProperty: 'object', visible: true },
            { columnHeaderDisplayName: 'Status', displayProperty: 'status', columnSearchProperty: 'status', visible: true },
            { columnHeaderDisplayName: 'Object Field', displayProperty: 'objectField', columnSearchProperty: 'objectField', visible: true },
            { columnHeaderDisplayName: 'Field Property', displayProperty: 'fieldProperty', columnSearchProperty: 'fieldProperty', visible: true },
            {
                columnHeaderTemplate: '<input type="checkbox" ng-model="publicEnabled.objectConfig" ng-change="setPublicEnabled(\'objectConfig\')"/> Public Enabled',
                template: '<input type="checkbox" ng-model="item.publicEnabled" ng-true-value="1"/>',
                visible: true
            },
            {
                columnHeaderTemplate: '<button class="btn btn-sm" ng-click="addObject(\'objectConfig\')"><a class="glyphicon glyphicon-plus"></a></button>',
                template: '<button class="btn btn-sm btn-danger" ng-click="removeObject(item, \'objectConfig\')"><a class="glyphicon glyphicon-remove"></a></button>',
                visible: true
            }
        ];
    };

    var _getPolicy = function (userId, roleId) {
        accountApi.getPolicy(userId, roleId).then(
            function (data) {
                $scope.objectConfigs = data.objectConfigs;
                $scope.objectActions = data.objectActions;

                // Populate public enabled for all
                $scope.publicEnabled = { objectAction: true, objectConfig: true };
                for (var i = 0; i < $scope.objectConfigs.length; i++) {
                    if ($scope.objectConfigs[i].publicEnabled != 1) { $scope.publicEnabled.objectConfig = false; break; }
                }
                for (var i = 0; i < $scope.objectActions.length; i++) {
                    if ($scope.objectActions[i].publicEnabled != 1) { $scope.publicEnabled.objectAction = false; break; }
                }

            }, function (err) {
                console.log(err);
            }
        );
    };

    var _savePolicy = function () {
        var policyModel = {
            userId: $scope.type == 'user' ? $scope.selected.user.id : 0,
            roleId: $scope.type == 'role' ? $scope.selected.role.id : 0,
            objectActions: $scope.objectActions,
            objectConfigs: $scope.objectConfigs
        };
        console.log(policyModel);
        accountApi.savePolicy(policyModel).then(
            function (data) {
                console.log(data);
            }, function (err) {
                ngToast.create('Error while saving Policy!');
            });
    };

    var _addObject = function (type) {
        var modal = dialogs.create('/Areas/Admin/Content/Static/Policy/objectEdit.html?bust=' + Math.random().toString(36).slice(2), 'policyModalCtrl', { type: type });
        modal.result.then(
            function (data) {
                if (type == 'objectConfig') {
                    // Use where to filter if the new objectConfig exists in list
                    var condition = { object: data.object, status: data.status, objectField: data.objectField, fieldProperty: data.fieldProperty};
                    var exists = whereFilter($scope.objectConfigs, condition).length == 1;
                    if (exists) {
                        ngToast.create('The new ObjectConfig already exists!');
                    } else {
                        $scope.objectConfigs.unshift(data);
                    }
                } else if (type == 'objectAction') {
                    // Use where to filter if the new objectConfig exists in list
                    var condition = { object: data.object, status: data.status, action: data.action };
                    var exists = whereFilter($scope.objectConfigs, condition).length == 1;
                    if (exists) {
                        ngToast.create('The new ObjectAction already exists!');
                    } else {
                        $scope.objectActions.unshift(data);
                    }
                }
            }, function (err) {
                if (err != 'cancelled' && err != 'escape key press' && err != 'backdrop click')
                    ngToast.create('Error while add new Object!' + err);
            });
    };
    var _removeObject = function (item, type) {
        if (type == 'objectAction') {
            var idx = $scope.objectActions.indexOf(item);
            $scope.objectActions.splice(idx, 1);
        } else if (type == 'objectConfig') {
            var idx = $scope.objectConfigs.indexOf(item);
            $scope.objectConfigs.splice(idx, 1);
        }
    };

    var _changeType = function () {
        $scope.selected = {};
        $scope.objectActions = [];
        $scope.objectConfigs = [];
    };

    var _setPublicEnabled = function (type) {
        if (type == 'objectAction') {
            for (var i = 0; i < $scope.objectActions.length; i++) {
                $scope.objectActions[i].publicEnabled = $scope.publicEnabled.objectAction ? 1 : 0;
            }
        } else if (type == 'objectConfig') {
            for (var i = 0; i < $scope.objectConfigs.length; i++) {
                $scope.objectConfigs[i].publicEnabled = $scope.publicEnabled.objectConfig ? 1 : 0;
            }
        }
    };

    $scope.getPolicy = _getPolicy;
    $scope.savePolicy = _savePolicy;

    $scope.addObject = _addObject;
    $scope.removeObject = _removeObject;

    $scope.changeType = _changeType;
    $scope.setPublicEnabled = _setPublicEnabled;

    // Initialize
    _init();
});

app.controller('policyModalCtrl', function ($scope, $modalInstance, data) {
    $scope.type = data.type;
    $scope.object = { id: 0 };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancelled');
    }; // end cancel

    $scope.save = function () {
        $modalInstance.close($scope.object);
    }; // end save

    $scope.hitEnter = function (evt) {
        if (angular.equals(evt.keyCode, 13) && !(angular.equals($scope.name, null) || angular.equals($scope.name, '')))
            $scope.save();
    }; // end hitEnter
});