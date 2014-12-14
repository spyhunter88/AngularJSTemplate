'use strict';

app.controller('requestController', ['$scope', '$rootScope', 'request.api',
    function ($scope, $rootScope, api) {
        $scope.title = "Request Management";
        $scope.totalItems = 0;

        // init header list
        $scope.headers = [
			{ colName: 'Status', name: 'Status', sortDir: 0 },
			{ colName: '', name: 'URL', sortDir: 0 },
			{ colName: 'VendorName', name: 'Vendor', sortDir: 0 },
			{ colName: 'ProductLine', name: 'Product Line', sortDir: 0 },
			{ colName: 'Representation', name: 'Representation', sortDir: 0 },
			{ colName: 'RepresentationPosition', name: 'Representation Pos', sortDir: 0 },
			{ colName: '', name: 'Program Name', sortDir: 0 },
			{ colName: 'Model', name: 'Model', sortDir: 0 },
			{ colName: 'Target', name: 'Target', sortDir: 0 },
			{ colName: 'StartDate', name: 'Start Date', sortDir: 0 },
			{ colName: 'EndDate', name: 'End Date', sortDir: 1 }
        ];

        var filter = function () {
            api.getRequests().then(function (data) {
                $scope.requests = data.requests;
                $scope.totalItems = data.total;
            }, function (error) {
                // $window.alert('Lỗi');
                console.log('Something wrong when access Request Service!');
            });
        };

        $scope.onClick = function () {
            filter();
        };
    }
]);
