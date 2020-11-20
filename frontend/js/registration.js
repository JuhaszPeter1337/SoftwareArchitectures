var check = function() {
    if (document.getElementById('mypassword').value == document.getElementById('mypassword_again').value) {
      document.getElementById('message').style.color = 'green';
      document.getElementById('message').innerHTML = 'Password is matching';
      document.getElementById("next_button").disabled = false;
    } else {
      document.getElementById('message').style.color = 'red';
      document.getElementById('message').innerHTML = 'Password is not matching';
      document.getElementById("next_button").disabled = true;
    }
  }

function write_to_console() {
    var usernameValue = document.getElementById("uniqueusername").value;
    var passwordValue = document.getElementById("mypassword").value;
    var password_againValue = document.getElementById("mypassword_again").value;

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
  
    console.log("Username: ", usernameValue, "\n" ,  "Password: ", passwordValue, "\n", "Password again: ", password_againValue);
    console.log("Watching Sport: ", interests1, "\n" ,  "Playing sport: ", interests2, "\n", "Cinema: ", interests3);      
}
