'use strict';

app
	.constant('menu.url', {
	    getMenus: 'api/Menu'
	})
	.factory('menuApi', ['menu.url', 'coreSvc',
	function (url, coreSvc) {
	    var api = {
	        getMenus: function () {
	            return coreSvc.callApi('GET', url.getMenus, null, null);
	        }
	    };

	    return api;
	}]);