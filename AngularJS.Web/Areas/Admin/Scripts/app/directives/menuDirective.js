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
                // define recursive function to add hasSub and level attributes
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

    /**
    *
    *
    */
    angular.module('myMenu').directive('ngMenuTable', ngMenuTable);
    function ngMenuTable($compile) {
        return {
            restrict: 'E',
            scope: {
                menu: '=',
                selectedId: '=?',
                autoGenerateSelectedId: '=?'
            },
            templateUrl: 'menu-tree.html',
            controller: function ($scope, $filter) {
                if ($scope.autoGenerateSelectedId == undefined || $scope.autoGenerateSelectedId || $scope.selectedId == undefined) {
                    $scope.selectedId = [];
                    for (var i = 0, len = $scope.menu.length; i < len; i++)
                        recursiveGet($scope.selectedId, $scope.menu[i]);
                } else {
                    for (var i = 0, len = $scope.menu.length; i < len; i++)
                        recursiveSet($scope.selectedId, $scope.menu[i]);
                };

                // Set select mode for all node have ID match in selectArr
                function recursiveSet(selectArr, item) {
                    if (selectArr.indexOf(item.id) != -1) {
                        item.selected = true;
                        $scope.$broadcast('changeChildren', item);
                    } else {
                        if (item.children != null) {
                            for (var i = 0, len = item.children.length; i < len; i++) 
                                recursiveSet(selectArr, item.children[i]);
                        }
                    }
                };

                // Get select ID from all nodes using recursive
                function recursiveGet(selectArr, item) {
                    // if (selectArr == null) selectArr = []; // put it outside the function to improve performance
                    if (item.children == null || item.children.length == 0) {
                        selectArr.push(item.id);
                    } else {
                        for (var i = 0, len = item.children.length; i < len; i++)
                            recursiveGet(selectArr, item.children[i]);
                    }
                };

                $scope.toggleAllCheckboxes = function($event) {
                    var item, selected = $event.target.checked;
                    for (var i = 0, len = $scope.menu.length; i < len; i++) {
                        item = $scope.menu[i];
                        item.selected = selected;
                        if (item.children)
                            $scope.$broadcast('changeChildren', item);
                    }
                };

                $scope.initCheckbox = function(item, parentItem) {
                    return item.selected = parentItem && parentItem.selected || 
                                item.selected || false;
                };

                $scope.toggleCheckbox = function(item, parentScope) {
                    if (item.children != null) {
                        $scope.$broadcast('changeChildren', item);
                    }
                    if (parentScope.item != null) {
                        return $scope.$emit('changeParent', parentScope);
                    }
                };

                $scope.$on('changeChildren', function (event, parentItem) {
                    var child;
                    for (var i = 0, len = parentItem.children.length; i < len; i++) {
                        child = parentItem.children[i];
                        child.selected = parentItem.selected;
                        if (child.children != null) {
                            $scope.$broadcast('changeChildren', child);
                        }
                    }
                });

                $scope.$on('changeParent', function(event, parentScope) {
                    var children;
                    children = parentScope.item.children;
                    parentScope.item.selected = $filter('selected')(children).length == children.length;
                    parentScope = parentScope.$parent.$parent;
                    if (parentScope.item != null) {
                        $scope.$broadcast('changeParent', parentScope);
                    }
                });
            }
        }
    } //end of function ngMenuTable
    angular.module('myMenu').filter('selected', ['$filter', function($filter) {
        return function(files) {
            return $filter('filter')(files, {selected: true });
        };
    }]);

})();