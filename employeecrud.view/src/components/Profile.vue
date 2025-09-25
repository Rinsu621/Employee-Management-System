
<template>
  <div>
    <Navbar />
    <div class="container position-relative mt-5 mb-5">
      <div class="background-circle one"></div>
      <div class="background-circle two"></div>
      <div class="background-circle three"></div>


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
                <select v-model="user.departmentName" class="form-select">
                  <option disabled value="">Select Department</option>
                  <option v-for="dept in departments" :key="dept.id" :value="dept.name">
                    {{ dept.name }}
                  </option>
                </select>
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
    </div>
</template>

<script setup>
  import Navbar from "../components/Navbar.vue";
  import { ref, reactive, onMounted, watch } from "vue"
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
        id: d.id.toString(),   // always string
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
      user.value.id = data.id;
      user.value.empName = data.empName;
      user.value.email = data.email;
      user.value.phone = data.phone || "";
      user.value.role = data.role || "N/A";
      user.value.departmentId = data.departmentId ? data.departmentId.toString() : "";
      user.value.departmentName = data.departmentName || "";
      user.value.createdAt = data.createdAt || new Date().toISOString();
      user.value.avatar = data.avatar || "";

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
      const selectedDept = departments.value.find(
        d => d.name === user.value.departmentName
      );
      const updatedUser = {
        id: user.value.id,
        empName: user.value.empName,
        email: user.value.email, // Send unchanged email
        phone: user.value.phone,
        role: user.value.role, // Send unchanged role
        departmentId: selectedDept ? selectedDept.id : user.value.departmentId,
      };
      await updateEmployee(updatedUser);


      successMessage.value = "Profile updated successfully!";
      setTimeout(() => {
        successMessage.value = "";
      }, 5000);
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
      setTimeout(() => {
        errorMessage.value = "";
      }, 5000);
    }
  }

  watch(
    () => user.value.departmentId,
    (newVal) => {
      console.log("user.departmentId changed:", newVal, "typeof:", typeof newVal)
    },
    { immediate: true }
  )

  watch(
    () => departments.value,
    (newVal) => {
      console.log("departments loaded:", newVal)
    },
    { deep: true, immediate: true }
  )

  watch(
    [() => user.value.departmentId, () => departments.value],
    ([deptId, depts]) => {
      const match = depts.find(d => d.id === deptId);
      if (match) {
        user.value.departmentId = match.id; // ensures v-model matches the option
      }
    },
    { immediate: true }
  );
  console.log("User departmentId:", user.value.departmentId, "typeof:", typeof user.value.departmentId);
  console.log("Departments:", departments.value.map(d => ({ id: d.id, name: d.name, type: typeof d.id })));

  async function loadProfile() {
    await fetchDepartments();
    await fetchUser();
  }

  onMounted(() => {
    loadProfile();
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

  .container {
    position: relative;
    z-index: 1;
  }

  /* Floating background circles */
  .background-circle {
    position: absolute;
    border-radius: 50%;
    opacity: 0.15;
    z-index: 0;
    animation: float 20s infinite linear;
  }

    /* Different sizes and positions */
    .background-circle.one {
      width: 200px;
      height: 200px;
      background: radial-gradient(circle, #0d6efd 0%, transparent 70%);
      top: -50px;
      left: -50px;
    }

    .background-circle.two {
      width: 150px;
      height: 150px;
      background: radial-gradient(circle, #6f42c1 0%, transparent 70%);
      bottom: -30px;
      right: -30px;
    }

    .background-circle.three {
      width: 100px;
      height: 100px;
      background: radial-gradient(circle, #198754 0%, transparent 70%);
      top: 150px;
      right: -50px;
    }

  @keyframes float {
    0% {
      transform: translateY(0) translateX(0);
    }

    50% {
      transform: translateY(-30px) translateX(20px);
    }

    100% {
      transform: translateY(0) translateX(0);
    }
  }

  /* Card styling */
  .card {
    background: rgba(255, 255, 255, 0.95);
    border: none;
    border-radius: 1rem;
    box-shadow: 0 15px 35px rgba(0,0,0,0.2);
    position: relative;
    z-index: 2;
  }

  /* Avatar */
  .avatar-wrapper {
    position: relative;
    width: 120px;
    height: 120px;
    margin: 0 auto 1rem;
  }

  .avatar-circle {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    width: 140px;
    height: 140px;
    border-radius: 50%;
    background: rgba(13, 110, 253, 0.2);
    animation: pulse 2s infinite;
    z-index: 0;
  }

  /* Button hover effect */
  .btn-primary {
    background: linear-gradient(45deg, #0d6efd, #6f42c1);
    border: none;
    transition: transform 0.3s, box-shadow 0.3s;
  }

    .btn-primary:hover {
      transform: translateY(-3px);
      box-shadow: 0 8px 25px rgba(0,0,0,0.2);
    }

  /* Floating animation for pulse avatar */
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
