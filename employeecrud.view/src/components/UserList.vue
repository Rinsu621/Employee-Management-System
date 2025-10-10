<template>
  <div>
    <div aria-live="polite" aria-atomic="true" class="position-relative">
      <div class="toast-container position-fixed top-0 end-0 p-3" id="toastContainer"></div>
    </div>

    <div class="background-circles">
      <div class="circle c1"></div>
      <div class="circle c2"></div>
      <div class="circle c3"></div>
      <div class="circle c4"></div>
      <div class="circle c5"></div>
    </div>
    <Layout>
      <div class="container mt-4">

        <h2>User List</h2>

        <div class="mb-3 d-flex align-items-center ">
          <!-- Left side: Sorting Dropdowns -->
          <div class="d-flex align-items-center justify-content-between">
            <label class="me-2">Sort by:</label>
            <select v-model="sortKey" class="form-select w-auto me-3">
              <option value="id">S.No</option>
              <option value="empName">Name</option>
              <option value="email">Email</option>
              <option value="role">Role</option>
              <option value="createdAt">Created At</option>
            </select>

            <select v-model="sortAsc" class="form-select w-auto">
              <option :value="true">Ascending</option>
              <option :value="false">Descending</option>
            </select>

            <!-- Role Filter -->
            <label class="ms-4 me-2">Role:</label>
            <select v-model="selectedRole" class="form-select w-auto">
              <option value="">All</option>
              <option v-for="role in roles" :key="role" :value="role">{{ role }}</option>
            </select>

            <!-- Department Filter -->
            <label class="ms-4 me-2">Department:</label>
            <select v-model="selectedDepartment" class="form-select w-auto">
              <option value="">All</option>
              <option v-for="dept in departments" :key="dept.id" :value="dept.id">{{ dept.name }}</option>
            </select>
          </div>

          <!-- From Date Filter -->
          <label class="ms-4 me-2">From:</label>
          <input type="date" v-model="fromDate" class="form-control w-auto d-inline" />

          <!-- To Date Filter -->
          <label class="ms-4 me-2">To:</label>
          <input type="date" v-model="toDate" class="form-control w-auto d-inline" />

        </div>
        <!--Search Bar-->
        <div class="mb-3 d-flex align-items-center justify-content-between">
          <div class="mb-3 d-flex align-items-center">
            <label class="me-2">Search:</label>
            <input type="text"
                   v-model="searchTerm"
                   class="form-control w-auto"
                   placeholder="Search" />
          </div>

        
          <div class="d-flex justify-content-end mb-3 gap-2 ">

            <button class="btn-export shadow-sm d-flex align-items-center px-3 py-1" @click="exportToExcel">
              <i class="bi bi-file-earmark-spreadsheet-fill me-2 fs-5" style="color:#217346;"></i>
              <span class="fw-semibold">Export</span>
            </button>

            <button class="btn-add  shadow-sm px-4 py-2 d-flex align-items-center" @click="openCreateModal">
              <i class="bi bi-person-plus-fill me-2 fs-5"></i>
              <span class="fw-semibold">Add User</span>
            </button>
          </div>
        </div>


          <!-- Table -->
          <div class="table-responsive shadow-sm rounded">
            <!-- Skeleton Table -->
            <b-skeleton-table v-if="loading"
                              :rows="5"
                              :columns="8"
                              :table-props="{ bordered: true, striped: true }">
            </b-skeleton-table>
            <table class="table table-hover align-middle">
              <thead class="table-dark">
                <tr>
                  <th>S.No</th>
                  <th>Name</th>
                  <th>Email</th>
                  <th>Phone</th>
                  <th>Department</th>
                  <th>Role</th>
                  <th>Created At</th>
                  <th>Action</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(user, index) in employees"
                    :key="user.id"
                    :class="{ 'table-info': user.email === currentAdminEmail }">
                  <td>{{ (currentPage - 1) * pageSize + index + 1 }}</td>
                  <td>{{ user.empName }}</td>
                  <td>{{ user.email }}</td>
                  <td>{{ user.phone }}</td>
                  <td>{{ user.departmentName || 'N/A' }}</td>
                  <td>{{ user.role }}</td>
                  <td>{{ new Date(user.createdAt).toLocaleDateString() }}</td>
                  <td>
                    <template v-if="user.email !== currentAdminEmail">
                      <div class="btn-group" role="group">
                        <button class="btn btn-outline-secondary" @click="openEditModal(user)" title="Edit">
                          <i class="bi bi-pencil-square fs-5"></i>
                        </button>
                        <button class="btn btn-outline-danger" @click="deleteEmployee(user)" title="Delete">
                          <i class="bi bi-trash fs-5"></i>
                        </button>
                      </div>
                    </template>
                  </td>
                </tr>
                <tr v-if="employees.length === 0">
                  <td colspan="8" class="text-center">No employees found</td>
                </tr>
              </tbody>
            </table>
          </div>

          <div class="d-flex align-items-center justify-content-between mb-2 mt-3">
            <!-- Left side -->
            <div>
              Showing {{ (currentPage - 1) * pageSize + 1 }} -
              {{ Math.min(currentPage * pageSize, totalEmployees) }}
              out of {{ totalEmployees }}
            </div>

            <!-- Middle: Pagination buttons -->
            <div>
              <button class="btn btn-sm btn-secondary me-1" @click="prevPage">Previous</button>
              <button class="btn btn-sm me-1"
                      v-for="page in totalPages"
                      :key="page"
                      @click="goToPage(page)"
                      :class="currentPage=== page ? 'btn-secondary' : 'btn-outline-secondary'">
                {{ page }}
              </button>
              <button class="btn btn-sm btn-secondary" @click="nextPage">Next</button>
            </div>

            <!-- Right side: Rows per page -->
            <div>
              <label>Rows per page:</label>
              <select v-model="pageSize" class="form-select d-inline w-auto ms-1">
                <option v-for="size in [5,10,20,50]" :key="size" :value="size">{{ size }}</option>
              </select>
            </div>
          </div>

        </div>
