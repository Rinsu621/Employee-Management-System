async function loadEmployees() {
    try {
        const response = await fetch('https://localhost:7070/api/employee/getallemployee');
        const employees = await response.json();

        const employeeSelect = document.getElementById('employeeId');
        employees.forEach(emp => {
            const option = document.createElement('option');
            option.value = emp.id;       // the EmployeeId to send to Salary API
            option.text = emp.empName;   // This is what the user sees
            employeeSelect.add(option);
        });
    }
    catch (error) {
        console.error("Error loading employee: ", error)
    }
}

async function loadPaymentMethod() {
    const paymentSelect = document.getElementById('paymentMode');
    paymentSelect.innerHTML = '<option value="">Select Payment Mode</option>';

    try {
        const response = await fetch('https://localhost:7070/api/salary/payment-mode');
        const payments = await response.json();
        console.log("Payments:", payments); //

        payments.forEach(pmt => {
            const option = document.createElement('option');
            option.value = pmt;
            option.text = pmt;
            paymentSelect.add(option);
        });
    }
    catch (error) {
        console.error("Error loading payment method:", error);
    }
}
window.addEventListener('DOMContentLoaded', () => {
    loadEmployees();       
    loadPaymentMethod();  
});

document.getElementById('salaryForm').addEventListener('submit', async function (e) {
    e.preventDefault();

    const salaryData = {
        employeeId: document.getElementById('employeeId').value,
        basicSalary: parseFloat(document.getElementById('basicSalary').value),
        conveyance: parseFloat(document.getElementById('conveyance').value),
        tax: parseFloat(document.getElementById('tax').value),
        pf: parseFloat(document.getElementById('pf').value),
        esi: parseFloat(document.getElementById('esi').value),
        paymentMethod: document.getElementById('paymentMode').value,
        status: document.getElementById('salaryStatus').value
    };

    try {
        const response = await fetch('https://localhost:7070/api/salary/add-salary', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(salaryData)
        });

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(errorText);
        }

        const result = await response.json();
        document.getElementById('result').innerHTML =
            `<div class="alert alert-success">Salary added successfully!</div>`;
    } catch (error) {
        console.error('API error:', error);
        document.getElementById('result').innerHTML =
            `<div class="alert alert-danger">Error adding salary: ${error.message}</div>`;
    }
});
