'use strict';

app
    .constant('menu.url', {
        getAllMenu: '/api/admin/Menu',
        getUserMenu: '/api/admin/Menu',
        getMenu: '/api/admin/Menu',
        postMenu: '/api/admin/Menu'
    })
    .factory('menuApi', ['menu.url', 'coreSvc', 'APP_SETTINGS',
	function (url, coreSvc, APP_SETTINGS) {
	    var api = {
	        getAllMenus: function() {
	            return coreSvc.callApi('GET', url.getMenu, { module: "" }, null);
	        },
	        getUserMenus: function() {
	            return coreSvc.callApi('GET', url.getMenu, { module: APP_SETTINGS.Module_Name }, null);
	        },
			getMenus: function(userId, roleId) {
			    return coreSvc.callApi('GET', url.getMenu, { userId: userId, roleId: roleId }, null);
			},
			saveMenus: function (userId, roleId, ids) {
			    return coreSvc.callApi('POST', url.postMenu, { userId: userId, roleId: roleId }, ids);
			}
		};
		
		return api;
    }
]);