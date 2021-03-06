﻿/**
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
                    var objects = angular.element(element).find('input');
                    // console.log(scope.config);
                    if (scope.config === undefined) return;
                    var config = angular.fromJson(scope.config);

                    angular.forEach(objects, function (obj) {
                        var name = angular.element(obj).attr('name');
                        if (config.disable.indexOf(name) != -1) {
                            angular.element(obj).attr('ng-disabled', true);
                            var fn = $compile(obj);
                            fn(scope);
                        }
                    });
                });
            }
        };
    };


    /**
    * Disables given elements.
    *
    * @param {Array.<HTMLElement>|NodeList} elements List of dom elements that must be disabled
    */
    var disableElements = function (elements) {
        var len = elements.length;
        for (var i = 0; i < len; i++) {
            if (elements[i].disabled === false) {
                elements[i].disabled = true;
                elements[i].disabledIf = true;
            }
        }
    };

    /**
     * Enables given elements.
     *
     * @param {Array.<HTMLElement>|NodeList} elements List of dom elements that must be enabled
     */
    var enableElements = function (elements) {
        var len = elements.length;
        for (var i = 0; i < len; i++) {
            if (elements[i].disabled === true && elements[i].disabledIf === true) {
                elements[i].disabled = false;
                elements[i].disabledIf = null;
            }
        }
    };

})();