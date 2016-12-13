(function () {
    "use strict";

    angular.module("app-profile")
    .controller("profileController",profileController);

    function profileController($http,$scope,$timeout){
        var pc = this;
        pc.user;
        var apiUrl = "/api/profile";
        pc.isProfilePic = true;
        $http.get(apiUrl)
                .then(function (response) {
                   pc.user = response.data;
                })
                .finally(function () {
                    pc.isProfilePic = (pc.user.profilePic != null);   
                });
    }
})();