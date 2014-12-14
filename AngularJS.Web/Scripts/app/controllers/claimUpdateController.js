'use strict';

app.controller('claimUpdateController', ['$scope', '$routeParams', '$timeout', 'claim.api',
    function ($scope, $routeParams, $timeout, api) {
        $scope.title = "Claim Management";
        $scope.claim = new {};
        // alert("Edit");

        $scope.load = function (claimId) {
            api.getClaim(claimId).then(function(data) {
                $scope.claim = data;
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

        // Create new Claim
        var newClaim = function newClaim() {
            // validate first

            // save
            api.saveClaim($scope.claim).then(function (data) {

            }, function (error) {
            });
        };

        $timeout(function () { $scope.$apply(function() { $scope.load($routeParams.claimId) }) }, 500);

}]);