window.renderParallelCoordinates = function (jsonData) {

    if (!jsonData) return;

    const data = JSON.parse(jsonData);


    if (data.length === 0) {
        d3.select("#parallelChart").selectAll("*").remove();
        return;
    }

    d3.select("#parallelChart").selectAll("*").remove();

    const svg = d3.select("#parallelChart");

    const containerNode = svg.node();
    if (!containerNode) return;

    const containerWidth = containerNode.getBoundingClientRect().width;
    const calculatedWidth = containerWidth > 0 ? containerWidth : 800;

    const margin = { top: 50, right: 50, bottom: 50, left: 50 };
    const width = calculatedWidth - margin.left - margin.right;
    const height = 500 - margin.top - margin.bottom;

    const dimensions = [
        { key: "TotalScore", label: "Toplam Puan" },
        { key: "CorrectRate", label: "Doğru %" },
        { key: "WrongRate", label: "Yanlış %" },
        { key: "BlankRate", label: "Boş %" },
        { key: "PartialRate", label: "Kısmi %" }
    ];

    // Create scales for each dimension
    const y = {};
    dimensions.forEach(dim => {
        y[dim.key] = d3.scaleLinear()
            .domain(d3.extent(data, d => +d[dim.key]))
            .range([height, 0]);
    });

    const x = d3.scalePoint()
        .range([0, width])
        .padding(1)
        .domain(dimensions.map(d => d.key));

    // Color scale
    const color = d3.scaleSequential(d3.interpolateViridis)
        .domain(d3.extent(data, d => +d.TotalScore));

    const g = svg.append("g")
        .attr("transform", `translate(${margin.left},${margin.top})`);

    // Draw background lines
    g.append("g")
        .attr("class", "background")
        .selectAll("path")
        .data(data)
        .enter().append("path")
        .attr("d", path)
        .style("fill", "none")
        .style("stroke", "#ddd")
        .style("opacity", 0.2);

    // Draw foreground lines
    g.append("g")
        .attr("class", "foreground")
        .selectAll("path")
        .data(data)
        .enter().append("path")
        .attr("d", path)
        .style("fill", "none")
        .style("stroke", d => color(d.TotalScore))
        .style("stroke-width", 2)
        .style("opacity", 0.6)
        .on("mouseover", function (event, d) {
            d3.select(this)
                .style("stroke-width", 4)
                .style("opacity", 1)
                .raise(); 

            showTooltip(event, d);
        })
        .on("mouseout", function () {
            d3.select(this)
                .style("stroke-width", 2)
                .style("opacity", 0.6);

            hideTooltip();
        });

    // Draw axes
    const axes = g.selectAll(".axis")
        .data(dimensions)
        .enter().append("g")
        .attr("class", "axis")
        .attr("transform", d => `translate(${x(d.key)},0)`)
        .each(function (d) {
            d3.select(this).call(
                d3.axisLeft()
                    .scale(y[d.key])
                    .ticks(5)
            );
        });

    // Add axis labels
    axes.append("text")
        .style("text-anchor", "middle")
        .attr("y", -9)
        .text(d => d.label)
        .style("fill", "black")
        .style("font-weight", "bold");

    // Add title
    svg.append("text")
        .attr("x", width / 2)
        .attr("y", -20)
        .attr("text-anchor", "middle")
        .style("font-size", "16px")
        .style("font-weight", "bold")
        .text("Öğrenci Performans Profilleri");

    // Path function
    function path(d) {
        return d3.line()(dimensions.map(dim => [x(dim.key), y[dim.key](d[dim.key])]));
    }

    // Tooltip functions
    function showTooltip(event, d) {
        hideTooltip();

        const tooltip = d3.select("body").append("div")
            .attr("class", "parallel-tooltip")
            .style("position", "absolute")
            .style("background", "rgba(255, 255, 255, 0.95)")
            .style("border", "1px solid #999")
            .style("border-radius", "4px")
            .style("padding", "10px")
            .style("pointer-events", "none")
            .style("box-shadow", "0 2px 5px rgba(0,0,0,0.2)")
            .style("font-size", "12px")
            .style("z-index", "10000");

        tooltip.html(`
            <strong>${d.StudentName}</strong><br/>
            <hr style="margin: 5px 0">
            Toplam Puan: <b>${d.TotalScore.toFixed(2)}</b><br/>
            Doğru: %${d.CorrectRate.toFixed(1)}<br/>
            Yanlış: %${d.WrongRate.toFixed(1)}<br/>
            Boş: %${d.BlankRate.toFixed(1)}<br/>
            Kısmi: %${d.PartialRate.toFixed(1)}
        `)
            .style("left", (event.pageX + 15) + "px")
            .style("top", (event.pageY - 15) + "px");
    }

    function hideTooltip() {
        d3.selectAll(".parallel-tooltip").remove();
    }
};