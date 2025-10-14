<template>
  <Layout>
    <div class="container mt-5">
      <div class="card shadow-sm">
        <div class="card-header text-white text-center">
          <h4 class="mb-0 " >Enter Employee Salary</h4>
        </div>
        <div class="card-body">

          <div v-if="alert.message" :class="`alert alert-${alert.type} alert-dismissible fade show`" role="alert">
            {{ alert.message }}
            <button type="button" class="btn-close" @click="alert.message = ''"></button>
          </div>

          <form @submit.prevent="submitSalary">
            <div class="row g-3 mb-3">
              <div class="col-md-6">
                <label for="employeeId" class="form-label">Employee</label>
                <select v-model="form.employeeId" class="form-select" required>
                  <option value="">Select Employee</option>
                  <option v-for="emp in employees" :key="emp.id" :value="emp.id">
                    {{ emp.empName }}
                  </option>
                </select>
              </div>

              <div class="col-md-6">
                <label class="form-label">Payment Mode</label>
                <select v-model="form.paymentMethod" class="form-select" required>
                  <option value="">Select Payment Mode</option>
                  <option v-for="mode in paymentModes" :key="mode" :value="mode">
                    {{ mode }}
                  </option>
                </select>
              </div>
            </div>

            <div class="row g-3 mb-3">
              <div class="col-md-4">
                <label class="form-label">Basic Salary</label>
                <input v-model.number="form.basicSalary" type="number" class="form-control" required />
              </div>
              <div class="col-md-4">
                <label class="form-label">Conveyance</label>
                <input v-model.number="form.conveyance" type="number" class="form-control" required />
              </div>
              <div class="col-md-4">
                <label class="form-label">Salary Status</label>
                <select v-model="form.status" class="form-select">
                  <option value="Unpaid">Unpaid</option>
                  <option value="Paid">Paid</option>
                </select>
              </div>
            </div>

            <div class="row g-3 mb-3">
              <div class="col-md-4">
                <label class="form-label">Tax</label>
                <input v-model.number="form.tax" type="number" class="form-control" />
              </div>
              <div class="col-md-4">
                <label class="form-label">PF</label>
                <input v-model.number="form.pf" type="number" class="form-control" />
              </div>
              <div class="col-md-4">
                <label class="form-label">ESI</label>
                <input v-model.number="form.esi" type="number" class="form-control" />
              </div>
            </div>

            <!-- Live Calculations -->
            <div class="p-3 mb-3 rounded" style="background: #f8f9fa; border-left: 4px solid #0d6efd;">
              <p class="mb-1"><strong>Gross Salary:</strong> <span class="text-success">{{ grossSalary.toFixed(2) }}</span></p>
              <p class="mb-0"><strong>Net Salary:</strong> <span class="text-primary">{{ netSalary.toFixed(2) }}</span></p>
            </div>

            <button type="submit" class="btn btn-primary w-100" :disabled="loading">
              {{ loading ? 'Submitting...' : 'Submit Salary' }}
            </button>
          </form>
        </div>
      </div>
    </div>
  </Layout>
</template>

<script setup>
  import { ref, computed, onMounted } from 'vue'
  import Layout from "../components/Layout.vue"
  import { getAllEmployees } from "../services/employeeService"
  import { getPaymentModes, addSalary } from "../services/salaryService.js";

  const employees = ref([])
  const paymentModes = ref([])
  const loading = ref(false)
  const alert = ref({ message: '', type: '' })

  const form = ref({
    employeeId: '',
    basicSalary: 0,
    conveyance: 0,
    tax: 0,
    pf: 0,
    esi: 0,
    paymentMethod: '',
    status: 'Unpaid'
  })

  const grossSalary = computed(() => form.value.basicSalary + form.value.conveyance)
  const netSalary = computed(() => form.value.basicSalary - (form.value.tax + form.value.pf + form.value.esi))

  async function loadEmployees() {
    try {
      const res = await getAllEmployees(1, 100, null, null, null, null, null, "CreatedAt", true)
      employees.value = res.data.employees
    } catch (error) {
      console.error('Error loading employees:', error)
    }
  }

  async function loadPaymentModes() {
    try {
      const res = await getPaymentModes();
      paymentModes.value = res.data;
    } catch (error) {
      console.error('Error loading payment modes:', error);
    }
  }

  async function submitSalary() {
    alert.value = { message: '', type: '' };
    loading.value = true;

    try {
      const response = await addSalary(form.value, { responseType: 'blob' });
      const pdfBlob = new Blob([response.data], { type: 'application/pdf' });
      const pdfUrl = URL.createObjectURL(pdfBlob);
      const link = document.createElement('a');
      link.href = pdfUrl;
      link.download = 'SalarySlip.pdf';
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      alert.value = { message: 'Salary added and PDF downloaded!', type: 'success' };
      form.value = {
        employeeId: '',
        basicSalary: 0,
        conveyance: 0,
        tax: 0,
        pf: 0,
        esi: 0,
        paymentMethod: '',
        status: 'Unpaid'
      };
    } catch (error) {
      alert.value = { message: 'Error adding salary: ' + error.message, type: 'danger' };
    } finally {
      loading.value = false;
    }
  }

  onMounted(() => {
    loadEmployees()
    loadPaymentModes()
  })
</script>

<style scoped>
  .container {
    max-width: 700px;
  }

  .card-header {
    font-size: 1.2rem;
    background: linear-gradient(135deg, #0d6efd, #6f42c1);
  }

  .form-label {
    font-weight: 500;
  }

  input:focus, select:focus {
    box-shadow: 0 0 5px rgba(13, 110, 253, 0.5);
  }
</style>
