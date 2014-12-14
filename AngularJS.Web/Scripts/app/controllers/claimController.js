'use strict';

app.controller('claimController', ['$scope', '$rootScope', 'claim.api',
    function ($scope, $rootScope, api) {
        $scope.title = "Claim Management";
        $scope.totalItems = 0;
		
        // init search value
        $scope.filterCriteria = {
            fullSearch: 'a', startDate: '15-08-2014', endDate: '', status: [1,10,11,12,13,14,15],
            vendor: '', bu: '', programType: '', participant: '',
            sortCol: 'createTime', sortDir: false,
			page: 1, pageSize: 10
		};
		
		// init header list
		$scope.headers = [
			{ colName: 'Status', name: 'Status', sortDir: 0 },
			{ colName: '', name: 'URL', sortDir: 0 },
			{ colName: 'BUName', name: 'BU', sortDir: 0 },
			{ colName: 'VendorName', name: 'Vendor', sortDir: 0 },
			{ colName: 'ProgramType', name: 'Program Type', sortDir: 0 },
			{ colName: 'VendorProgramCode', name: 'Program Code', sortDir: 0 },
			{ colName: '', name: 'Program Name', sortDir: 0 },
			{ colName: 'UnitClaim', name: 'Unit', sortDir: 0 },
			{ colName: '', name: 'Remain Payment', sortDir: 0 },
			{ colName: '', name: 'Remain Allocation', sortDir: 0 },
			{ colName: 'CreateTime', name: 'Create Time', sortDir: 1 }
		];

		var filter = function () {
			api.getClaims($scope.filterCriteria).then(function(data) {
				$scope.claims = data.claims;
				$scope.totalItems = data.total;
			}, function(error) {
				alert('Lỗi');
			});
			/*
		    $http
				.get('api/Claim', { params: $scope.filterCriteria })
				.success(function (data, status) {
				    $scope.claims = data.claims;


				    $scope.totalItems = data.total;
				});
			*/
		};
		
		$scope.selectPage = function(page) {
		    filter();
		};
		
		$scope.onClick = function() {
		    filter();
		};

		$scope.sortBy = function (colName) {
			if (colName == '') return;
		
		    if ($scope.filterCriteria.sortCol == colName) {
		        $scope.filterCriteria.sortDir = !$scope.filterCriteria.sortDir.reverse;
		    } else {
		        $scope.filterCriteria.sortCol = colName;
		        $scope.filterCriteria.sortDir = false;
		    }

		    for (var i = 0, len = $scope.headers.length; i < len; i++) {
		        if ($scope.headers[i].colName == colName) {
		            if ($scope.filterCriteria.sortDir) $scope.headers[i].sortDir = -1;
		            else $scope.headers[i].sortDir = 1;
		        } else {
		            $scope.headers[i].sortDir = 0;
		        }
		    }

		    filter();
		};
}]);