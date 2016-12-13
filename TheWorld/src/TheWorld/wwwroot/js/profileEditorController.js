(function () {
    "use strict";
    angular.module("app-profile")
    .controller("profileEditorController", profileEditorController);

    function profileEditorController($http, $scope, $timeout) {
        var vm = this;
        vm.user;
        vm.gai = "gaidys";
        var apiUrl = "/api/profile";
        vm.isProfilePic = true;
        $http.get(apiUrl)
                .then(function (response) {
                    vm.user = response.data;
                    window.alert(vm.user.userName);
                })
                .finally(function () {
                    vm.isProfilePic = (vm.user.profilePic != null);
                    window.alert(vm.isProfilePic);
                });
    }
})();