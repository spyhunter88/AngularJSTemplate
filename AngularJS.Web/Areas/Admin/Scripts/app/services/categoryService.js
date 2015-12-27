'use strict';

app.factory('categoryModel', function () {
    return kendo.data.Model.define({
        id: "CategoryID",
        fields: {
            Type: { type: "string", nullable: false },
            Code: { type: "string" },
            Value: { type: "string" },
            Description: { type: "string" }
        }
    });
});

app.factory('categoriesService', ['categoryModel', 'localStorageService',
    function (categoryModel, localStorageService) {
        var serviceBase = '';
        var crudServiceBaseUrl = serviceBase + "/odata/Admin/Category";
        var authData = localStorageService.get('authorizationData');

        return new kendo.data.DataSource({
            type: 'odata',
            transport: {
                create: {
                    async: false,
                    url: crudServiceBaseUrl,
                    dataType: "json",
                    beforeSend: function (xhr) {
                        if (authData) {
                            xhr.setRequestHeader("Authorization", 'Bearer ' + authData.token);
                        }
                    }
                },
                read: {
                    async: false,
                    url: crudServiceBaseUrl,
                    dataType: "json",
                    beforeSend: function (xhr) {
                        if (authData) {
                            xhr.setRequestHeader("Authorization", 'Bearer ' + authData.token);
                        }
                    }
                },
                update: {
                    url: function (data) {
                        return crudServiceBaseUrl + '(' + data.CategoryID + ')';
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
                        return crudServiceBaseUrl + '(' + data.CategoryID + ')';
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
                model: categoryModel
            },
            error: function (e) {
                alert(e.xhr.responseText);
            }

        });

    }]);