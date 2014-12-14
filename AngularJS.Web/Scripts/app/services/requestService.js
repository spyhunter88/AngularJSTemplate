'use strict';

app
	.constant('request.url', {
	    getRequests: 'api/Request',
	    getRequest: 'api/Request/'
	})
	.factory('request.api', ['request.url', 'coreSvc',
	function (url, coreSvc) {
	    var api = {
	        getRequests: function () {
	            return coreSvc.callApi('GET', url.getRequests, null, null);
	        },
	        getRequest: function (id) {
	            return coreSvc.callApi('GET', url.getRequest, { id: id }, null);
	        }
	    };

	    return api;
	}]);