</Layout>
  </div>


  <!-- Create User Modal -->
  <div class="modal fade" tabindex="-1" ref="createModal">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Add New Employee</h5>
          <button type="button" class="btn-close" @click="closeCreateModal"></button>
        </div>
        <div class="modal-body">
          <form @submit.prevent="createEmployeeHandler">
            <div class="mb-3">
              <label class="form-label">Name</label>
              <input type="text" v-model="newEmployee.empName" class="form-control" required />
              <small class="text-danger">{{ errors.EmpName }}</small>
            </div>
            <div class="mb-3">
              <label class="form-label">Email</label>
              <input type="email" v-model="newEmployee.email" class="form-control" required />
              <small class="text-danger">{{ errors.Email }}</small>
            </div>
            <div class="mb-3">
              <label class="form-label">Phone</label>
              <input type="text" v-model="newEmployee.phone" class="form-control" />
              <small class="text-danger">{{ errors.Phone }}</small>
            </div>

            <div class="mb-3">
              <label class="form-label">Department</label>
              <select v-model="newEmployee.departmentId" class="form-select" required>
                <option value="" disabled>Select Department</option>
                <option v-for="dept in departments" :key="dept.id" :value="dept.id.toString()">{{dept.name}}</option>
              </select>
            </div>
            <div class="mb-3">
              <label class="form-label">Role</label>
              <select v-model="newEmployee.role" class="form-select" required>
                <option value="" disabled>Select role</option>
                <option v-for="role in roles" :key="role" :value="role">{{ role }}</option>
              </select>
            </div>

            <button type="submit" class="btn btn-success">Create</button>
            <button type="button" class="btn btn-secondary ms-2" @click="closeCreateModal">Cancel</button>
          </form>
        </div>
      </div>
    </div>
  </div>

  <!-- Edit Employee Modal -->
  <div class="modal fade" tabindex="-1" ref="editModal">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Edit Employee</h5>
          <button type="button" class="btn-close" @click="editModalInstance.hide()"></button>
        </div>
        <b-overlay :show="showOverlay" rounded="sm">
          <div class="modal-body">
            <form @submit.prevent="editEmployeeHandler">
              <div class="mb-3">
                <label class="form-label">Name</label>
                <input type="text" v-model="editingEmployee.empName" class="form-control" required />
              </div>
              <div class="mb-3">
                <label class="form-label">Email</label>
                <input type="email" v-model="editingEmployee.email" class="form-control" required />
              </div>
              <div class="mb-3">
                <label class="form-label">Phone</label>
                <input type="text" v-model="editingEmployee.phone" class="form-control" />
              </div>
              <div class="mb-3">
                <label class="form-label">Department</label>
                <select v-model="editingEmployee.departmentId" class="form-select">
                  <option value="" disabled>Select department</option>
                  <option v-for="dept in departments" :key="dept.id" :value="dept.id.toString()">
                    {{ dept.name }}
                  </option>
                </select>
              </div>
              <div class="mb-3">
                <label class="form-label">Role</label>
                <select v-model="editingEmployee.role" class="form-select">
                  <option v-for="role in roles" :key="role" :value="role">{{ role }}</option>
                </select>
              </div>
              <button type="submit" class="btn btn-primary">Update</button>
              <button type="button" class="btn btn-secondary ms-2" @click="editModalInstance.hide()">Cancel</button>
            </form>
          </div>
        </b-overlay>
      </div>
    </div>
  </div>
</template>

