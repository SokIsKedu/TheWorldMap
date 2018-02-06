(function () {
    "use strict";
    // Creating the module
    angular.module("app-admin", ["ngRoute", "720kb.datepicker", "ngAnimate","angularModalService"])
    .config(function ($routeProvider) {
        $routeProvider.when("/", {
            controller: "adminPanelController",
            controllerAs: "pc",
            templateUrl: "/views/admin.html"
        });
        $routeProvider.when("/editProfile/:userName",
            {
                controller: "profileEditorController",
                controllerAs: "vm",
                templateUrl: "/views/profileEditorView.html"
            });
        $routeProvider.otherwise({ redirectTo: "/" });
    });

})();