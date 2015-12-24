'use strict';

app.controller('categoryController',
    function ($scope, $rootScope, $location, categoriesService) {
        categoriesService.filter({});

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

        $scope.title = 'Category List';
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

        $scope.dataSource = categoriesService;

        $scope.onDataBound = function (e) {
            if ($rootScope.lastSelectedDataItem == null) {
                return;
            }

            var view = this.dataSource.view(); // get all the rows

            for (var i = 0; i < view.length; i++) {
                if (view[i].CategoryID == $rootScope.lastSelectedDataItem.CategoryID) {
                    var grid = e.sender;

                    grid.select(grid.table.find("tr[data-uid='" + view[i].uid + "']"));
                    break;
                }
            }
        };
    });