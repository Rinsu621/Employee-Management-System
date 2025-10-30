<template>
  <Layout>
    <div v-if="alert.message" :class="`alert alert-${alert.type} alert-dismissible fade show`" role="alert">
      {{ alert.message }}
      <button type="button" class="btn-close" @click="alert.message = ''"></button>
    </div>

    <div class="container mt-5 d-flex justify-content-end">
      <button class="btn btn-addSalary" @click="showModal = true">Add Salary</button>
    </div>

    <div class="row mb-3 g-3">
      <div class="col-md-3">
        <label class="form-label">Select Year</label>
        <select v-model="selectedYear" class="form-select" @change="loadSalaries(selectedYear, selectedMonth)">
          <option v-for="year in years" :key="year" :value="year">{{ year }}</option>
        </select>
      </div>

      <div class="col-md-3">
        <label class="form-label">Select Month</label>
        <select v-model="selectedMonth" class="form-select" @change="loadSalaries(selectedYear, selectedMonth)">
          <option v-for="(monthName, index) in months" :key="index" :value="index + 1">{{ monthName }}</option>
        </select>
      </div>
    </div>

    <div class="container mt-4">
      <div class="card shadow-sm">
        <div class="card-header bg-primary text-white">
          <h5 class="mb-0">Salary Details</h5>
        </div>
        <div class="card-body p-0">
          <table class="table table-striped mb-0">
            <thead>
              <tr>
                <th>Employee</th>
                <th>Basic Salary</th>
                <th>Conveyance</th>

                <th>Tax</th>
                <th>PF</th>
                <th>ESI</th>
                <th>Gross Salary</th>
                <th>Net Salary</th>
                <th>Payment Mode</th>
                <th>Status</th>
                <th>Created By</th>
                <th>Action By</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="salary in salaries" :key="salary.id">
                <td>{{salary.empName}}</td>
                <td>{{ (salary.basicSalary ?? 0).toFixed(2) }}</td>
                <td>{{ (salary.conveyance ?? 0).toFixed(2) }}</td>
                <td>{{ (salary.tax ?? 0).toFixed(2) }}</td>
                <td>{{ (salary.pf ?? 0).toFixed(2) }}</td>
                <td>{{ (salary.esi ?? 0).toFixed(2) }}</td>
                <td>{{ ((salary.basicSalary ?? 0) + (salary.conveyance ?? 0)).toFixed(2) }}</td>
                <td>{{ ((salary.basicSalary ?? 0) + (salary.conveyance ?? 0) - ((salary.tax ?? 0) + (salary.pf ?? 0) + (salary.esi ?? 0))).toFixed(2) }}</td>

                <td>{{ salary.paymentMode }}</td>
                <td>
                  <select v-model="salary.status"
                          class="form-select form-select-sm"
                          @change="updateStatus(salary)">
                    <option value="Pending">Pending</option>
                    <option value="Approved">Approved</option>
                    <option value="Rejected">Rejected</option>
                  </select>
                </td>
                <td>{{ salary.createdByName || '-' }}</td>
                <td>{{ salary.actionByName || '-' }}</td>


              </tr>
              <tr v-if="salaries.length === 0">
                <td colspan="11" class="text-center py-3">No salary records found.</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>






    <div v-if="showModal" class="modal-overlay" @click.self="closeModal">
      <div class="modal-content">
        <button class="close-btn" @click="closeModal">×</button>
        <div class="card shadow-sm">
          <div class="card-header text-white text-center">
            <h4 class="mb-0 ">Enter Employee Salary</h4>
          </div>
          <div class="card-body">

            <!--<div v-if="alert.message" :class="`alert alert-${alert.type} alert-dismissible fade show`" role="alert">-->
              <!--{{ alert.message }}-->
              <!--<button type="button" class="btn-close" @click="alert.message = ''"></button>
            </div>-->

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
                  <small v-if="errors.basicSalary" class="text-danger">{{ errors.basicSalary }}</small>
                </div>
                <div class="col-md-4">
                  <label class="form-label">Conveyance</label>
                  <input v-model.number="form.conveyance" type="number" class="form-control" required />
                  <small v-if="errors.conveyance" class="text-danger">{{ errors.conveyance }}</small>
                </div>

              </div>

              <div class="row g-3 mb-3">
                <div class="col-md-4">
                  <label class="form-label">Tax</label>
                  <input v-model.number="form.tax" type="number" class="form-control" />
                  <small v-if="errors.tax" class="text-danger">{{ errors.tax }}</small>
                </div>
                <div class="col-md-4">
                  <label class="form-label">PF</label>
                  <input v-model.number="form.pf" type="number" class="form-control" />
                  <small v-if="errors.pf" class="text-danger">{{ errors.pf }}</small>
                </div>
                <div class="col-md-4">
                  <label class="form-label">ESI</label>
                  <input v-model.number="form.esi" type="number" class="form-control" />
                  <small v-if="errors.esi" class="text-danger">{{ errors.esi }}</small>
                </div>
              </div>
              <div class="row g-3 mb-3">
                <div class="col-md-6">
                  <label class="form-label">Salary Month</label>
                  <input v-model="form.salaryDate" type="month" class="form-control" required />
                  <small v-if="errors.salaryDate" class="text-danger">{{ errors.salaryDate }}</small>
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
    <!-- Send PDF Option Modal -->
    <div v-if="showSendPdfModal" class="send-pdf-modal-overlay" @click.self="showSendPdfModal = false">
      <div class="send-pdf-modal-content">
        <button class="send-pdf-close-btn" @click="showSendPdfModal = false">×</button>
        <div class="card shadow-sm text-center">
          <div class="card-header bg-primary text-white">
            <h4 class="mb-0">Send Salary Slip via Email?</h4>
          </div>
          <div class="card-body py-4">
            <p class="mb-4">You can send the generated salary slip to the employee via email. Do you want to proceed?</p>
            <div class="d-flex justify-content-center gap-3">
              <button class="btn btn-success px-4" @click="sendPdfEmail">
                <i class="bi bi-envelope-fill me-2"></i>Yes, Send Email
              </button>
              <button class="btn btn-outline-secondary px-4" @click="showSendPdfModal = false">
                Cancel
              </button>
            </div>
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
  import { getPaymentModes, addSalary, generateSalaryPdf, getAllSalaries, updateSalaryStatus } from "../services/salaryService.js";
  import { watch } from 'vue'

  const employees = ref([])
  const paymentModes = ref([])
  const salaries = ref([]);
  const selectedYear = ref(new Date().getFullYear());
  const selectedMonth = ref(new Date().getMonth() + 1);
  const loading = ref(false)
  const alert = ref({ message: '', type: '' })
  const showModal = ref(false)
  const showSendPdfModal = ref(false)
  const errors = ref({})
  let savedSalaryId = ref(null)
  const years = Array.from({ length: 5 }, (_, i) => new Date().getFullYear() - i); // last 5 years
  const months = [
    "January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
  ];


  const form = ref({
    employeeId: '',
    basicSalary: 0,
    conveyance: 0,
    tax: 0,
    pf: 0,
    esi: 0,
    paymentMethod: '',
    status: 'Pending',
    salaryDate: ''
  })



  const grossSalary = computed(() => Number(form.value.basicSalary || 0) + Number(form.value.conveyance || 0))
  const netSalary = computed(() => (Number(form.value.basicSalary || 0) + Number(form.value.conveyance || 0)) - (Number(form.value.tax || 0) + Number(form.value.pf || 0) + Number( form.value.esi || 0)))

  async function loadEmployees() {
    try {
      const res = await getAllEmployees(1, 100, "Manager", null, null, null, null, "CreatedAt", true)
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

  //async function submitSalary() {
  //  alert.value = { message: '', type: '' }
  //  loading.value = true
  //  try {
  //    const payload = { ...form.value, salaryDate: new Date(form.value.salaryDate + '-01') }
  //    const res = await addSalary(payload)
  //    savedSalaryId.value = res.data // store salary Id returned from API
  //    showModal.value = false
  //    showSendPdfModal.value = true
  //    await loadSalaries(selectedYear.value, selectedMonth.value)
  //  } catch (error) {

  //    //alert.value = { message: 'Error adding salary: ' + error.message, type: 'danger' }

  //    if (err.response?.data?.error) {
  //      alert.value = err.response.data.error;
  //    } else {
  //      alert.value = 'Login failed';
  //    }
  //  } finally {
  //    loading.value = false
  //  }
  //}

  async function submitSalary() {
    // Clear previous errors
    errors.value = {
    }
    loading.value = true
    try {
      const payload = { ...form.value, salaryDate: new Date(form.value.salaryDate + '-01') }
      const res = await addSalary(payload)
      savedSalaryId.value = res.data
      showModal.value = false
      showSendPdfModal.value = true
      await loadSalaries(selectedYear.value, selectedMonth.value)
    } catch (err) {
      alert.value = { message: '', type: '' }
      if (err.response?.status === 400 && err.response.data.errors) {
        const backendErrors = err.response.data.errors
        errors.value = {}

        for (const key in backendErrors) {
          const camelKey = key.charAt(0).toLowerCase() + key.slice(1)
          errors.value[camelKey] = backendErrors[key][0]
        }
      } else {
        alert.value = { message: 'Error adding salary: ' + err.message, type: 'danger' }
      }

    } finally {
      loading.value = false
    }
  }


  // Send PDF Email & Download
  async function sendPdfEmail() {
    try {
      const res = await generateSalaryPdf(savedSalaryId.value, { responseType: 'blob' })
      const pdfBlob = new Blob([res.data], { type: 'application/pdf' })
      const pdfUrl = URL.createObjectURL(pdfBlob)
      const link = document.createElement('a')
      link.href = pdfUrl
      link.download = 'SalarySlip.pdf'
      document.body.appendChild(link)
      link.click()
      document.body.removeChild(link)
      alert.value = { message: 'Salary PDF sent and downloaded!', type: 'success' }
    } catch (error) {
      alert.value = { message: 'Error sending PDF: ' + error.message, type: 'danger' }
    } finally {
      showSendPdfModal.value = false
    }
  }

  async function loadSalaries(year = selectedYear.value, month = selectedMonth.value) {
    try {
      const res = await getAllSalaries(year, month); // Call your API with year/month params
      salaries.value = res.data;
      console.log(salaries.value);
    } catch (error) {
      console.error("Error loading salaries:", error);
    }
  }

  const updateStatus = async (salary) => {
    try {
      await updateSalaryStatus(salary.id, salary.status)
      await loadSalaries(selectedYear.value, selectedMonth.value) 
    } catch (error) {
      console.error('Error updating salary status:', error)
      alert.value = {
        message: error.response?.data?.message || error.message || 'Error updating status.',
        type: 'danger'
      }
    }
  }




  onMounted(() => {
    loadEmployees();
    loadPaymentModes();
    loadSalaries();

  })

  watch([selectedYear, selectedMonth], () => {


    loadSalaries(selectedYear.value, selectedMonth.value)
  })
</script>

<style scoped>
/*  .container {
    max-width: 700px;
  }*/

  .send-pdf-modal-overlay {
    position: fixed;
    inset: 0;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 1050;
  }

  .send-pdf-modal-content {

    background: #fff;
    width: 100%;
    max-width: 400px;
    border-radius: 12px;
    padding: 1rem;
    position: relative;
    box-shadow: 0 8px 25px rgba(0, 0, 0, 0.2);
    animation: fadeInUp 0.3s ease;
  }

  .send-pdf-close-btn {
    position: absolute;
    top: 10px;
    right: 15px;
    border: none;
    background: transparent;
    font-size: 24px;
    font-weight: bold;
    color: #333;
    cursor: pointer;
    transition: color 0.2s ease;
  }

    .send-pdf-close-btn:hover {
      color: #ff4d4f;
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
  .btn-sendPdf {
    background: linear-gradient(135deg, #0d6efd, #6f42c1);
    color: white;
   
    border: none; /* Removes the default border */
    outline: none; /* Removes focus outline (optional) */
  }

    .btn-sendPdf:hover {
      background: linear-gradient(135deg, #6f42c1, #0d6efd);
    }

  .btn-addSalary {
      padding:1rem;
    background: linear-gradient(135deg, #0d6efd, #6f42c1);
    color: white;
    border-radius: 1rem;
    border: none; /* Removes the default border */
    outline: none; /* Removes focus outline (optional) */
  }

    .btn-addSalary:hover {
      background: linear-gradient(135deg, #6f42c1, #0d6efd);
      color: white;
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
