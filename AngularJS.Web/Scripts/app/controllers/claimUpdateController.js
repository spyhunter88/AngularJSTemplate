'use strict';

app.controller('claimUpdateController', ['$scope', '$routeParams', '$timeout', 'claim.api', 'category.api',
    function ($scope, $routeParams, $timeout, claimApi, catApi) {
        $scope.title = "Claim Management";
        $scope.options = {};

        var _init = function () {
            catApi.getCategories({ type: 'UNIT' }).then(function (data) {
                $scope.options.units = data;

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
        $scope.init();
        $timeout(function () { $scope.$apply(function() { $scope.load($routeParams.claimId) }) }, 500);
}]);