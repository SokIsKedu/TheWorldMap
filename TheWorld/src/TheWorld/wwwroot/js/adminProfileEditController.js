(function () {
    "use strict";

    angular.module("app-admin")
    .controller("profileEditorController", profileEditorController);

    function profileEditorController($http, $scope, $timeout,$routeParams) {
        var getUserUrl = "/api/Admin/getUser/";
        var updateUserUrl = "/api/profile";
        var vm = this;
        vm.userName = $routeParams.userName;
        
        vm.isProfilePic = true;
        $http.get(getUserUrl + vm.userName)
                .then(function (response) {
                    vm.user = response.data;
                    
                })
                .finally(function () {
                    vm.isProfilePic = vm.user.profilePic !== null;
                });

        $scope.saveUser = function () {
            $http.post(updateUserUrl, vm.user)
            .then(function (response) {
            })
            .finally(function (response) {
                adminPanelController();
            });
        };


    }
})();