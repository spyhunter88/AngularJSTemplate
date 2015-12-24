'use strict';

app.factory('menuItemModel', function () {
    return kendo.data.Model.define({
        id: "ID",
        fields: {
            Href: { type: "string", nullable: false },
            Title: { type: "string", nullable: false },
            Route: { type: "string" },
            ParentID: { type: "number" },
            Module: { type: "string" }
        }
    });
});

app
    .constant('menu.url', {
        getAllMenu: '/api/admin/Menu',
        getUserMenu: '/api/admin/Menu',
        getMenu: '/api/admin/Menu',
        postMenu: '/api/admin/Menu',
        crudServiceBaseUrl: '/odata/Admin/MenuItem'
    })
    .factory('menuApi', ['menu.url', 'coreSvc', 'menuItemModel', 'APP_SETTINGS', 'localStorageService',
	function (url, coreSvc, menuItemModel, APP_SETTINGS, localStorageService) {
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
			},
			odataService: function () {
			    var authData = localStorageService.get('authorizationData');

			    return new kendo.data.DataSource({
			        type: 'odata',
			        transport: {
			            read: {
			                async: false,
			                url: url.crudServiceBaseUrl,
			                dataType: "json",
			                beforeSend: function (xhr) {
			                    if (authData) {
			                        xhr.setRequestHeader("Authorization", 'Bearer ' + authData.token);
			                    }
			                }
			            },
			            update: {
			                url: function (data) {
			                    return crudServiceBaseUrl + '(' + data.ID + ')';
			                },
			                type: 'patch',
			                dataType: 'json',
			                beforeSend: function (xhr) {
			                    if (authData) {
			                        xhr.setRequestHeader("Authorization", 'Bearer ' + authData.token);
			                    }
			                }
			            },
			            destroy: {
			                url: function (data) {
			                    return crudServiceBaseUrl + '(' + data.ID + ')';
			                },
			                dataType: 'json',
			                beforeSend: function (xhr) {
			                    if (authData) {
			                        xhr.setRequestHeader("Authorization", 'Bearer ' + authData.token);
			                    }
			                }
			            }
			        },
			        batch: false,
			        serverPaging: true,
			        serverSorting: true,
			        serverFiltering: true,
			        pageSize: 20,
			        schema: {
			            data: function (data) { return data.value; },
			            total: function (data) { return data["odata.count"]; },
			            model: menuItemModel
			        },
			        error: function (e) {
			            alert(e.xhr.responseText);
			        }

			    });
			} // end of "odataService"
		};
		
		return api;
    }
]);