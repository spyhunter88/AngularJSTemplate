/**
 * @author LinhNH <linhnh2805@gmail.com>
 */
(function () {
    'use strict';

    angular.module('formConfig', []);

    angular.module('formConfig').directive('formConfig', formConfig);

    /**
    * @ngInject
    */
    function formConfig($compile) {
        return {
            restrict: 'A',
            scope: {config: '=formConfig' },
            link: function (scope, element, attrs) {
                // watch 'config' to listen to config outside
                scope.$watch('config', function () {
                    var objects = element[0].querySelectorAll('input, checkbox, select, textarea');
                    // console.log(scope.config);
                    if (scope.config === undefined) return;
                    var config = angular.fromJson(scope.config);

                    angular.forEach(objects, function (obj) {
                        var name = angular.element(obj).attr('name');
                        if (config.disable.indexOf(name) != -1) {
                            // angular.element(obj).attr('readonly', true);
                            angular.element(obj).attr('disabled', true);
                        }
                    });
                });
            }
        };
    };
})();