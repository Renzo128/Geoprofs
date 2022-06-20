// Jquery code
/*$(document).ready(function () {
});*/
function ShowPopUp(element_id) {
    $("#" + element_id).toggleClass("show");
}

$(".img-icon, #nieuwe-aanvraag, .form-shadow").click(function () {
    $(".form-shadow, .create-request").toggleClass("hidden");
});

document.getElementById("AbsenceStart").addEventListener("change", function () {
    console.log("test");
    var firstdate = document.getElementById("AbsenceStart").value;
    document.getElementById("AbsenceEnd").value = "";
    document.getElementById("AbsenceEnd").setAttribute("min", firstdate);
});