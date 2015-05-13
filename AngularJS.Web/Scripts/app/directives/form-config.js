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
                    var _getProperty = function (list, property) {
                        var res = [];
                        angular.forEach(list, function (obj) {
                            console.log(obj);
                            if (obj.FieldProperty == property) res.push(obj.ObjectField);
                        });
                        return res;
                    };

                    var objects = element[0].querySelectorAll('input, checkbox, select, textarea');
                    // console.log(scope.config);
                    if (scope.config === undefined) return;
                    var config = angular.fromJson(scope.config);

                    var _disabled = _getProperty(config, "disabled");

                    angular.forEach(objects, function (obj) {
                        var name = angular.element(obj).attr('name');
                        if (_disabled.indexOf(name) != -1) {
                            if (obj.tagName.toLowerCase() == "select") angular.element(obj).attr('disabled', true);
                            else {
                                angular.element(obj).attr('readonly', true);
                                angular.element(obj).attr('ng-value', angular.element(obj).attr('ng-model'));
                                angular.element(obj).removeAttr('ng-model');
                            }

                            // Re-update directive (i.e.: datepicker ...)
                            $compile(obj)(scope);
                        }
                    });
                });
            }
        };
    };
})();