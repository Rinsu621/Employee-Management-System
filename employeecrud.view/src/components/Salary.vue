<template>
  <Layout>

    <div class="container mt-5 text-center">
      <button class="btn btn-primary" @click="showModal = true">Add Salary</button>
    </div>

    <div v-if="showModal" class="modal-overlay" @click.self="closeModal">
      <div class="modal-content">
        <button class="close-btn" @click="closeModal">×</button>
          <div class="card shadow-sm">
            <div class="card-header text-white text-center">
              <h4 class="mb-0 ">Enter Employee Salary</h4>
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
                <div class="row g-3 mb-3">
                  <div class="col-md-6">
                    <label class="form-label">Salary Month</label>
                    <input v-model="form.salaryDate" type="month" class="form-control" required />
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
  const showModal = ref(false)

  const form = ref({
    employeeId: '',
    basicSalary: 0,
    conveyance: 0,
    tax: 0,
    pf: 0,
    esi: 0,
    paymentMethod: '',
    status: 'Unpaid',
    salaryDate: ''
  })

  const grossSalary = computed(() => form.value.basicSalary + form.value.conveyance)
  const netSalary = computed(() => (form.value.basicSalary + form.value.conveyance) - (form.value.tax + form.value.pf + form.value.esi))

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

  function closeModal() {
  showModal.value = false
}

  async function submitSalary() {
    alert.value = { message: '', type: '' };
    loading.value = true;

    try {
      const payload = {
        ...form.value,
        salaryDate: new Date(form.value.salaryDate + '-01') // first day of selected month
      };
      const response = await addSalary(payload, { responseType: 'blob' });
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
        status: 'Unpaid',
        salaryDate: ''
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
/*  .container {
    max-width: 700px;
  }*/

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


  .modal-overlay {
    position: fixed;
    inset: 0;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 1050;
  }

  .modal-content {
    background: white;
    width: 100%;
    max-width: 700px;
    border-radius: 10px;
    padding: 20px;
    position: relative;
    overflow-y: auto;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
    animation: fadeInUp 0.3s ease;
  }

  .close-btn {
    position: absolute;
    top: 15px;
    right: 20px;
    border: none;
    background: transparent;
    font-size: 28px;
    font-weight: bold;
    color: #fff; /* change to #fff if your header background is dark */
    cursor: pointer;
    z-index: 9999; /* ensures it's above everything inside modal */
    transition: color 0.2s ease;
  }



  @keyframes fadeInUp {
    from {
      transform: translateY(20px);
      opacity: 0;
    }

    to {
      transform: translateY(0);
      opacity: 1;
    }
  }
</style>
