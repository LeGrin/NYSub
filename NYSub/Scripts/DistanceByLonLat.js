function getDistanceFromLatLonInKm(latlon1, latlon2) {
    var R = 6371; // Radius of the earth in km
    var dLat = deg2rad(latlon2[0] - latlon1[0]);  // deg2rad below
    var dLon = deg2rad(latlon2[1] - latlon1[1]);
    var a =
      Math.sin(dLat / 2) * Math.sin(dLat / 2) +
      Math.cos(deg2rad(latlon1[0])) * Math.cos(deg2rad(latlon2[0])) *
      Math.sin(dLon / 2) * Math.sin(dLon / 2)
    ;
    var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    var d = R * c; // Distance in km
    return d.toPrecision(3);
}

function deg2rad(deg) {
    return deg * (Math.PI / 180)
}