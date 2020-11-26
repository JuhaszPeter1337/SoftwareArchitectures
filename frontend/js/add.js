function ckChange(ckType){
    var ckName = document.getElementsByName(ckType.name);
    var checked = document.getElementById(ckType.id);

    if (checked.checked) {
      for(var i=0; i < ckName.length; i++){

          if(!ckName[i].checked){
              ckName[i].disabled = true;
          }else{
              ckName[i].disabled = false;
          }
      } 
    }
    else {
      for(var i=0; i < ckName.length; i++){
        ckName[i].disabled = false;
      } 
    }    
}

async function sendEvent(){
    var title = document.getElementById("titlei").value;
    var begin = document.getElementById("begini").value;
    var end = document.getElementById("endi").value;
    var description = document.getElementById("desi").value;

    var interests1 = document.getElementById("progress1").checked;
    var interests2 = document.getElementById("progress2").checked;
    var interests3 = document.getElementById("progress3").checked;
    var interests4 = document.getElementById("progress4").checked;
    var interests5 = document.getElementById("progress5").checked;
    var interests6 = document.getElementById("progress6").checked;

    var language1 = document.getElementById("l1").checked;
    var language2 = document.getElementById("l2").checked;
    var language3 = document.getElementById("l3").checked;
    var language4 = document.getElementById("l4").checked;
    var language5 = document.getElementById("l5").checked;
    var language6 = document.getElementById("l6").checked;

    var event = new Event();

    event.setTitle(title);
    event.setBegin(begin);
    event.setEnd(end);
    event.setDescription(description);

    event.setInterests(interests1, interests2, interests3, interests4, interests5, interests6);
    event.setLanguages(language1, language2, language3, language4, language5, language6);

    try {
        await connection.invoke("AddEvent", event);
    } catch (err) {
        console.error(err);
    }
}

