(function () {
    'use strict';

    angular.module('app-profile')
    .controller('profileEditorController', profileEditorController);

    function profileEditorController($http, $scope, $timeout) {
        //vm.isPictureSelected = false;
        var vm = this;
        vm.user;
        var apiUrl = "/api/profile";
        vm.isProfilePic = true;
        $http.get(apiUrl)
                .then(function (response) {
                    vm.user = response.data;
                })
                .finally(function () {
                    vm.isProfilePic = (vm.user.profilePic != null);
                });
    }
})();