'use strict';

app
	.constant('admin.account.url', {
	    getAccounts: '/api/admin/Account',
	    getAccount: '/api/admin/Account/',
	    postAccount: '/api/admin/Account',
	    getRoles: '/api/admin/Role',

        getPolicy: '/api/admin/Policy',
        postPolicy: '/api/admin/Policy'
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
            // Get only role list
	        getRoles: function () {
	            return coreSvc.callApi('GET', url.getRoles, null, null);
	        },

	        getPolicy: function(userId, roleId) {
	            return coreSvc.callApi('GET', url.getPolicy, { userId: userId, roleId: roleId }, null);
	        },
	        savePolicy: function (policy) {
	            return coreSvc.callApi('POST', url.postPolicy, null, policy);
	        }
	    };

	    return api;
	}]);