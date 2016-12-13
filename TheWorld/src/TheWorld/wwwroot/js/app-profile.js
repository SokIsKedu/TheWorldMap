(function () {
    "use strict";
    // Creating the module
    angular.module("app-profile", ["ngRoute", "720kb.datepicker", "ngAnimate"])
    .config(function ($routeProvider) {
        $routeProvider.when("/", {
            controller: "profileController",
            controllerAs: "pc",
            templateUrl: "/views/profileView.html"
        });
        $routeProvider.when("/editProfile",
            {
                controller: "profileEditorController",
                controllerAs: "vm",
                templateUrl: "/views/profileEditorView.html"
            });
        $routeProvider.otherwise({ redirectTo: "/" });
    });

})();