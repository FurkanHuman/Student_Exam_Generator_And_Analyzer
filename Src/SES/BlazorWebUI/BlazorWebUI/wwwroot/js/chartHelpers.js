window.renderCustomBoxPlot = function (elementId, seriesDataJson, title) {
    try {
        if (typeof ApexCharts === 'undefined') {
            console.error("ApexCharts kütüphanesi bulunamadı! Lütfen script tag'ini ekleyin.");
            return;
        }

        var element = document.querySelector("#" + elementId);
        if (!element) {
            return;
        }

        if (element.chartInstance) {
            element.chartInstance.destroy();
        }

        var data = JSON.parse(seriesDataJson);

        var options = {
            series: [{
                type: 'boxPlot',
                data: data
            }],
            chart: {
                type: 'boxPlot',
                height: 400,
                toolbar: { show: false }
            },
            title: {
                text: title,
                align: 'left'
            },
            plotOptions: {
                boxPlot: {
                    colors: {
                        upper: '#5C4742',
                        lower: '#A5978B'
                    }
                }
            }
        };

        var chart = new ApexCharts(element, options);
        chart.render();

        element.chartInstance = chart;

    } catch (error) {
        console.error("BoxPlot çizilirken hata oluştu:", error);
    }
};