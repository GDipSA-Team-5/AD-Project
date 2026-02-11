const fs = require('fs');

// Load ZAP metadata
const zapData = JSON.parse(fs.readFileSync('scan-data/zap-metadata.json', 'utf8'));

// Load historical data
let historicalData = [];
const historyFile = 'dashboard/history.json';

if (fs.existsSync(historyFile)) {
  historicalData = JSON.parse(fs.readFileSync(historyFile, 'utf8'));
}

// Add current scan to history
const currentScan = {
  timestamp: zapData.timestamp,
  run_id: zapData.run_id,
  branch: zapData.branch,
  commit: zapData.commit.substring(0, 7),
  zap: zapData.zap
};

historicalData.push(currentScan);

// Keep last 30 scans
if (historicalData.length > 30) {
  historicalData = historicalData.slice(-30);
}

fs.writeFileSync(historyFile, JSON.stringify(historicalData, null, 2));

// Generate HTML
const html = `
<!DOCTYPE html>
<html>
<head>
  <title>ZAP Security Dashboard</title>
  <script src="https://cdn.jsdelivr.net/npm/chart.js@4.4.1/dist/chart.umd.min.js"></script>
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="bg-light">

<div class="container py-5">

  <h2 class="mb-4">OWASP ZAP Aggregated Security Dashboard</h2>

  <div class="card p-4 mb-4">
    <h5>Latest Scan</h5>
    <p>
      Branch: <strong>${currentScan.branch}</strong><br>
      Commit: <strong>${currentScan.commit}</strong><br>
      Date: <strong>${new Date(currentScan.timestamp).toLocaleString()}</strong>
    </p>
  </div>

  <div class="row text-center mb-4">
    <div class="col-md-3">
      <div class="card p-3">
        <h3 class="text-danger">${currentScan.zap.high}</h3>
        <p>High</p>
      </div>
    </div>
    <div class="col-md-3">
      <div class="card p-3">
        <h3 class="text-warning">${currentScan.zap.medium}</h3>
        <p>Medium</p>
      </div>
    </div>
    <div class="col-md-3">
      <div class="card p-3">
        <h3 class="text-info">${currentScan.zap.low}</h3>
        <p>Low</p>
      </div>
    </div>
    <div class="col-md-3">
      <div class="card p-3">
        <h3>${currentScan.zap.total}</h3>
        <p>Total</p>
      </div>
    </div>
  </div>

  <div class="card p-4 mb-4">
    <h5>ZAP Alerts Trend</h5>
    <canvas id="trendChart"></canvas>
  </div>

  <div class="card p-4">
    <h5>Latest Breakdown</h5>
    <canvas id="pieChart"></canvas>
  </div>

</div>

<script>
const history = ${JSON.stringify(historicalData)};

const labels = history.map(h => h.commit);
const totals = history.map(h => parseInt(h.zap.total));

new Chart(document.getElementById('trendChart'), {
  type: 'line',
  data: {
    labels: labels,
    datasets: [{
      label: 'Total Alerts',
      data: totals,
      borderColor: '#0d6efd',
      tension: 0.3
    }]
  },
  options: {
    responsive: true,
    scales: { y: { beginAtZero: true } }
  }
});

new Chart(document.getElementById('pieChart'), {
  type: 'doughnut',
  data: {
    labels: ['High', 'Medium', 'Low'],
    datasets: [{
      data: [
        ${currentScan.zap.high},
        ${currentScan.zap.medium},
        ${currentScan.zap.low}
      ],
      backgroundColor: ['#dc3545', '#ffc107', '#0dcaf0']
    }]
  },
  options: { responsive: true }
});
</script>

</body>
</html>
`;

fs.writeFileSync('dashboard/index.html', html);
console.log("ZAP-only dashboard generated.");