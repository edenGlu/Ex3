// show a point on the scrin of the location of the plan
function showPoint(ctx, lon, lat)
{
    ctx.canvas.width = window.innerWidth;
    ctx.canvas.height = window.innerHeight;
    // Normalizes values to screen size
    var y = (window.innerHeight / 180) * (parseInt(lat) + 90);
    var x = (window.innerWidth / 360) * (parseInt(lon) + 180);
    ctx.fillStyle = "red"; //set the circle color to red
    ctx.beginPath();
    ctx.arc(x, y, 5, 0, 2 * Math.PI); // make the point
    ctx.fill();
    ctx.stroke();
}
// make continue line of the roude of the plan
function makeLine(ctx, xml)
{
    // open the xml
    var xmlDoc = $.parseXML(xml),
        $xml = $(xmlDoc),
        lon = $xml.find("Lon").text();
    lat = $xml.find("Lat").text();
    // Normalizes values to screen size
    var x = (window.innerWidth / 360) * (parseInt(lon) + 180);
    var y = (window.innerHeight / 180) * (parseInt(lat) + 90);
    // make the line
    ctx.lineTo(x, y);
    ctx.stroke();
}