document.getElementById('salaryForm').addEventListener('submit', async function (e) {
    e.preventDefault();

    const salaryData = {
        employeeId: document.getElementById('employeeId').value,
        basicSalary: parseFloat(document.getElementById('basicSalary').value),
        conveyance: parseFloat(document.getElementById('conveyance').value),
        tax: parseFloat(document.getElementById('tax').value),
        pf: parseFloat(document.getElementById('pf').value),
        esi: parseFloat(document.getElementById('esi').value),
        paymentMethod: document.getElementById('paymentMode').value
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
