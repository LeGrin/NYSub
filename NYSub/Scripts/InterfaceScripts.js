var mapURL="";
function OnLoad()
{
    var stations = [];
    for (var key in data) {
        if (data.hasOwnProperty(key)) {
            stations.push(key);
        }}
    $( "#lookups1" ).autocomplete( {source: stations} );
    $( "#lookups2" ).autocomplete( {source: stations} );
}
function CalculateDistance()
{
    var st1 = document.getElementById("lookups1").value;
    var st2 = document.getElementById("lookups2").value;
    if (st1.length > 2 && st2.length >2)
    {
        document.getElementById("distance").innerHTML = "is " +
            getDistanceFromLatLonInKm(data[st1],data[st2]) + " KM";
        document.getElementById("map").hidden = "";
        mapURL = "https://www.google.com/maps/embed/v1/directions?origin=" + data[st1][0] + "," + data[st1][1]
            + "&destination=" + data[st2][0] + "," + data[st2][1] 
            + "&mode=transit&units=metric&key=AIzaSyAN0om9mFmy1QN6Wf54tXAowK4eT0ZUPrU";
        if(decodeURI(document.getElementById("mapframe").src) != mapURL) document.getElementById("mapframe").src = mapURL;
    }
}
window.setInterval(CalculateDistance, 50);