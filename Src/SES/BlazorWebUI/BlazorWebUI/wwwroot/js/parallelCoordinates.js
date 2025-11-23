window.renderParallelCoordinates = function (jsonData) {
    console.log("🔍 renderParallelCoordinates called");

    if (typeof d3 === 'undefined') {
        console.error("❌ D3.js is not loaded!");
        return;
    }

    if (!jsonData) {
        console.error("❌ No jsonData provided");
        return;
    }

    const data = JSON.parse(jsonData);
    console.log("📊 Data parsed:", data.length, "students");

    if (data.length === 0) {
        d3.select("#parallelChart").selectAll("*").remove();
        console.warn("⚠️ Empty data");
        return;
    }

    d3.select("#parallelChart").selectAll("*").remove();

    const svg = d3.select("#parallelChart");
    const containerNode = svg.node();
    if (!containerNode) {
        console.error("❌ SVG node not found");
        return;
    }

    const containerWidth = containerNode.getBoundingClientRect().width;
    const calculatedWidth = containerWidth > 0 ? containerWidth : 800;

    const margin = { top: 50, right: 50, bottom: 50, left: 50 };
    const width = calculatedWidth - margin.left - margin.right;
    const height = 500 - margin.top - margin.bottom;

    console.log("📐 Dimensions:", { width, height });

    const dimensions = [
        { key: "TotalScore", label: "Toplam Puan" },
        { key: "CorrectRate", label: "Doğru %" },
        { key: "WrongRate", label: "Yanlış %" },
        { key: "BlankRate", label: "Boş %" },
        { key: "PartialRate", label: "Kısmi %" }
    ];

    const y = {};
    dimensions.forEach(dim => {
        const extent = d3.extent(data, d => +d[dim.key]);
        y[dim.key] = d3.scaleLinear()
            .domain(extent)
            .range([height, 0]);
    });

    const x = d3.scalePoint()
        .range([0, width])
        .padding(1)
        .domain(dimensions.map(d => d.key));

    const color = d3.scaleSequential(d3.interpolateViridis)
        .domain(d3.extent(data, d => +d.TotalScore));

    const g = svg.append("g")
        .attr("transform", `translate(${margin.left},${margin.top})`);

    function path(d) {
        return d3.line()(dimensions.map(dim => [x(dim.key), y[dim.key](d[dim.key])]));
    }

    data.forEach(student => {
        student._pathString = path(student);
    });

    const groupedData = d3.group(data, d => d._pathString);
    console.log("👥 Grouped profiles:", groupedData.size);

    // Background
    g.append("g")
        .attr("class", "background")
        .selectAll("path")
        .data(data)
        .enter().append("path")
        .attr("d", d => d._pathString)
        .style("fill", "none")
        .style("stroke", "#ddd")
        .style("opacity", 0.2);

    const profileGroups = Array.from(groupedData.values());

    let pinnedTooltip = null;
    let pinnedGroup = null;

    // Foreground
    g.append("g")
        .attr("class", "foreground")
        .selectAll("path")
        .data(profileGroups)
        .enter().append("path")
        .attr("d", d => d[0]._pathString)
        .style("fill", "none")
        .style("stroke", d => color(d[0].TotalScore))
        .style("stroke-width", d => Math.min(8, Math.max(2, d.length * 0.5)))
        .style("opacity", 0.6)
        .style("cursor", "pointer")
        .on("mouseover", function (event, groupStudents) {

            if (pinnedTooltip) return;

            d3.select(this)
                .style("stroke-width", Math.min(12, Math.max(4, groupStudents.length)))
                .style("opacity", 1)
                .raise();

            showGroupTooltip(event, groupStudents, false);
        })
        .on("mouseout", function (event, groupStudents) {

            if (pinnedTooltip) return;

            d3.select(this)
                .style("stroke-width", Math.min(8, Math.max(2, groupStudents.length * 0.5)))
                .style("opacity", 0.6);

            hideTooltip();
        })
        .on("click", function (event, groupStudents) {
            event.stopPropagation();

            if (pinnedGroup === groupStudents) {
                unpinTooltip();
                d3.select(this)
                    .style("stroke-width", Math.min(8, Math.max(2, groupStudents.length * 0.5)))
                    .style("opacity", 0.6);
                pinnedGroup = null;
            } else {
                unpinTooltip();

                d3.select(this)
                    .style("stroke-width", Math.min(12, Math.max(4, groupStudents.length)))
                    .style("opacity", 1)
                    .raise();

                showGroupTooltip(event, groupStudents, true);
                pinnedGroup = groupStudents;
            }
        });

    // Axes
    const axes = g.selectAll(".axis")
        .data(dimensions)
        .enter().append("g")
        .attr("class", "axis")
        .attr("transform", d => `translate(${x(d.key)},0)`)
        .each(function (d) {
            d3.select(this).call(d3.axisLeft().scale(y[d.key]).ticks(5));
        });

    axes.append("text")
        .style("text-anchor", "middle")
        .attr("y", -9)
        .text(d => d.label)
        .style("fill", "black")
        .style("font-weight", "bold");

    svg.append("text")
        .attr("x", width / 2 + margin.left)
        .attr("y", 20)
        .attr("text-anchor", "middle")
        .style("font-size", "16px")
        .style("font-weight", "bold")
        .text(`Öğrenci Performans Profilleri (${data.length} öğrenci, ${profileGroups.length} farklı profil)`);

    svg.on("click", function () {
        unpinTooltip();
    });

    function showGroupTooltip(event, groupStudents, isPinned) {
        if (!isPinned) {
            hideTooltip();
        }

        const tooltip = d3.select("body").append("div")
            .attr("class", isPinned ? "parallel-tooltip pinned" : "parallel-tooltip")
            .style("position", "absolute")
            .style("background", "rgba(255, 255, 255, 0.98)")
            .style("border", isPinned ? "3px solid #007bff" : "2px solid #333")
            .style("border-radius", "8px")
            .style("padding", "15px")
            .style("pointer-events", isPinned ? "auto" : "none")
            .style("box-shadow", "0 4px 12px rgba(0,0,0,0.3)")
            .style("font-size", "13px")
            .style("z-index", "10000")
            .style("max-width", "400px")
            .style("max-height", "500px")
            .style("overflow-y", "auto");

        if (isPinned) {
            pinnedTooltip = tooltip;
        }

        const firstStudent = groupStudents[0];

        let html = `
            <div style="font-weight: bold; font-size: 15px; margin-bottom: 10px; color: #007bff; border-bottom: 2px solid #007bff; padding-bottom: 5px; display: flex; justify-content: space-between; align-items: center;">
                <span>📊 Bu Profildeki Öğrenciler: ${groupStudents.length}</span>
                ${isPinned ? '<button onclick="this.closest(\'.parallel-tooltip\').remove()" style="background: #dc3545; color: white; border: none; border-radius: 4px; padding: 4px 8px; cursor: pointer; font-size: 12px;">✖ Kapat</button>' : ''}
            </div>
            
            <div style="background: #e3f2fd; padding: 10px; border-radius: 6px; margin-bottom: 10px;">
                <strong style="color: #1976d2;">Profil Değerleri:</strong><br/>
                <table style="width: 100%; margin-top: 5px; font-size: 12px;">
                    <tr><td>Toplam Puan:</td><td><b>${firstStudent.TotalScore.toFixed(2)}</b></td></tr>
                    <tr><td>Doğru Oranı:</td><td><b>%${firstStudent.CorrectRate.toFixed(1)}</b></td></tr>
                    <tr><td>Yanlış Oranı:</td><td><b>%${firstStudent.WrongRate.toFixed(1)}</b></td></tr>
                    <tr><td>Boş Oranı:</td><td><b>%${firstStudent.BlankRate.toFixed(1)}</b></td></tr>
                    <tr><td>Kısmi Doğru:</td><td><b>%${firstStudent.PartialRate.toFixed(1)}</b></td></tr>
                </table>
            </div>
            
            <div style="margin-top: 10px;">
                <strong style="color: #333; display: block; margin-bottom: 8px;">Öğrenci Listesi:</strong>
                <div style="max-height: 250px; overflow-y: auto; border: 1px solid #ddd; border-radius: 4px; padding: 5px;">
        `;

        groupStudents.forEach((student, index) => {
            const bgColor = index % 2 === 0 ? '#ffffff' : '#f8f9fa';
            html += `
                <div style="padding: 6px 8px; background: ${bgColor}; margin: 2px 0; border-radius: 3px; font-size: 12px;">
                    <strong style="color: #1976d2;">${index + 1}.</strong> ${student.StudentName}
                </div>
            `;
        });

        html += `</div></div>`;

        tooltip.html(html)
            .style("left", (event.pageX + 15) + "px")
            .style("top", (event.pageY - 15) + "px");
    }

    function hideTooltip() {
        d3.selectAll(".parallel-tooltip:not(.pinned)").remove();
    }

    function unpinTooltip() {
        if (pinnedTooltip) {
            pinnedTooltip.remove();
            pinnedTooltip = null;
        }
    }

    console.log("✅ Chart rendered successfully");
};