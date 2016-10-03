
(function () {
    var $sidebarAndWrapper = $("#sidebar,#wrapper");
    $("#sidebarToggle").on("click",function(){
        $sidebarAndWrapper.toggleClass("hide-sidebar");
        if ($sidebarAndWrapper.hasClass("hide-sidebar")) {
            $(this).html("Show Sidebar  <i class='fa fa-angle-right'></i>");
        } else {
            $(this).html("<i class='fa fa-angle-left'></i>  Hide Sidebar");
        }
    });
    
    //var toogleSidebar = 
})();