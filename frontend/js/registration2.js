function write_to_console() {
    var interests1 = document.getElementById("interests1").checked;
    var interests2 = document.getElementById("interests2").checked;
    var interests3 = document.getElementById("interests3").checked;
    var interests4 = document.getElementById("interests4").checked;
    var interests5 = document.getElementById("interests5").checked;
    var interests6 = document.getElementById("interests6").checked;
    
    var language1 = document.getElementById("language1").checked;
    var language2 = document.getElementById("language2").checked;
    var language3 = document.getElementById("language3").checked;
    var language4 = document.getElementById("language4").checked;
    var language5 = document.getElementById("language5").checked;
    var language6 = document.getElementById("language6").checked;

    console.log("Watching Sport: ", interests1, "\n" ,  "Playing sport: ", interests2, "\n", "Cinema: ", interests3);     
}