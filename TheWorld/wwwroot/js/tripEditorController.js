(function () {
    "use strict";

    
    



    angular.module("app-trips")
    .controller("tripEditorController", tripEditorController);




    function tripEditorController($routeParams, $http) {
       
        var vm = this;
        vm.tripName = $routeParams.tripName;
        vm.stops = [];
        vm.errormessage = "";
        vm.isBusy = true;
        vm.show = true;
        vm.newStop = {};
        var apiUrl = "/api/trips/" + vm.tripName + "/stops";
        var apiUrlDeleteStop = apiUrl + "/delstop"
        
        vm.reverse = false;
        vm.propertyName = "name";
        

        //Rusiuoti
        vm.sortBy = function (propertyName) {
            vm.reverse = (vm.propertyName === propertyName) ? !vm.reverse : false;
            vm.propertyName = propertyName;

        };
       

        


        $http.get(apiUrl)
        .then(function (response) {
            //success
            angular.copy(response.data, vm.stops);
            _showMap(vm.stops);
        }, function (err) {
            //fail
            vm.errormessage = "Failed tu load your trip stops";
        })
        .finally(function () {
            vm.isBusy = false;
        });
       

        vm.addStop = function () {
            vm.isBusy = true;
            vm.newStop.order = vm.stops.length;
            $http.post(apiUrl, vm.newStop)
            .then(function (response) {
                vm.stops.push(response.data);
                _showMap(vm.stops); 
                vm.newStop = {};
                vm.show = true;
            }, function (err) {
                vm.errormessage = "Failed to add new stop";
            })
            .finally(function () {
                vm.isBusy = false;
            });
        };

        vm.removeStop = function (stop) {
            vm.isBusy = true;
            var index = vm.stops.indexOf(stop);
            $http.post(apiUrlDeleteStop, vm.stops[index])
                // working delete request
            .then(function (response) {               
                vm.stops.splice(index, 1);
                _showMap(vm.stops);
                if (vm.stops.length == 0) {
                    vm.show = false;
                }

                // Error deleting
            }, function (err) {                
                vm.errormessage = "Failed to remove stop";
                window.alert(vm.errormessage);
            }).finally(function () {
                vm.isBusy = false;
            });
         };
    }








   





    

    function _showMap(stops) {
       
        if (stops && stops.length > 0) {
            var mapStops = _.map(stops, function (item) {
                return {
                    lat: item.latitude,
                    long: item.longitude,
                    info: item.name
                };
            });
          
            var currStop;
            if (stops.length == 0) {
                currStop = 0;
            } else {
                currStop = stops.length - 1;
            }
            //ShowMap
            
            travelMap.createMap({
                stops: mapStops,
                selector: "#map",
                currentStop: currStop,
                initialZoom: 3
            });
            
        } 
    }
})();