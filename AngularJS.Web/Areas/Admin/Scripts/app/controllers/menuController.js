'use strict';
app.controller('menuController', 
        function ($scope, $rootScope, accountApi, menuApi, ngToast) {
    menuApi.odataService().filter({});

    /* Menu Item List function */
    var onClick = function (event, delegate) {
        var grid = event.grid;
        var selectedRow = grid.select();
        var dataItem = grid.dataItem(selectedRow);

        if (selectedRow.length > 0) {
            delegate(grid, selectedRow, dataItem);
        } else {
            alert("Please select a row.");
        }
    };

    $scope.title = 'Menu Item List';
    $scope.toolbarTemplate = kendo.template($('#toolbar').html());

    $scope.save = function (e) {
        onClick(e, function (grid) {
            grid.saveRow();
            $('.toolbar').toggle();
        });
    };

    $scope.cancel = function (e) {
        onClick(e, function (grid) {
            grid.cancelRow();
            $('.toolbar').toggle();
        });
    };

    $scope.edit = function (e) {
        onClick(e, function (grid, row) {
            grid.editRow(row);
            $('.toolbar').toggle();
        });
    };

    $scope.destroy = function (e) {
        onClick(e, function (grid, row, dataItem) {
            grid.dataSource.remove(dataItem);
            grid.dataSource.sync();
        });
    };

    $scope.onChange = function (e) {
        var grid = e.sender;
        $rootScope.lastSelectedDataItem = grid.dataItem(grid.select());
    };

    $scope.dataSource = menuApi.odataService;

    $scope.onDataBound = function (e) {
        if ($rootScope.lastSelectedDataItem == null) {
            return;
        }

        var view = this.dataSource.view(); // get all the rows

        for (var i = 0; i < view.length; i++) {
            if (view[i].ID == $rootScope.lastSelectedDataItem.ID) {
                var grid = e.sender;

                grid.select(grid.table.find("tr[data-uid='" + view[i].uid + "']"));
                break;
            }
        }
    };

    /* Menu config function */
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