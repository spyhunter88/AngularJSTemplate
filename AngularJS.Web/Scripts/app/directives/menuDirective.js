/**
* @author: spyhunter88
* require: ui.bootstrap
*/

(function () {

    angular.module('myMenu', ['ui.bootstrap']);

    angular.module('myMenu').directive('ngMenuJson', menuJson);

    function menuJson($compile) {
        return {
            restrict: 'E',
            scope: {
                menus: '='
            },
            compile: function (tElement, tAttr) {
                // define recursive function
                var recursive = function (menu, level) {
                    menu.level = level;
                    if (menu.submenus == undefined || menu.submenus.length == 0) {
                        menu.hasSub = false;
                    } else {
                        menu.hasSub = true;
                        for (var i = 0; i < menu.submenus.length; i++) {
                            recursive(menu.submenus[i], level + 1);
                        }
                    }
                }; // end of recursive function

                // save parent for later use
                var parent = tElement.parent();
                // tElement.remove();
                return function link(scope, element, attrs) {

                    scope.$watch('menus', function (data) {
                        // processing input data
                        var level = 0;
                        for (var i = 0; i < scope.menus.length; i++) {
                            recursive(scope.menus[i], level);
                        }
                    });

                    var position = parent.children().index(element);

                    var html = '<li is-menu-json data-match-route="{{menu.route}}"' +
                                'class="dropdown" dropdown ng-repeat="menu in menus" ' +
                                'ng-include="\'menu-sub.html\'"></li>'
                                ;

                    // insert after element
                    var newMenu = angular.element(html);
                     parent.insertAt(newMenu, position);
                    $compile(newMenu)(scope);
                }
            }
        }; // end of main return
    };

})();