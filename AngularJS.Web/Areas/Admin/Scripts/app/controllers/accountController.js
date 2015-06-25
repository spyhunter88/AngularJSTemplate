'use strict';

app.controller('accountController', function ($scope, accountApi, ngToast) {
    $scope.users = [];

    // Call to retrieve account list from database
    accountApi.getAccounts().then(
        function (data) {
            $scope.users = data;
        }, function (err) {
            ngToast.create('Error');
        });
});