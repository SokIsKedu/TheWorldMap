!function(){"use strict";function t(t,e){var s=this;s.tripName=t.tripName,s.stops=[],s.errormessage="",s.isBusy=!0,s.show=!0,s.newStop={};var n="/api/trips/"+s.tripName+"/stops",r=n+"/delstop";s.reverse=!1,s.propertyName="name",s.sortBy=function(t){s.reverse=s.propertyName===t&&!s.reverse,s.propertyName=t},e.get(n).then(function(t){angular.copy(t.data,s.stops),o(s.stops)},function(t){s.errormessage="Failed tu load your trip stops"}).finally(function(){s.isBusy=!1}),s.addStop=function(){s.isBusy=!0,s.newStop.order=s.stops.length,e.post(n,s.newStop).then(function(t){s.stops.push(t.data),o(s.stops),s.newStop={},s.show=!0},function(t){s.errormessage="Failed to add new stop"}).finally(function(){s.isBusy=!1})},s.removeStop=function(t){s.isBusy=!0;var n=s.stops.indexOf(t);e.post(r,s.stops[n]).then(function(t){s.stops.splice(n,1),o(s.stops),0==s.stops.length&&(s.show=!1)},function(t){s.errormessage="Failed to remove stop",window.alert(s.errormessage)}).finally(function(){s.isBusy=!1})}}function o(t){if(t&&t.length>0){var o,e=_.map(t,function(t){return{lat:t.latitude,long:t.longitude,info:t.name}});o=0==t.length?0:t.length-1,travelMap.createMap({stops:e,selector:"#map",currentStop:o,initialZoom:3})}}angular.module("app-trips").controller("tripEditorController",t)}();