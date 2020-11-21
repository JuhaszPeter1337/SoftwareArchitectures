$(function () {
    var login = sessionStorage.getItem("login");
    if(login){
        var user = sessionStorage.getItem("user");
    }
    else{
        window.location.href = "/index.html";
    }
});