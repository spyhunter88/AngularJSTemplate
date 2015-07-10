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
                                'ng-include="\'/Scripts/ng-misc/my-menu/menu-sub.html\'"></li>'
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
                selectedId: '=?'
            },
            templateUrl: '/Scripts/ng-misc/my-menu/menu-tree.html',
            controller: function ($scope, $filter) {
                if ($scope.selectedId == undefined) {
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
                        if (item.children)
                            $scope.$broadcast('changeChildren', item);
                    } else {
                        item.selected = false;
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
                        if (item.selected) selectArr.push(item.id);
                    } else {
                        for (var i = 0, len = item.children.length; i < len; i++)
                            recursiveGet(selectArr, item.children[i]);
                    }
                };

                function updateSelectedId(id, addOrRemove) {
                    var idx = $scope.selectedId.indexOf(id);
                    if (addOrRemove && idx == -1) $scope.selectedId.push(id);
                    if (!addOrRemove && idx != -1) $scope.selectedId.splice(idx, 1);
                };

                $scope.toggleAllCheckboxes = function($event) {
                    var item, selected = $event.target.checked;
                    for (var i = 0, len = $scope.menu.length; i < len; i++) {
                        item = $scope.menu[i];
                        item.selected = selected;
                        if (selected) $scope.selectedId.push(item.id);
                        else $scope.selectedId = [];
                        if (item.children)
                            $scope.$broadcast('changeChildren', item);
                    }
                };

                $scope.initCheckbox = function(item, parentItem) {
                    return item.selected = parentItem && parentItem.selected || 
                                item.selected || false;
                };

                $scope.toggleCheckbox = function (item, parentScope) {
                    updateSelectedId(item.id, item.selected);
                    if (item.children != null) {
                        $scope.$broadcast('changeChildren', item);
                    }
                    if (parentScope.item != null) {
                        return $scope.$emit('changeParent', parentScope);
                    }
                };

                $scope.$on('changeChildren', function (event, parentItem) {
                    var child, id;
                    for (var i = 0, len = parentItem.children.length; i < len; i++) {
                        child = parentItem.children[i]; id = parentItem.children[i].id;
                        child.selected = parentItem.selected;
                        // Add/Remove child's id into select list
                        updateSelectedId(id, parentItem.selected);
                        if (child.children != null) {
                            $scope.$broadcast('changeChildren', child);
                        }
                    }
                });

                $scope.$on('changeParent', function(event, parentScope) {
                    var children = parentScope.item.children, id = parentScope.item.id,
                        selected = $filter('selected')(children).length == children.length;
                    parentScope.item.selected = $filter('selected')(children).length == children.length;
                    parentScope = parentScope.$parent.$parent;
                    // Add/Remove child's id into select list
                    updateSelectedId(id, selected);
                    if (parentScope.item != null) {
                        $scope.$broadcast('changeParent', parentScope);
                    }
                });

                // watch for selectedId changed from outside directive (don't know why inside is not raise here)
                $scope.$watch('selectedId', function (val) {
                    for (var i = 0, len = $scope.menu.length; i < len; i++)
                        recursiveSet(val, $scope.menu[i]);
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