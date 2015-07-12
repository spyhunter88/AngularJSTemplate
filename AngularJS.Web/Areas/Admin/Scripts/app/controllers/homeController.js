'use strict';
app.controller('homeController', function ($scope, $location, menuApi, ngToast, authService) {
    
    $scope.menus = [];
    menuApi.getUserMenus().then(
        function (data) {
            $scope.menus = data.nestedArray('submenus', 'id', 'parentID');
        }, function (err) {
            ngToast.create('Error while loading Menu!');
        });

    $scope.logout = function () {
        authService.logOut();
        $location.path('/home');
    }

    // return false if time expire
    var _isAuth = function () {
        var xp = moment(authService.authentication.expireTime).diff(moment(), 's');

        if (authService.authentication.isAuth && xp > 0) return true;
        else return false;
    };

    // Get authentication information to show in menu
    $scope.authentication = authService.authentication;
    $scope.isAuth = _isAuth;
}); 