'use strict';
app.controller('homeController', function ($scope, menuApi, ngToast) {
    
    $scope.menus = [];
    menuApi.getMenus().then(
        function (data) {
            $scope.menus = data.nestedArray('submenus', 'id', 'parentID');
            console.log($scope.menus);
        }, function (err) {
            ngToast.create('Error while loading Menu!');
        });
}); 