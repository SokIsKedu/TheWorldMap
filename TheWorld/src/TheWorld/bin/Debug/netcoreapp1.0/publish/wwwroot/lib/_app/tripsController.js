!function(){"use strict";function i(i){var r=this;r.trips=[],r.newTrip={},r.name="lopas",r.errorMessage="",r.isBusy=!0,i.get("/api/trips").then(function(i){angular.copy(i.data,r.trips)},function(i){r.errorMessage="Failed to load data: "+i}).finally(function(){r.isBusy=!1}),r.addTrip=function(){r.errorMessage="",r.isBusy=!0,i.post("/api/trips",r.newTrip).then(function(i){r.trips.push(i.data),r.newTrip={}},function(){r.errorMessage="Failed to save new trip"}).finally(function(){r.isBusy=!1})}}angular.module("app-trips").controller("tripsController",i)}();