<template>
  <div class="container">
    <h2>Salary Slip - {{ salary.SalaryMonth }}</h2>
    <p>Hello <strong>{{ salary.EmployeeName }}</strong>,</p>
    <table>
      <tr>
        <th>Earnings</th>
        <th>Amount</th>
        <th>Deductions</th>
        <th>Amount</th>
      </tr>
      <tr>
        <td>Basic Salary</td>
        <td>{{ salary.BasicSalary.toFixed(2) }}</td>
        <td>Tax</td>
        <td>{{ salary.Tax.toFixed(2) }}</td>
      </tr>
      <tr>
        <td>Conveyance</td>
        <td>{{ salary.Conveyance.toFixed(2) }}</td>
        <td>PF</td>
        <td>{{ salary.PF.toFixed(2) }}</td>
      </tr>
      <tr>
        <td><b>Gross Salary</b></td>
        <td><b>{{ salary.GrossSalary.toFixed(2) }}</b></td>
        <td>ESI</td>
        <td>{{ salary.ESI.toFixed(2) }}</td>
      </tr>
      <tr>
        <td colspan="2"></td>
        <td><b>Net Salary</b></td>
        <td><b>{{ salary.NetSalary.toFixed(2) }}</b></td>
      </tr>
    </table>
    <p>Payment Mode: {{ salary.PaymentMode }}</p>
    <p>Status: {{ salary.Status }}</p>
    <div class="footer">This is an automated email, please do not reply.</div>
  </div>
</template>

<script setup>
defineProps({
  salary: Object
})
</script>

<style scoped>
.container {
  font-family: Arial, sans-serif;
  padding: 20px;
  border: 1px solid #ddd;
  background-color: #f9f9f9;
}
table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 10px;
}
th, td {
  border: 1px solid #ccc;
  padding: 8px;
  text-align: left;
}
th {
  background-color: #f2f2f2;
}
.footer {
  margin-top: 20px;
  font-size: 12px;
  color: #555;
}
</style>
