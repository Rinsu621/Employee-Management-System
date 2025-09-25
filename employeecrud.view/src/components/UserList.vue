<template>
  <div>
    <Navbar />
    <div class="container mt-4">
      <h2>User List</h2>

      <div class="mb-3 d-flex align-items-center justify-content-between">
        <!-- Left side: Sorting Dropdowns -->
        <div class="d-flex align-items-center">
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
        </div>

        <!-- Right side: Create Button -->
        <div>
          <button class="btn btn-success" @click="openCreateModal">Add User</button>
        </div>
      </div>

      <!-- Table -->
      <table class="table table-bordered table-striped mt-3">
        <thead class="table-light">
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
          <tr v-for="(user, index) in paginatedEmployees"
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
                <button class="btn btn-primary btn-sm me-2" @click="openEditModal(user)">Edit</button>
                <button class="btn btn-danger btn-sm" @click="deleteEmployee(user)">Delete</button>
              </template>
            </td>
          </tr>
          <tr v-if="paginatedEmployees.length === 0">
            <td colspan="7" class="text-center">No employees found</td>
          </tr>
        </tbody>
      </table>

      <!-- Pagination -->
      <nav class="mt-3">
        <ul class="pagination">
          <li class="page-item" :class="{ disabled: currentPage === 1 }">
            <button class="page-link" @click="prevPage">Previous</button>
          </li>
          <li class="page-item"
              v-for="page in totalPages"
              :key="page"
              :class="{ active: currentPage === page }">
            <button class="page-link" @click="goToPage(page)">
              {{ page }}
            </button>
          </li>
          <li class="page-item" :class="{ disabled: currentPage === totalPages }">
            <button class="page-link" @click="nextPage">Next</button>
          </li>
        </ul>
      </nav>

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
      <!--End of modal-->
      <!--Edit Modal-->
      <div class="modal fade" tabindex="-1" ref="editModal">
        <div class="modal-dialog">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title">Edit Employee</h5>
              <button type="button" class="btn-close" @click="editModalInstance.hide()"></button>
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
          </div>
        </div>


      </div>
    </div>
  </div>
</template>

<script setup>
  import Navbar from "../components/Navbar.vue"
  import { ref, computed, onMounted, watch } from "vue"
  import { getAllEmployees, createEmployee, getRoles, updateEmployee, deleteEmployeeById , getDepartments} from "../services/employeeService"
  import * as bootstrap from 'bootstrap/dist/js/bootstrap.bundle.min.js';
  import jwtDecode from "jwt-decode";

  const employees = ref([]);
  const sortKey = ref("id");
  const sortAsc = ref(true);
  const currentPage = ref(1);
  const pageSize = 5;
  const roles = ref([]);
  const errors = ref({});
  //const currentAdminEmail = ref(localStorage.getItem("userEmail"));
  const token = localStorage.getItem("token"); // your JWT
const decoded = jwtDecode(token);
const currentAdminEmail = ref(decoded.email);

  const createModal = ref(null);
  const createModalInstance = ref(null);
  const newEmployee = ref({ empName: '', email: '', phone: '', role: '' });

  const editModal = ref(null);
  const editModalInstance = ref(null);
  const editingEmployee = ref({});
  const departments = ref([]);

  const sortedEmployees = computed(() => {
    return [...employees.value].sort((a, b) => {
      const aVal = a[sortKey.value] || "";
      const bVal = b[sortKey.value] || "";
      if (aVal < bVal) return sortAsc.value ? -1 : 1;
      if (aVal > bVal) return sortAsc.value ? 1 : -1;
      return 0;
    });
  });

  const totalPages = computed(() => Math.ceil(sortedEmployees.value.length / pageSize));

  const paginatedEmployees = computed(() => {
    const start = (currentPage.value - 1) * pageSize;
    return sortedEmployees.value.slice(start, start + pageSize);
  });

  function nextPage() { if (currentPage.value < totalPages.value) currentPage.value++; }
  function prevPage() { if (currentPage.value > 1) currentPage.value--; }
  function goToPage(page) { currentPage.value = page; }

  function openCreateModal() { createModalInstance.value?.show(); }
  function closeCreateModal() { createModalInstance.value?.hide(); }

  function openEditModal(employee) {
    const dept = departments.value.find(d => d.name === employee.departmentName);
    editingEmployee.value = { ...employee,  departmentId: dept? dept.id.toString() : ""  };
    editModalInstance.value?.show();
  }

  async function createEmployeeHandler() {
    errors.value = {};
    try {
      await createEmployee(newEmployee.value);
      await fetchEmployees();
      closeCreateModal();
    } catch (err) {
      if (err.response?.status === 400 && err.response.data.errors) {
        const backendErrors = err.response.data.errors;
        for (const key in backendErrors) {
          const field = key.split(".").pop();
          errors.value[field] = backendErrors[key][0];
        }
      } else {
        console.error(err);
      }
    }
  }

  async function editEmployeeHandler() {
    try {
      await updateEmployee(editingEmployee.value);
      await fetchEmployees();
      editModalInstance.value?.hide();
    } catch (err) {
      console.error(err);
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
        console.error(err);
      }
    }
  }

  async function fetchEmployees() {
    try {
      const res = await getAllEmployees();
      employees.value = res.data;
    } catch (err) { console.error(err); }
  }

  async function fetchRoles() {
    try {
      const res = await getRoles();
      roles.value = res.data;
    } catch (err) { console.error(err); }
  }
  async function fetchDepartments() {
  try {
    const response = await getDepartments(); // Axios call returning response.data
    departments.value = response.data.map(d => ({
      id: d.id.toString(),   // make sure this matches the type of editingEmployee.departmentId
      name: d.name
    }));
  } catch (err) {
    console.error("Error fetching departments:", err);
  }
}


  onMounted(() => {
    fetchEmployees();
    fetchRoles();
      fetchDepartments();
    if (createModal.value) createModalInstance.value = new bootstrap.Modal(createModal.value);
    if (editModal.value) editModalInstance.value = new bootstrap.Modal(editModal.value);

  });

  watch([sortKey, sortAsc], () => { currentPage.value = 1; });

</script>