<script setup>
  import Navbar from "../components/Navbar.vue"
  import Layout from "../components/Layout.vue"
  import { ref, computed, onMounted, watch, watchEffect } from "vue"
  import { logout } from "../services/authService.js"
  import { getAllEmployees, createEmployee, getRoles, updateEmployee, deleteEmployeeById, getDepartments, exportEmployeesToExcel } from "../services/employeeService"
  import * as bootstrap from 'bootstrap/dist/js/bootstrap.bundle.min.js';
  import jwtDecode from "jwt-decode";

  const employees = ref([]);
  const sortKey = ref("id");
  const sortAsc = ref(true);
  const roles = ref([]);
  const errors = ref({});
  let decode = null;;
  const token = sessionStorage.getItem("token");
  if (!token) {
    logout();
  }
  const decoded = jwtDecode(token);
  const currentAdminEmail = ref(decoded.email);

  const createModal = ref(null);
  const createModalInstance = ref(null);
  const newEmployee = ref({ empName: '', email: '', phone: '', departmentId: '', role: '' });

  const editModal = ref(null);
  const editModalInstance = ref(null);
  const editingEmployee = ref({});
  const departments = ref([]);
  const totalEmployees = ref(0);
  const currentPage = ref(1);
  const pageSize = ref(5);
  const totalPages = computed(() => Math.ceil(totalEmployees.value / pageSize.value));

  const selectedRole = ref("");
  const selectedDepartment = ref("");
  const fromDate = ref(null)
  const toDate = ref(null)
  const searchTerm = ref("")
  const loading = ref(true);
  const showOverlay = ref(false);






  function showToast(message, variant = 'success') {
    const toastContainer = document.getElementById('toastContainer');
    if (!toastContainer) return;

    const toastEl = document.createElement('div');
    toastEl.className = `toast align-items-center text-bg-${variant} border-0`;
    toastEl.role = 'alert';
    toastEl.ariaLive = 'assertive';
    toastEl.ariaAtomic = 'true';
    toastEl.innerHTML = `
    <div class="d-flex">
      <div class="toast-body">${message}</div>
      <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
    </div>
  `;

    toastContainer.appendChild(toastEl);

    const bsToast = new bootstrap.Toast(toastEl, { delay: 3000 });
    bsToast.show();

    toastEl.addEventListener('hidden.bs.toast', () => {
      toastEl.remove();
    });
  }



  function openCreateModal() { createModalInstance.value?.show(); }
  function closeCreateModal() { createModalInstance.value?.hide(); }

  function openEditModal(employee) {
    const dept = departments.value.find(d => d.name === employee.departmentName);
    editingEmployee.value = { ...employee, departmentId: dept ? dept.id.toString() : "" };
    editModalInstance.value?.show();
  }

  async function createEmployeeHandler() {
    errors.value = {};
    try {
      await createEmployee(newEmployee.value);
      await fetchEmployees();
      newEmployee.value = { empName: '', email: '', phone: '', departmentId: '', role: '' };
      closeCreateModal();
      showToast('Employee added successfully!', 'success')
    } catch (err) {
      if (err.response?.status === 400 && err.response.data.errors) {
        const backendErrors = err.response.data.errors;
        for (const key in backendErrors) {
          const field = key.split(".").pop();
          errors.value[field] = backendErrors[key][0];
        }
      } else {
        showToast('Failed to add employee. Please try again.', 'danger')
        console.error(err);
      }
    }
  }

  async function editEmployeeHandler() {
    showOverlay.value = true;
    try {
      await updateEmployee(editingEmployee.value);
      await fetchEmployees();
      showToast('Employee edited successfully!', 'success')
      editModalInstance.value?.hide();
    } catch (err) {
      console.error(err);
    }
    finally {
      showOverlay.value = false; // hide overlay
    }
  }

  async function deleteEmployee(employee) {
    if (employee.email === currentAdminEmail.value) {
      alert("You cannot delete yourself!");
      return;
    }
    if (confirm(`Are you sure you want to delete ${employee.empName}?`)) {
      try {
        await deleteEmployeeById(employee.id);
        await fetchEmployees();
      } catch (err) {
        showToast('Failed to edit employee. Please try again.', 'danger')
        console.error(err);
      }
    }
  }

  async function fetchEmployees() {
    loading.value = true;
    try {
      console.log("Fetching with:", currentPage.value, pageSize.value, selectedRole.value, selectedDepartment.value);
      const res = await getAllEmployees(currentPage.value, pageSize.value, selectedRole.value || null,
        selectedDepartment.value || null, fromDate.value || null,
        toDate.value || null, searchTerm.value || null, sortKey.value || "createdAt", sortAsc.value);
      console.log("Response:", res.data);
      employees.value = res.data.employees;
      totalEmployees.value = res.data.totalCount;
    } catch (err) {
      console.error(err);
    }
  }
  async function fetchRoles() {
    try {
      const res = await getRoles();
      roles.value = res.data;
    } catch (err) {
      console.error(err);
    }
  }

  async function fetchDepartments() {
    try {
      const response = await getDepartments();
      departments.value = response.data.map(d => ({
        id: d.id.toString(),
        name: d.name
      }));
    } catch (err) {
      console.error("Error fetching departments:", err);
    }
  }
  function prevPage() {
    if (currentPage.value > 1) currentPage.value--;
  }

  function nextPage() {
    if (currentPage.value < totalPages.value) currentPage.value++;
  }

  function goToPage(page) {
    currentPage.value = page;
  }


  onMounted(() => {
    fetchEmployees();
    fetchRoles();
    fetchDepartments();
    if (createModal.value) createModalInstance.value = new bootstrap.Modal(createModal.value);
    if (editModal.value) editModalInstance.value = new bootstrap.Modal(editModal.value);
  });


  const exportToExcel = async () => {
    try {
      const filters = {
        Role: selectedRole.value || null,
        DepartmentId: selectedDepartment.value || null,
        FromDate: fromDate.value || null,
        ToDate: toDate.value || null,
        SearchTerm: searchTerm.value || null,
        SortKey: sortKey.value,
        SortAsc: sortAsc.value
      };

      const response = await exportEmployeesToExcel(filters);

      //Blob is a binary large object, way to handle file like raw data in js to blob as browser can handle it as file
      //convert response data to blob and create a temporary URL for Blob so browser can download it
      const url = window.URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement('a'); //create a new anchor tag dynamically using js, can be used to trigger  download
      link.href = url;// <a href="blob:http://localhost:5173/1234-5678-blob"></a> it will look like this
      link.download = 'Employees.xlsx';
      document.body.appendChild(link);
      link.click();  //<a href="blob:http://localhost:5173/1234-5678-blob" download="Employees.xlsx"></a>
      document.body.removeChild(link);
      window.URL.revokeObjectURL(url);
    } catch (error) {
      console.error('Error exporting employees:', error);
    }
  };


  watch([currentPage, pageSize, sortKey, sortAsc], () => {
    fetchEmployees();
  });

  watch([selectedRole, selectedDepartment, fromDate, toDate, searchTerm], () => {
    currentPage.value = 1;
    fetchEmployees();
  });

