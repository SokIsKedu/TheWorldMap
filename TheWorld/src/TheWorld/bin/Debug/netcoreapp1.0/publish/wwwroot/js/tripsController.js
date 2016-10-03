(function () {
    "use strict";
    //Getting the exsisting module
    angular.module("app-trips")
    .controller("tripsController", tripsController);

    function tripsController($http) {
        var vm = this;
        vm.trips = [];
        vm.newTrip = {};
        vm.name = "lopas";
        vm.errorMessage = "";
        vm.isBusy = true;

        $http.get("/api/trips")
            .then(function (response) {
                angular.copy(response.data, vm.trips)
            }, function (error) {
                vm.errorMessage = "Failed to load data: " + error;
            }).finally(function () {
                vm.isBusy = false;

        });

        vm.addTrip = function () {
            vm.errorMessage = "";
            vm.isBusy = true;
            $http.post("/api/trips", vm.newTrip)
            .then(function (response) {
                vm.trips.push(response.data);
                vm.newTrip = {};
            }, function () {
                vm.errorMessage = "Failed to save new trip";
            }).finally(function ()
            {
                vm.isBusy = false;
            })
        }
    }
})();