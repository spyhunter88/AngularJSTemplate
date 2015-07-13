'use strict';
app.controller('menuController', function ($scope, accountApi, menuApi, ngToast) {

    function _init() {
        $scope.type = 'user';
        $scope.types = [{ data: 'user', vis: 'By User' }, { data: 'role', vis: 'By Role' }];

        $scope.users = [];
        $scope.roles = [];
        $scope.selected = {};

        $scope.allMenu = [];
        $scope.selectedMenu = [];
        menuApi.getAllMenus().then(
            function (data) {
                // console.log(data);
                $scope.allMenu = data.nestedArray('children', 'id', 'parentID');
                console.log($scope.allMenu);
            }, function (err) {

            });

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
    };

    var _loadMenu = function (userId, roleId) {
        menuApi.getMenus(userId, roleId).then(
            function (data) {
                $scope.selectedMenu = data;
            }, function (err) {
            });
    };

    var _saveMenu = function () {
        var userId = ($scope.type == 'user') ? $scope.selected.user.id : 0;
        var roleId = ($scope.type == 'role') ? $scope.selected.role.id : 0;
        menuApi.saveMenus(userId, roleId, $scope.selectedMenu).then(
            function (data) { },
            function (err) {
                ngToast.create('Error while saving Menu: ' + err);
            }
        );
    };

    var _changeType = function () {
        $scope.selected = {};
        $scope.selectedMenu = [];
    };

    $scope.loadMenu = _loadMenu;
    $scope.saveMenu = _saveMenu;

    $scope.changeType = _changeType;

    _init();
});