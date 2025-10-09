<template>

  <div>
    <div class="background-circles">
      <div class="circle c1"></div>
      <div class="circle c2"></div>
      <div class="circle c3"></div>
      <div class="circle c4"></div>
      <div class="circle c5"></div>
    </div>
    <Layout>
      <div class="container mt-4">
        <h2>Employee List</h2>

        <!--Filter & Sort-->
        <div class="mb-3 d-flex align-items-center justify-content-between">
          <div class="d-flex align-items-center">
            <!-- Sort -->
            <label class="me-2">Sort by:</label>
            <select v-model="sortKey" class="form-select w-auto me-2">
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

            <label class="ms-2 me-1">Department:</label>
            <select v-model="selectedDepartment" class="form-select w-auto">
              <option value="">All</option>
              <option v-for="dept in departments" :key="dept.id" :value="dept.id">{{dept.name}}</option>
            </select>
          </div>

          <!-- Create Button -->
          <div v-if="currentUserRole==='Admin'|| currentUserRole==='Manager'">
            <button class="btn btn-success shadow-sm px-4 py-2 d-flex align-items-center" @click="openCreateModal">
              <i class="bi bi-person-plus-fill me-2 fs-5"></i>
              <span class="fw-semibold">Add User</span>
            </button>
          </div>
        </div>

        <!-- Table -->
        <div class="table-responsive shadow-sm rounded">
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
                <th v-if="currentUserRole === 'Admin' || currentUserRole === 'Manager'">Action</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(user, index) in employees" :key="user.id"
                  :class="{ 'table-info': user.email === currentAdminEmail }">
                <td>{{ (currentPage - 1) * pageSize + index + 1 }}</td>
                <td>{{ user.empName }}</td>
                <td>{{ user.email }}</td>
                <td>{{ user.phone }}</td>
                <td>{{ user.departmentName || 'N/A' }}</td>
                <td>{{ user.role }}</td>
                <td>{{ new Date(user.createdAt).toLocaleDateString() }}</td>
                <td v-if="currentUserRole === 'Admin' || currentUserRole === 'Manager'">
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

          <!-- Pagination -->
          <div class="d-flex align-items-center justify-content-between mb-2 mt-3">
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
              <button type="button" class="btn-close" @click="editModalInstance.value.hide()"></button>
            </div>
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
                    <option v-for="dept in departments" :key="dept.id" :value="dept.id.toString()">{{ dept.name }}</option>
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
          </div>
        </div>
      </div>
    </Layout>
  </div>

</template>

<script setup>
  import Navbar from "../components/Navbar.vue"
  import Layout from "../components/Layout.vue"
  import { ref, computed, onMounted, watch } from "vue"
  import { getAllEmployees, createEmployee, getRoles, updateEmployee, deleteEmployeeById, getDepartments } from "../services/employeeService"
  import * as bootstrap from 'bootstrap/dist/js/bootstrap.bundle.min.js'
  import jwtDecode from "jwt-decode"
  import { logout } from '../services/authService'

  const employees = ref([])
  const sortKey = ref("id")
  const sortAsc = ref(true)
  const roles = ref([])
  const errors = ref({})
  const token = sessionStorage.getItem("token")
  if (!token) {
    logout();
  }
  const decoded = jwtDecode(token)
  console.log(decoded)

  const currentAdminEmail = ref(decoded.email)
  const currentUserRole = ref(
    decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || ""
  );


  const createModal = ref(null)
  const createModalInstance = ref(null)
  const newEmployee = ref({ empName: '', email: '', phone: '', role: '' })

  const editModal = ref(null)
  const editModalInstance = ref(null)
  const editingEmployee = ref({})
  const departments = ref([])
  const totalEmployees = ref(0);
  const currentPage = ref(1);
  const pageSize = ref(5);
  const totalPages = computed(() => Math.ceil(totalEmployees.value / pageSize.value));
  const selectedDepartment = ref("");


  // Sort employees dynamically
  const sortedEmployees = computed(() => {
    return [...employees.value].sort((a, b) => {
      const aVal = a[sortKey.value] || ""
      const bVal = b[sortKey.value] || ""
      if (aVal < bVal) return sortAsc.value ? -1 : 1
      if (aVal > bVal) return sortAsc.value ? 1 : -1
      return 0
    })
  })


  function openCreateModal() { createModalInstance.value?.show() }
  function closeCreateModal() { createModalInstance.value?.hide() }

  function openEditModal(employee) {
    const dept = departments.value.find(d => d.name === employee.departmentName);
    editingEmployee.value = { ...employee, departmentId: dept ? dept.id.toString() : "" };
    editModalInstance.value?.show();

  }

  async function createEmployeeHandler() {
    errors.value = {}
    try {
      await createEmployee(newEmployee.value)
      await fetchEmployees()
      closeCreateModal()
    } catch (err) {
      if (err.response?.status === 400 && err.response.data.errors) {
        const backendErrors = err.response.data.errors
        for (const key in backendErrors) {
          const field = key.split(".").pop()
          errors.value[field] = backendErrors[key][0]
        }
      } else console.error(err)
    }
  }

  async function editEmployeeHandler() {
    try {
      console.log("Updating employee with data:", editingEmployee.value)
      await updateEmployee(editingEmployee.value)
      await fetchEmployees()
      editModalInstance.value?.hide()
    } catch (err) { console.error(err) }
  }

  async function deleteEmployee(employee) {
    if (employee.email === currentAdminEmail.value) {
      alert("You cannot delete yourself!")
      return
    }
    if (confirm(`Are you sure you want to delete ${employee.empName}?`)) {
      try { await deleteEmployeeById(employee.id); await fetchEmployees() }
      catch (err) { console.error(err) }
    }
  }

  async function fetchEmployees() {
    try {
      const res = await getAllEmployees(
        currentPage.value,
        pageSize.value,
        "Employee", selectedDepartment.value || null, null, null
      );
      employees.value = res.data.employees // adjust based on API
      totalEmployees.value = res.data.totalCount;

    } catch (err) { console.error(err) }
  }

  async function fetchRoles() {
    try { roles.value = (await getRoles()).data } catch (err) { console.error(err) }
  }

  async function fetchDepartments() {
    try {
      const response = await getDepartments()
      departments.value = response.data.map(d => ({ id: d.id.toString(), name: d.name }))
    } catch (err) { console.error(err) }
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
    fetchEmployees()
    fetchRoles()
    fetchDepartments()
    if (createModal.value) createModalInstance.value = new bootstrap.Modal(createModal.value)
    if (editModal.value) editModalInstance.value = new bootstrap.Modal(editModal.value)
  })

  watch([currentPage, pageSize, sortKey, sortAsc], () => {
    fetchEmployees();
  });
  watch([selectedDepartment], () => { currentPage.value = 1; fetchEmployees(); });

</script>

<style scoped>
  .container {
    position: relative;
    z-index: 1; /* Keep content above circles */
  }

  .background-circles {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    overflow: hidden;
    z-index: -1; /* Behind content */
  }

    .background-circles .circle {
      position: absolute;
      border-radius: 50%;
      background-color: rgba(13, 110, 253, 0.3);
      animation: float 15s infinite ease-in-out;
    }

  /* Different sizes and positions */
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

