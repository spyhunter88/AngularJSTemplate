'use strict';
app.controller('homeController', function ($rootScope, $scope, $location, AUTH_EVENTS,
    menuApi, ngToast, authService, dialogs) {
    
    $scope.menus = [];
    menuApi.getUserMenus().then(
        function (data) {
            if (data != '')
            $scope.menus = data.nestedArray('submenus', 'id', 'parentID');
        }, function (err) {
            ngToast.error('Error while loading Menu!');
        }
    );

    var _logout = function () {
        authService.logOut();
        $location.path('/home');
    };
    var _login = function () {
        var modal = dialogs.create(
            '/Scripts/app/views/dialogs/loginForm.html?bust=' + Math.random().toString(36).slice(2),
            'loginController',
            {}, { size: 'sm'}
            );
    };

    // return false if time expire
    var _isAuth = function () {
        var xp = moment(authService.authentication.expireTime).diff(moment(), 's');
        if (authService.authentication.isAuth && xp > 0) return true;
        else return false;
    };

    // Get authentication information to show in menu
    $scope.authentication = authService.authentication;
    $scope.isAuth = _isAuth;
    $scope.login = _login;
    $scope.logout = _logout;

    // register listener
    $scope.$on(AUTH_EVENTS.notAuthenticated, _login);
    $scope.$on(AUTH_EVENTS.notAuthorized, _login);
    $scope.$on(AUTH_EVENTS.sessionTimeout, _login);
	// $scope.$on(AUTH_EVENTS.loginSuccess, hideDialog);
})
.controller('loginController', function ($scope, $modalInstance, data, authService, AUTH_EVENTS) {
    $scope.credentials = { userName: "", password: "" };
    $scope.message = '';

    $scope.login = function () {
        authService.login($scope.credentials).then(function (response) {
            $scope.message = "Login success!";

            // return to home if login from main
            if ($location.path().indexOf('login') != -1) $location.path('/');
            else $modalInstance.close({});
        }, function (err) {
            $scope.message = "Login failed!";
            // $rootScope.$broadcast(AUTH_EVENTS.loginFailed);
        });
    };
})
;