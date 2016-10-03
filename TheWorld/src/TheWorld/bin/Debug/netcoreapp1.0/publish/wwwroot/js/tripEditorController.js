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
        vm.newStop = {};
        var apiUrl = "/api/trips/" + vm.tripName + "/stops";
        // NEVEIKIA DAR IDEJIMAS I STOPUS
        //
        //
        //
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
            $http.post(apiUrl, vm.newStop)
            .then(function (response) {
                vm.stops.push(response.data);
                _showMap(vm.stops);
                vm.newStop = {};
            }, function (err) {
                vm.errormessage = "Failed to add new stop";
            })
            .finally(function () {
                vm.isBusy = false;
            });

        };
    }

    function _showMap(stops) {

        if (stops && stops.length > 0) {
            //var stops = [{
            //    lat: 33.748995,
            //    long: -84.387982,
            //    info: "Atlanta, GA"
            //}, {
            //    lat: 51.050409,
            //    long: 13.737262,
            //    info: "Dresden, Germany"
            //}]
            //travelMap.createMap({
            //    stops: stops,
            //    selector: "#map",
            //    currentStop: 1,
            //    initialZoom: 3
            //});
            var mapStops = _.map(stops, function (item) {
                return {
                    lat: item.latitude,
                    long: item.longitude,
                    info: item.name
                };
            });
            //ShowMap
            travelMap.createMap({
                stops: mapStops,
                selector: "#map",
                currentStop: stops.length -1,
                initialZoom:3
            });
        }
    }

})();