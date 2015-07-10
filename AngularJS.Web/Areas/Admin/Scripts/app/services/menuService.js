'use strict';

app
    .constant('menu.url', {
        getAllMenu: '/api/admin/Menu',
        getMenu: '/api/admin/Menu',
        postMenu: '/api/admin/Menu'
    })
    .factory('menuApi', ['menu.url', 'coreSvc',
	function (url, coreSvc) {
	    var api = {
	        getAllMenus: function() {
	            return coreSvc.callApi('GET', url.getAllMenu, null, null);
	        },
			getMenus: function(userId, roleId) {
			    return coreSvc.callApi('GET', url.getMenu, { userId: userId, roleId: roleId }, null);
			},
			postMenus: function(menus) {
			    return coreSvc.callApi('POST', url.postMenu, { userId: userId, roleId: roleId }, menus);
			}
		};
		
		return api;
    }
]);