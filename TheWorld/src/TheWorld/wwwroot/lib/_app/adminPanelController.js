!function(){"use strict";function e(e,r,n,o){var s="/api/Admin",t=this;t.errorMessage="",t.isBusy=!0,t.users=[],t.reverse=!1,t.propertyName="name",t.sortBy=function(e){t.reverse=t.propertyName===e&&!t.reverse,t.propertyName=e},e.get(s).then(function(e){angular.copy(e.data,t.users)}).finally(function(){}),t.ask=function(e){o.showModal({templateUrl:"modal.html",controller:"yesNoModalController"}).then(function(r){r.element.modal(),r.close.then(function(r){"Yes"===r&&t.removeUser(e)})})},t.removeUser=function(r){t.errorMessage="",t.isBusy=!0;var n=t.users.indexOf(r);e.delete(s+"/delUser/"+r.userName).then(function(e){t.users.splice(n,1)},function(e){t.errormessage="Failed to remove trip",window.alert(t.errormessage)}).finally(function(){t.isBusy=!1})}}angular.module("app-admin").controller("adminPanelController",e),angular.module("app-admin").controller("yesNoModalController",function(e,r){e.close=function(e){r(e,500)}})}();