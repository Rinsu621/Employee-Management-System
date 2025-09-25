```vue
<template>
  <div>
    <Navbar />

    <div class="container mt-4">
      <!-- Success/Error Message -->
      <div v-if="successMessage" class="alert alert-success">{{ successMessage }}</div>
      <div v-if="errorMessage" class="alert alert-danger">{{ errorMessage }}</div>

      <div class="row">
        <!-- Left side: Edit form -->
        <div class="col-md-6">
          <h3>Edit Profile</h3>
          <form @submit.prevent="updateProfile">
            <div class="mb-3">
              <label class="form-label">Name</label>
              <input type="text" v-model="user.empName" class="form-control" required />
            </div>

            <div class="mb-3">
              <label class="form-label">Email</label>
              <input type="email" v-model="user.email" class="form-control" disabled />
            </div>

            <div class="mb-3">
              <label class="form-label">Phone</label>
              <input type="text" v-model="user.phone" class="form-control" />
            </div>

            <div class="mb-3">
              <label class="form-label">Department</label>
              <select v-model="user.departmentId" class="form-select">
                <option :value="null" disabled>Select Department</option>
                <option v-for="dept in departments" :key="dept.id" :value="dept.id">
                  {{ dept.name }}
                </option>
              </select>
              <small v-if="user.departmentId" class="text-muted">
                Current selection: {{ getDepartmentName(user.departmentId) }}
              </small>
            </div>

            <button type="submit" class="btn btn-primary">Update Profile</button>
          </form>
        </div>

        <!-- Right side: Profile Card -->
        <div class="col-md-6 d-flex justify-content-center">
          <div class="card text-center p-4 shadow-lg" style="width: 20rem; border-radius: 1rem; position: relative;">
            <div class="avatar-wrapper mb-3">
              <div class="avatar-circle"></div>
              <img :src="userAvatar ? userAvatar : '/OIP.jpeg'"
                   alt="Profile"
                   class="rounded-circle position-absolute top-50 start-50 translate-middle"
                   style="width: 120px; height: 120px; object-fit: cover; border: 4px solid white;" />
            </div>

            <h4 class="card-title mt-3">{{ user.empName }}</h4>
            <p class="text-primary fw-bold">{{ user.role }}</p>
            <p class="card-text text-muted">
              Department: {{ user.departmentName || 'N/A' }}<br />
              Joined: {{ new Date(user.createdAt).toLocaleDateString() }}
            </p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
  import Navbar from "../components/Navbar.vue";
  import { ref, onMounted } from "vue";
  import { getDepartments, getEmployeeByEmail, updateEmployee } from "../services/employeeService";
  import jwtDecode from "jwt-decode";

  const token = localStorage.getItem("token");
  const decoded = token ? jwtDecode(token) : null;

  console.log("Decoded token:", decoded); // Debug: Check token fields

  const userEmail = decoded?.email;
  console.log("User email from token:", userEmail);

  const user = ref({
    id: "",
    empName: "",
    email: "",
    phone: "",
    role: "",
    departmentId: "",
    departmentName: "",
    createdAt: "",
    avatar: ""
  });

  const userAvatar = ref("");
  const departments = ref([]);
  const errorMessage = ref("");
  const successMessage = ref("");


  const getDepartmentName = (departmentId) => {
    const dept = departments.value.find(d => d.id === departmentId);
    return dept ? dept.name : 'N/A';
  };


  async function fetchDepartments() {
    try {
      const response = await getDepartments();
      departments.value = response.data.map(d => ({
        id: d.id.toString(),
        name: d.name
      }));
      console.log("Fetched departments:", departments.value); // Debug
    } catch (err) {
      console.error("Error fetching departments:", err);
      errorMessage.value = `Failed to fetch departments: ${err.response?.data?.message || err.message}`;
    }
  }

  async function fetchUser() {
    if (!userEmail) {
      errorMessage.value = "Authentication token is missing or invalid. Please log in.";
      return;
    }
    try {
      const res = await getEmployeeByEmail(userEmail);
      const data = res.data.data || res.data; // Handle nested response
      user.value = {
        id: data.id,
        empName: data.empName,
        email: data.email,
        phone: data.phone || "",
        role: data.role || "N/A",
        departmentId: data.departmentId ? data.departmentId.toString() : "",
        departmentName: data.departmentName || "",
        createdAt: data.createdAt || new Date().toISOString(),
        avatar: data.avatar || ""
      };
      userAvatar.value = data.avatar || "";
    } catch (err) {
      console.error("Error fetching user:", err);
      errorMessage.value = `Failed to fetch user profile: ${err.response?.data?.message || err.message}`;
    }
  }

  async function updateProfile() {
    errorMessage.value = "";
    successMessage.value = "";
    try {
      const updatedUser = {
        id: user.value.id,
        empName: user.value.empName,
        email: user.value.email, // Send unchanged email
        phone: user.value.phone,
        role: user.value.role, // Send unchanged role
        departmentId: user.value.departmentId || null
      };
      await updateEmployee(updatedUser);
      successMessage.value = "Profile updated successfully!";
      await fetchUser(); // Refresh user data to reflect changes
    } catch (err) {
      console.error("Error updating profile:", err);
      let errorMsg = "Failed to update profile. Please try again.";
      if (err.response) {
        if (err.response.status === 400 && err.response.data.errors) {
          const backendErrors = err.response.data.errors;
          errorMsg = Object.values(backendErrors).flat().join("; ");
        } else {
          errorMsg = `Error ${err.response.status}: ${err.response.data?.message || err.message}`;
        }
      }
      errorMessage.value = errorMsg;
    }
  }

  onMounted(() => {
    if (!token) {
      errorMessage.value = "Please log in to access this page.";
      return;
    }
    fetchDepartments();
    fetchUser();
  });


</script>

<style scoped>
  .avatar-wrapper {
    position: relative;
    width: 120px;
    height: 120px;
    margin: 0 auto;
  }

  .avatar-circle {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    width: 140px;
    height: 140px;
    border-radius: 50%;
    background: rgba(0, 123, 255, 0.2);
    animation: pulse 2s infinite;
    z-index: 0;
  }

  @keyframes pulse {
    0% {
      transform: translate(-50%, -50%) scale(0.9);
      opacity: 0.6;
    }

    50% {
      transform: translate(-50%, -50%) scale(1.1);
      opacity: 0.3;
    }

    100% {
      transform: translate(-50%, -50%) scale(0.9);
      opacity: 0.6;
    }
  }
</style>
```
