window.drawChart = (labels, data) => {
    const ctx = document.getElementById('salesChart');
    if (!ctx) return;

    let chartStatus = Chart.getChart(ctx);
    if (chartStatus) {
        chartStatus.destroy();
    }

    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Продажи',
                data: data,
                backgroundColor: 'rgba(54, 162, 235, 0.5)',
                borderColor: 'rgba(54, 162, 235, 1)',
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false
        }
    });
};