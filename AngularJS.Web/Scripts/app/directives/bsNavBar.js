
(function () {
    angular.module('bsNavBarMod', [])
        .directive('bsNavbar', function ($location) {
            'use strict';

            return {
                restrict: 'A',
                link: function postLink(scope, element, attrs, controller) {
                    // Watch for the $location
                    scope.$watch(function () {
                        return $location.path();
                    }, function (newValue, oldValue) {

                        $('li[data-match-route]', element).each(function (k, li) {
                            var $li = angular.element(li);
                            // data('match-rout') does not work with dynamic attributes
                            var pattern = $li.attr('data-match-route');
                            if (pattern == '/') pattern = pattern + '$';
                            var regexp = new RegExp('^' + pattern, ['i']);
                            console.log(regexp);

                            if (regexp.test(newValue)) {
                                $li.addClass('active');
                            } else {
                                $li.removeClass('active');
                            }

                        });
                    });
                }
            };
        });
}).call(this);
