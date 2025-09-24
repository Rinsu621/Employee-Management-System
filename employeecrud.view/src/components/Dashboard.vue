<template>
  <div>
    <!-- Navbar -->
    <nav class="navbar navbar-expand-lg navbar-light bg-light mb-4">
      <div class="container-fluid">
        <a class="navbar-brand" href="#">Employee CRUD</a>

        <div class="collapse navbar-collapse justify-content-end">
          <ul class="navbar-nav">
            <li class="nav-item me-3">
              <span class="nav-link">Hello, {{ userName }}</span>
            </li>
            <li class="nav-item me-3">
              <button class="btn btn-outline-primary btn-sm" @click="changePassword">
                Change Password
              </button>
            </li>
            <li class="nav-item">
              <button class="btn btn-outline-danger btn-sm" @click="logout">
                Logout
              </button>
            </li>
          </ul>
        </div>
      </div>
    </nav>

    <!-- Main Dashboard Content -->
    <div class="container">
      <h2>Welcome to Dashboard</h2>
    </div>
  </div>
</template>

<script setup>
  import { ref, onMounted } from 'vue';
  import { useRouter } from 'vue-router';

  const router = useRouter();
  const userName = ref('');

  onMounted(() => {
    userName.value = localStorage.getItem('name') || '';
  });

  // Logout function
  function logout() {
    localStorage.removeItem("token");
    localStorage.removeItem("refreshToken");
    localStorage.removeItem("name");
    userName.value = ''; 
    router.push("/login");
  }

  // Change password function
  function changePassword() {
    router.push("/change-password");
  }
</script>

<style>
  .navbar-nav .nav-link {
    color: #000;
  }
</style>
