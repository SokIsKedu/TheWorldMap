

(function () {
    "use strict";
    //Getting the exsisting module
    angular.module("app-trips")
    .controller("tripsController", tripsController);

    function tripsController($http) {
        var vm = this;
        vm.trips = [];
        vm.newTrip = {};
        
        vm.errorMessage = "";
        vm.isBusy = true;
        var apiUrl ="/api/trips";
        vm.reverse = false;
        vm.propertyName = "name";

        //Rusiuoti
        vm.sortBy = function (propertyName) {
            vm.reverse = vm.propertyName === propertyName ? !vm.reverse : false;
            vm.propertyName = propertyName;

        };

        $http.get(apiUrl)
            .then(function (response) {
                angular.copy(response.data, vm.trips);
            }, function (error) {
                vm.errorMessage = "Failed to load data: " + error;
            }).finally(function () {
                vm.isBusy = false;
        });

        vm.addTrip = function () {
            vm.errorMessage = "";

            if (contains(vm.newTrip)) {
                vm.errorMessage = "Name already exists";
            } else {
                vm.isBusy = true;
                $http.post(apiUrl, vm.newTrip)
                .then(function (response) {
                    vm.trips.push(response.data);
                    vm.newTrip = {};
                }, function () {
                    vm.errorMessage = "Failed to save new trip";
                }).finally(function () {
                    vm.isBusy = false;
                });
            }
        };
        
        vm.removeTrip = function (trip) {
            alert("eina");
            vm.errorMessage = "";
            vm.isBusy = true;
            var index = vm.trips.indexOf(trip);
            $http.delete(apiUrl + "/deltrip/" + trip.name)
                // working delete request
            .then(function (response) {
                vm.trips.splice(index, 1);
                alert("eina");
                // Error deleting
            }, function (err) {
                vm.errormessage = "Failed to remove trip";
                window.alert(vm.errormessage);
            }).finally(function () {
                vm.isBusy = false;
            });
        };

        var contains = function (trip) {
            var contains = false;
            for (var i = 0; i < vm.trips.length; i++) {
                if (trip.name === vm.trips[i].name) {
                    alert(!contains);
                    return !contains;
                }
            }
            return contains;
        };
    }
})();

(function () {
    var $sidebarAndWrapper = $("#sidebar,#wrapper");
    $("#sidebarToggle").on("click", function () {
        $sidebarAndWrapper.toggleClass("hide-sidebar");
        if ($sidebarAndWrapper.hasClass("hide-sidebar")) {
            $(this).html("Show Sidebar  <i class='fa fa-angle-right'></i>");
        } else {
            $(this).html("<i class='fa fa-angle-left'></i>  Hide Sidebar");
        }
    });

    document.getElementById("uploadBtn").onchange = function () {
        document.getElementById("uploadFile").value = this.value;
    };

})();