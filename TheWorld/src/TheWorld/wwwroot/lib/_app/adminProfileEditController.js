!function(){"use strict";function e(e,i,n,r){var o="/api/Admin/getUser/",l="/api/profile",t=this;t.userName=r.userName,t.isProfilePic=!0,e.get(o+t.userName).then(function(e){t.user=e.data}).finally(function(){t.isProfilePic=null!==t.user.profilePic}),i.saveUser=function(){e.post(l,t.user).then(function(e){}).finally(function(e){adminPanelController()})}}angular.module("app-admin").controller("profileEditorController",e)}();