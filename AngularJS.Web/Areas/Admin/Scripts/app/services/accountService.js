'use strict';

app
	.constant('admin.account.url', {
	    getAccounts: '/api/Admin/Account',
	    getAccount: '/api/Admin/Account/',
	    postAccount: '/api/Admin/Account',
        getRoles: '/api/Admin/Role'
	})
	.factory('accountApi', ['admin.account.url', 'coreSvc',
	function (url, coreSvc) {
	    var api = {
	        getAccounts: function () {
	            return coreSvc.callApi('GET', url.getAccounts, null, null);
	        },
	        getAccount: function (id) {
	            return coreSvc.callApi('GET', url.getAccount, { id: id }, null);
	        },
	        createAccount: function (account) {
	            return coreSvc.callApi('POST', url.postAccount, { action: 'create' }, account);
	        },
	        saveAccount: function (account, action) {
	            return coreSvc.callApi('POST', url.postAccount, { action: action }, account);
	        },
	        putAccount: function (account, action) {
	            return coreSvc.callApi('PUT', url.postAccount, { action: action }, account);
	        },

	        getRoles: function () {
	            return coreSvc.callApi('GET', url.getRoles, null, null);
	        }
	    };

	    return api;
	}]);