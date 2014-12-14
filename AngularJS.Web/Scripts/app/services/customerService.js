'use strict';

app.factory('customersService', ['customerModel', 'localStorageService',
    function (customerModel, localStorageService) {
        var serviceBase = '';
        var crudServiceBaseUrl = serviceBase + "odata/Customer";
        var authData = localStorageService.get('authorizationData');
        // alert(authData.token);

        return new kendo.data.DataSource({
            type: "odata",
            transport: {
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
                        return crudServiceBaseUrl + "(" + data.CustomerID + ")";
                    },
                    type: "patch",
                    dataType: "json",
                    beforeSend: function (xhr) {
                        if (authData) {
                            xhr.setRequestHeader("Authorization", 'Bearer ' + authData.token);
                        }
                    }
                },
                destroy: {
                    url: function (data) {
                        return crudServiceBaseUrl + "(" + data.CustomerID + ")";
                    },
                    dataType: "json",
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
            pageSize: 10,
            schema: {
                data: function (data) { return data.value; },
                total: function (data) { return data["odata.count"]; },
                model: customerModel
            },
            error: function (e) {
                alert(e.xhr.responseText);
            }
        });

        // return customerDataSource;
    }]);