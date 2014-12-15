'use strict';

app.controller('claimUpdateController', ['$scope', '$routeParams', '$timeout', 'claim.api', 'category.api', 'ngToast',
    function ($scope, $routeParams, $timeout, claimApi, catApi, ngToast) {
        $scope.title = "Claim Management";
        $scope.options = {};

        var _init = function () {
            catApi.getCategories({ type: 'UNIT' }).then(function (data) {
                $scope.options.units = data;
				ngToast.create('Load UNIT success!');
            }, function (error) {
                // ngToast.create('Error due loading UNIT!');
            });
        };

        // Need to load BU, Participant, Unit, Vendor, Program Type, Payment, ProductLine list
        $scope.bus = ['F9', 'F5', 'FAP', 'FHP'];

        $scope.load = function (claimId) {
            claimApi.getClaim(claimId).then(function (data) {
                $scope.claim = data;
                $scope.actions = {};
                $scope.actions.isSave = false;
            }, function(error) {
                alert('Lỗi');
            });

            /*
            $http.get('api/Claim/' + claimId)
                .success(function (data, status) {
                    $scope.claim = data;
            });
            */
        };
		
		var _addCP = function(cp) {
			cp.id = 0;
			$scope.claim.checkPoints.push(cp);
		};
		
		var _removeCP = function(cp) {
			var idx = $scope.claim.checkPoints.indexOf(cp);
			$scope.claim.checkPoints.splice(idx, 1);
		};

        // Create new Claim
        var newClaim = function () {
            // validate first
            // save
            claimApi.saveClaim($scope.claim).then(function (data) {
                $scope.claim = data;
            }, function (error) {
                alert('1123');
            });
        };

        $scope.init = _init;
		$scope.addCheckPoint = _addCP;
		$scope.removeCheckPoint = _removeCP;
        $scope.init();
        $timeout(function () { $scope.$apply(function() { $scope.load($routeParams.claimId) }) }, 500);
}]);