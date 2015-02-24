'use strict';

app
	.constant('vendor.url', {
	    getVendors: 'api/Vendor',
	    getVendor: 'api/Vendor/'
	})
	.factory('vendor.api', ['vendor.url', 'coreSvc',
	function (url, coreSvc) {
	    var api = {
	        getVendors: function (type) {
	            return coreSvc.callApi('GET', url.getVendors, type, null);
	        },
	        getVendor: function (id) {
	            return coreSvc.callApi('GET', url.getVendor, { id: id }, null);
	        }
	    };

	    return api;
	}]);