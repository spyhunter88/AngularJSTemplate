/**
* @author: spyhunter88
*/

(function () {

    angular.module('myMenu', []);

    angular.module('myMenu').directive('ngMenuJson', menuJson);

    function menuJson($compile) {
        return {
            restrict: 'E',
            scope: {
                menus: '='

            },
            // template: '<ng-include ng-repeat="menu in menus" src="\'Scripts/app/directives/menu-sub.html\'"></ng-include>',
            compile: function (tElement, tAttr) {
                // define recursive function
                var recursive = function (menu, level) {
                    if (menu.submenus == undefined || menu.submenus.length == 0) {
                        menu.level = level;
                        menu.hasSub = false;
                    } else {
                        menu.hasSub = true;
                        for (var i = 0; i < menu.submenus.length; i++) {
                            recursive(menu.submenus[i], level + 1);
                        }
                    }
                }; // end of recursive function

                var parent = tElement.parent();
                // tElement.remove();
                return function link(scope, element, attrs) {
                    if (scope.isGenerated) return;

                    scope.$watch('menus', function (data) {
                        var level = 0;
                        recursive(scope.menus, level);
                        console.log(scope.menus);

                        // remove exists 
                        parent.find('li[is-menu-json]').remove();
                        var position = parent.children().index(element);
                        scope.isGenerated = true;
                        // Populate the first level
                        angular.forEach(scope.menus.slice().reverse(), function (item) {
                            // element.remove();
                            if (!item.hasSub) {
                                var html = '<li is-menu-json data-match-route="' + item.route + '"><a href="' + item.href + '">' + item.title + '</a></li>';
                            } else {
                                var html = '<li is-menu-json><a href="' + item.href + '" class="dropdown-toggle">'
                                        + item.title + '<b class="caret"></b>'
                                        + '</a><ul class="dropdown-menu">'
                                        + '<li data-match-route="' + item.route + '" class="dropdown-submenu" ng-repeat="menu in menu.submenus" '
                                        + 'ng-include="\'Scripts/app/directive/menu-sub.html\'"></li></ul></li>';
                            }
                            var newMenu = angular.element(html);
                            parent.insertAt(newMenu, position);
                            $compile(newMenu)(scope);
                        });
                        scope.isGenerated = false;

                    });
                }
            }
        }; // end of return
    };

})();