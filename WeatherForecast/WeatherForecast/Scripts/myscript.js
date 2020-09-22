

/////chart javascript code ////////

//var ctx = document.getElementById('myChart');
//var myChart = new Chart(ctx, {
//    type: 'line',
//    data: {
//        labels: ['0','1', '2', '3', '4', '5', '6','7','8','9','10','11','12','13','14','15','16'],
//        datasets: [{
//            label: 'Temparature of days',
//            data: [10, 19, 30, 15, 20, 35,28,25,38,18,20,19,21,24,27,29,20],
//            backgroundColor: 'rgb(153 135 225 / 72%)',
//            borderColor:'rgba(255, 99, 132, 1)',
//            borderWidth: .5,
//            fill:true,
//            lineTension:0.1,
//            borderCapStyle:'butt',
//            borderJoinStyle:'miter',
//            pointBorderWidth:1,
//            poinyRadius:1,
//            pointHitRadius:10,
//        }]
//    },
//    options: {
//        scales: {
//            yAxes: [{
//                ticks: {
//                    beginAtZero: true
//                }
//            }]
//        }
//    }
//});


var today = new Date()
var date = today.getFullYear() + "-" + today.getDate() + "-" + today.getMonth(); 


//canvas js
var today1 =  new Date();

var datax = [
    { x: new Date(), y: 32 },
    { x: new Date(today1.setDate(today.getDate()+ 1)), y: 38 },
    { x: new Date(today1.setDate(today.getDate() + 2)), y: 20 },
    { x: new Date(today1.setDate(today.getDate() + 3)), y: 28 },
    { x: new Date(today1.setDate(today.getDate() + 4)), y: 23 },
    { x: new Date(today1.setDate(today.getDate() + 5)), y: 27 },
    { x: new Date(today1.setDate(today.getDate() + 6)), y: 28 },
    { x: new Date(today1.setDate(today.getDate() + 7)), y: 28 },
];

console.log(datax);

datax.forEach((d) => {
    console.log(d.x);
    d["indexLabel"] = String(d.y);
});

var chart = new CanvasJS.Chart("chartContainer", {
    animationEnabled: true,
    title: {
        text: "7 Days Weather Forecast"
    },

    axisY: {
        title: "Temperature in Celcius",
        valueFormatString: "#0",
        suffix: "",
    },
    axisX: {
        valueFormatString: "DDDD",
        minimum: datax[0].x,
        maximum: (datax[datax.length - 1].x)
    },
    data: [{
        xValueType: "dateTime",
        type: "splineArea",
        color: "rgba(153,135,225,.7)",
        markerSize: 5,
        dataPoints: datax
    }]
});
chart.render();
