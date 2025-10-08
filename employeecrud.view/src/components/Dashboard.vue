<template>
  <Layout>
    <div class="dashboard position-relative">

      <div class="background-circle one"></div>
      <div class="background-circle two"></div>
      <div class="background-circle three"></div>
      <div class="container py-5 position-relative" style="z-index:1;">

        <h2 class="mb-6 text-center text-primary">Welcome to Dashboard</h2>

        <div class="row justify-content-center g-3 mt-3">
          <!-- User List Card -->
          <div v-if="userRole === 'Admin'" class="col-12 col-md-4">
            <div class="card dashboard-card h-100">
              <div class="card-body text-center d-flex flex-column">
                <i class="bi bi-people-fill fs-1 mb-3"></i>
                <h5 class="card-title">User List</h5>
                <p class="card-text mb-4">
                  View and manage all users in the system. Add, edit, or remove user accounts easily.
                </p>
                <button class="btn btn-light mt-auto" @click="goToUserList">
                  Go to User List
                </button>
              </div>
            </div>
          </div>

          <!-- Role List Card -->
          <div v-if="userRole === 'Admin'" class="col-12 col-md-4">
            <div class="card dashboard-card h-100">
              <div class="card-body text-center d-flex flex-column">
                <i class="bi bi-shield-lock-fill fs-1 mb-3"></i>
                <h5 class="card-title">Role List</h5>
                <p class="card-text mb-4">
                  See the list of roles available
                </p>
                <button class="btn btn-light mt-auto" @click="goToRoleList">
                  Go to Role List
                </button>
              </div>
            </div>
          </div>

          <!-- Employee List Card -->
          <div class="col-12 col-md-4">
            <div class="card dashboard-card h-100">
              <div class="card-body text-center d-flex flex-column">
                <i class="bi bi-person-badge-fill fs-1 mb-3"></i>
                <h5 class="card-title">Employee List</h5>
                <!--<p class="card-text mb-4">
                See all employees, manage their information, and monitor their roles and departments.
              </p>-->
                <p class="card-text mb-4">
                  {{
              userRole === 'Admin'
               ? 'See all employees, manage their information, and monitor their roles and departments.'
              : userRole === 'Manager'
               ? 'View your team members and track their roles and departments.'
               :'View information of Employee.'
                  }}
                </p>
                <button class="btn btn-light mt-auto" @click="goToEmployeeList">
                  Go to Employee List
                </button>
              </div>
            </div>
          </div>

        </div>
      </div>
    </div>
  </Layout>
</template>

<script setup>
  import Navbar from "../components/Navbar.vue"
  import Layout from "../components/Layout.vue"
  import { ref, onMounted } from "vue"

  import * as jwt_decode from "jwt-decode"
  import { useRouter } from "vue-router"
  import { logout } from "../services/authService.js"


  const router = useRouter()
  const userRole = ref("")
  const token = localStorage.getItem("token");

  
onMounted(() => {
  const token = localStorage.getItem("token")
  
  // Step 1: If token does not exist, logout
  if (!token) {
    logout()
    return
  }

  // Step 2: If token exists, try to decode it
  try {
    const decoded = jwt_decode.default(token)
    userRole.value =
      decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || ""
  } catch (err) {
    console.warn("Invalid token, logging out")
    logout()
  }
})

  function goToUserList() {
    router.push("/userlist")
  }

  function goToRoleList() {
    router.push("/role-list")
  }

  function goToEmployeeList() {
    router.push("/employee-list")
  }
</script>

<style scoped>
  .dashboard {
    min-height: 100vh;
    background: linear-gradient(135deg, #e0f0ff, #ffffff);
    overflow: visible; /* add this */
    position: relative; /* ensure relative for absolute children */
  }

  .dashboard-card {
    cursor: pointer;
    border-radius: 1rem;
    box-shadow: 0 8px 20px rgba(0, 0, 0, 0.1);
    transition: transform 0.3s, box-shadow 0.3s, background 0.3s;
    background: linear-gradient(135deg, #0d6efd, #6f42c1);
    color: white;
  }

    .dashboard-card:hover {
      transform: translateY(-5px);
      box-shadow: 0 12px 25px rgba(0, 0, 0, 0.2);
      background: linear-gradient(135deg, #6f42c1, #0d6efd);
    }

  .card-title {
    font-weight: bold;
  }

  .card-text {
    font-size: 0.9rem;
  }

  .btn-light {
    background: rgba(255, 255, 255, 0.85);
    color: #0d6efd;
    font-weight: bold;
    transition: background 0.3s, color 0.3s;
  }

    .btn-light:hover {
      background: white;
      color: #6f42c1;
    }

  i {
    color: white;
  }

  /* Floating background circles */
  .background-circle {
    position: absolute;
    border-radius: 50%;
    opacity: 0.3;
    z-index: 0;
    animation: float 20s infinite linear;
  }

    /* Different sizes and positions */
    .background-circle.one {
      width: 200px;
      height: 200px;
      background: radial-gradient(circle, #0d6efd 0%, transparent 70%);
      top: 10%;
      left: 10%;
    }

    .background-circle.two {
      width: 150px;
      height: 150px;
      background: radial-gradient(circle, #6f42c1 0%, transparent 70%);
      bottom: 10%;
      right: 5%;
    }

    .background-circle.three {
      width: 300px;
      height: 300px;
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

</style>
