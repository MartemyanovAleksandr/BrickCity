var myApp = function () {   
    am4core.useTheme(am4themes_animated);    

    function queryHouses() {
        return $.ajax({
            cache: false,
            url: "/Home/GetHouses"
        });
    };

    function queryPlants() {
        return $.ajax({
            cache: false,
            url: "/Home/GetPlants"
        });
    };

    function queryHouseConsupmption() {
        return $.ajax({
            cache: false,
            url: "/Home/GetHouseConsupmption"
        });
    };

    function queryPlantConsupmption() {
        return $.ajax({
            cache: false,
            url: "/Home/GetPlantConsupmption"
        });
    };

    document.addEventListener("DOMContentLoaded", function () {
        $.when(queryHouseConsupmption(), queryPlantConsupmption(), queryHouses(), queryPlants()).done(function (houseConsumptionData, plantConsumptionData, houseData, plantsData) {

            var houseConsumption = houseConsumptionData[0];
            var plantConsumption = plantConsumptionData[0];
            var houses = houseData[0].map(function (item) {
                return {
                    Id: item.Id,
                    Name: item.Name,
                    Type: "house"
                };
            });
            var plants = plantsData[0].map(function (item) {
                return {
                    Id: item.Id,
                    Name: item.Name,
                    Type: "plant"
                };
            });
            var consumers = houses.concat(plants);

            var stackedChart = function () {
                function createStackedStruct(data) {
                    var res = [];

                    data.forEach(function (item) {
                        var piePoint = res.filter(function (resItem) {
                            return resItem._date == item.Date;
                        })[0];

                        if (!piePoint) {
                            piePoint = {
                                _date: item.Date,
                                date: new Date(item.Date),
                            };
                            res.push(piePoint);
                        };

                        if (item.HouseId || item.HouseId === 0) {
                            piePoint["house_" + item.HouseId] = item.Value;
                            piePoint["weather"] = item.Weather;
                        };
                        if (item.PlantId || item.PlantId === 0) {
                            piePoint["plant_" + item.PlantId] = item.Value;
                            piePoint["price"] = item.Price;
                        };
                    });

                    res.forEach(function (item) {
                        var sum = 0;
                        for (var key in item) {
                            if (key.indexOf("date") < 0) sum += item[key];
                        };

                        item["sum_0"] = sum;
                    });

                    return res;
                };

                function createStackedSeries(data) {
                    var clone = JSON.parse(JSON.stringify(data));
                    clone.push({
                        Id: 0,
                        Name: "Всего",
                        Type: "sum"
                    });

                    return clone.map(function (consumer) {
                        return {
                            "type": "LineSeries",
                            "dataFields": {
                                "dateX": "date",
                                "valueY": consumer.Type + "_" + consumer.Id
                            },
                            "name": consumer.Name,
                            "tooltipHTML": "<span style='font-size:14px; color:#000000;'><b>{valueY.value}</b></span>",
                            "tooltip": {
                                "background": {
                                    //"fill": "#FFF",
                                    "strokeWidth": 3
                                },
                                "getStrokeFromObject": true,
                                "getFillFromObject": false
                            },
                            "fillOpacity": 0.6,
                            "strokeWidth": 2,
                            "stacked": true
                        };
                    });

                };

                var chart = am4core.createFromConfig({
                    "dateFormatter": {
                        //"inputDateFormat": "yyyy"
                        "inputDateFormat": "yyyy-MM-dd"
                    },
                    "yAxes": [{
                        "type": "ValueAxis",
                        "tooltip": {
                            "disabled": true
                        }
                    }],
                    "xAxes": [{
                        "type": "DateAxis",
                        "renderer": {
                            "minGridDistance": 50
                        },
                        //"startLocation": 0.5,
                        //"endLocation": 0.5,
                        //"baseInterval": {
                        //    "timeUnit": "year",
                        //    "count": 1
                        //},
                    }],
                    data: createStackedStruct(houseConsumption.concat(plantConsumption)),
                    series: createStackedSeries(consumers),
                    "cursor": {
                        "type": "XYCursor"
                    },
                    //"scrollbarX": {
                    //    "type": "Scrollbar"
                    //},
                    "legend": {
                        "position": "top"
                    }
                }, "stackedChart", am4charts.XYChart);
            }(); 

            var houseChart = function () {
                function generateChartData(data) {
                    var res = [];

                    var consumpterIdentifer = "HouseId";

                    data.forEach(function (consumption) {
                        var point = res.filter(function (item) {
                            return item.Date == consumption.Date && consumption.HouseId == item.ConsumerId;
                        })[0];

                        if (!point) {
                            point = {
                                ConsumerId: consumption.HouseId,
                                Date: consumption.Date,
                                date: new Date(consumption.Date)
                            };
                            res.push(point);
                        };

                        point.Weather = consumption.Weather;
                        point["Weather_" + consumption[consumpterIdentifer]] = consumption.Weather;
                        point["Consumption_" + consumption[consumpterIdentifer]] = consumption.Value;
                    });

                    res.sort(function (a, b) {
                        return (a.Weather > b.Weather) ? 1 : -1;
                    });

                    return res;
                };

                function addChartSeries(data) {
                    data.forEach(function (consumer) { 
                        var s = chart.series.push(new am4charts.LineSeries());
                        s.dataFields.valueX = "Weather_" + consumer.Id;
                        s.dataFields.valueY = "Consumption_" + consumer.Id;
                        s.strokeWidth = 3;
                        s.name = consumer.Name;
                        //s.tooltipText = "{name}: [bold]{valueY}[/]";                                          
                        //s.tooltipHTML = "<span style='font-size:14px; color:#000000;'><b>{Date}</b></span>";
                    });
                };

                var chart = am4core.create("houseChart", am4charts.XYChart);
                chart.data = generateChartData(houseConsumption);

                // Create axes
                var xAxis = chart.xAxes.push(new am4charts.ValueAxis());
                xAxis.title.text = "ТЕМПЕРАТУРА";
                xAxis.title.fontSize = 20;
              
                var yAxis = chart.yAxes.push(new am4charts.ValueAxis());               
                yAxis.title.text = "ПОТРЕБЛЕНИЕ";
                yAxis.title.fontSize = 20;
             
                addChartSeries(houses);
                chart.legend = new am4charts.Legend();
                chart.cursor = new am4charts.XYCursor();                
             
                // Scrollbars
                //chart.scrollbarX = new am4core.Scrollbar();
                //chart.scrollbarY = new am4core.Scrollbar();
            }();
            
            var plantsChart = function () {
                function generateChartData(data) {
                    var res = [];

                    var consumpterIdentifer = "PlantId";

                    data.forEach(function (consumption) {
                        var point = res.filter(function (item) {
                            return item.Date == consumption.Date && consumption[consumpterIdentifer] == item.ConsumerId;
                        })[0];

                        if (!point) {
                            point = {
                                ConsumerId: consumption[consumpterIdentifer],
                                Date: consumption.Date,
                                date: new Date(consumption.Date)
                            };
                            res.push(point);
                        };

                        point.Weather = consumption.Weather;
                        point["Price_" + consumption[consumpterIdentifer]] = consumption.Price;
                        point["Consumption_" + consumption[consumpterIdentifer]] = consumption.Value;
                    });

                    res.sort(function (a, b) {
                        return (a.Price > b.Price) ? 1 : -1;
                    });

                    return res;
                };

                function addChartSeries(data) {
                    data.forEach(function (consumer) { 
                        var s = chart.series.push(new am4charts.LineSeries());
                        s.dataFields.valueX = "Price_" + consumer.Id;
                        s.dataFields.valueY = "Consumption_" + consumer.Id;
                        s.strokeWidth = 3;
                        s.name = consumer.Name;
                        //s.tooltipText = "{name}: [bold]{valueY}[/]";                                          
                        //s.tooltipHTML = "<span style='font-size:14px; color:#000000;'><b>{Date}</b></span>";
                    });
                };

                var chart = am4core.create("plantChart", am4charts.XYChart);
                chart.data = generateChartData(plantConsumption);
                chart.colors.step = 10;

                // Create axes
                var xAxis = chart.xAxes.push(new am4charts.ValueAxis());
                xAxis.title.text = "ЦЕНА";
                xAxis.title.fontSize = 20;
              
                var yAxis = chart.yAxes.push(new am4charts.ValueAxis());               
                yAxis.title.text = "ПОТРЕБЛЕНИЕ";
                yAxis.title.fontSize = 20;

                addChartSeries(plants);
                chart.legend = new am4charts.Legend();
                chart.cursor = new am4charts.XYCursor();                
             
                // Scrollbars
                //chart.scrollbarX = new am4core.Scrollbar();
                //chart.scrollbarY = new am4core.Scrollbar();
            }();

        });
    });
}();