</script>

<style scoped>
  .container {
    position: relative;
    z-index: 1; 
  }

  .background-circles {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    overflow: hidden;
    z-index: -1; 
  }

    .background-circles .circle {
      position: absolute;
      border-radius: 50%;
      background-color: rgba(13, 110, 253, 0.3);
      animation: float 15s infinite ease-in-out;
    }


  .c1 {
    width: 150px;
    height: 150px;
    top: 10%;
    left: 5%;
    animation-duration: 20s;
  }

  .c2 {
    width: 100px;
    height: 100px;
    top: 40%;
    left: 80%;
    animation-duration: 18s;
  }

  .c3 {
    width: 120px;
    height: 120px;
    top: 70%;
    left: 20%;
    animation-duration: 22s;
  }

  .c4 {
    width: 80px;
    height: 80px;
    top: 60%;
    left: 50%;
    animation-duration: 16s;
  }

  .c5 {
    width: 180px;
    height: 180px;
    top: 20%;
    left: 60%;
    animation-duration: 25s;
  }

  .page-wrapper {
    position: relative;
    min-height: 100vh; /* Full viewport height */
    overflow: hidden;
    background-color: #f8f9fa; /* Optional: light background */
  }

  .background-circles {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    overflow: hidden;
    z-index: 0; /* Behind everything */
  }

  .btn-add {
    background: linear-gradient(135deg, #0d6efd, #6f42c1);
    color: white;
    border-radius: 1rem;
    border: none; /* Removes the default border */
    outline: none; /* Removes focus outline (optional) */
  }

    .btn-add:hover {
      background: linear-gradient(135deg, #6f42c1, #0d6efd);
    }

    .btn-export {
  background-color: white;
  color: #217346; /* Excel green color */
  border: 1px solid #ccc;
  border-radius: 0.5rem; /* smaller radius */
  font-size: 0.9rem;     /* slightly smaller text */
  transition: background 0.2s;
}

.btn-export:hover {
  background-color: #f1f1f1;
}
  @keyframes float {
    0% {
      transform: translateY(0) translateX(0);
      opacity: 0.15;
    }

    50% {
      transform: translateY(-20px) translateX(20px);
      opacity: 0.25;
    }

    100% {
      transform: translateY(0) translateX(0);
      opacity: 0.15;
    }
  }

</style>
