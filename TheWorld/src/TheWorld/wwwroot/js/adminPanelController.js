(function () {
    "use strict";

    angular.module("app-admin")
    .controller("adminPanelController", adminPanelController);

    function adminPanelController($http, $scope, $timeout,ModalService) {


        var apiUrl = "/api/Admin";
        var pc = this;
        pc.errorMessage = "";
        pc.isBusy = true;
        pc.users = [];
        pc.reverse = false;
        pc.propertyName = "name";
        pc.sortBy = function (propertyName) {
            pc.reverse = pc.propertyName === propertyName ? !pc.reverse : false;
            pc.propertyName = propertyName;

        };

        
        $http.get(apiUrl)
                .then(function (response) {
                    angular.copy(response.data, pc.users);
                })
                .finally(function () {
                    
                });


        pc.ask = function (user) {
            ModalService.showModal({
                templateUrl: "modal.html",
                controller: "yesNoModalController"
            }).then(function (modal) {
                modal.element.modal();
                modal.close.then(function (result) {
                    if (result === "Yes")
                    {
                        pc.removeUser(user);
                    }
                });
            });
        }
        

        pc.removeUser = function (user) {
            pc.errorMessage = "";
            pc.isBusy = true;
            var index = pc.users.indexOf(user);
            $http.delete(apiUrl + "/delUser/" + user.userName)
                // working delete request
            .then(function (response) {
                pc.users.splice(index, 1);
                // Error deleting
            }, function (err) {
                pc.errormessage = "Failed to remove trip";
                window.alert(pc.errormessage);
            }).finally(function () {
                pc.isBusy = false;
            });
        };
    }


    angular.module("app-admin")
    .controller('yesNoModalController', function ($scope, close) {
        $scope.close = function (result) {
            close(result, 500);
        }
    }); 
})();