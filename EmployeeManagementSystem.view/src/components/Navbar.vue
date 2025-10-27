<template>
  <nav class="navbar navbar-expand-lg navbar-light bg-gradient shadow-sm py-2 ">
    <div class="container-fluid">

      <div class="d-flex ms-auto align-items-center">
        <router-link to="/profile" class="d-flex align-items-center text-decoration-none me-3 profile-link">
          <div class="avatar-wrapper me-2">
            <img :src="userAvatar ? userAvatar : '/OIP.jpeg'" alt="Profile" class="rounded-circle avatar-img" />
          </div>
          <span class="fw-semibold text-dark">Hello, {{ userName }}</span>
        </router-link>

        <button class="btn btn-outline-primary btn-sm me-2 action-btn" @click="changePassword">
          Change Password
        </button>
        <button class="btn btn-outline-danger btn-sm action-btn" @click="logout">
          Logout
        </button>
      </div>
    </div>
  </nav>
</template>

<script setup>
  import { ref, onMounted } from "vue"
  import { useRouter } from "vue-router"
  import * as jwt_decode from "jwt-decode"

  const router = useRouter()
  const userName = ref("")
  const userAvatar = ref("")
  const userRole = ref("")

  onMounted(() => {
    const token = sessionStorage.getItem("token")
    if (token) {
      try {
        const decoded = jwt_decode.default(token)
        userName.value = sessionStorage.getItem("name") || decoded["name"] || "User"
        userAvatar.value = sessionStorage.getItem("avatar") || ""
        userRole.value = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || ""
      } catch (err) {
        console.error("Invalid token", err)
        userName.value = "User"
        userRole.value = ""
      }
    }
  })

  function logout() {
    sessionStorage.removeItem("token")
    sessionStorage.removeItem("refreshToken")
    sessionStorage.removeItem("name")
    sessionStorage.removeItem("avatar")
    router.push("/login")
  }

  function changePassword() {
    router.push("/change-password")
  }
</script>

<style scoped>
  .navbar {
    background: linear-gradient(135deg, #ffffff, #e0f0ff);
    border-radius: 0.5rem;
    transition: box-shadow 0.3s;
  }

    .navbar:hover {
      box-shadow: 0 8px 25px rgba(0, 0, 0, 0.15);
    }

  .navbar-brand {
    font-size: 1.4rem;
  }

  .profile-link:hover {
    text-decoration: none;
    transform: scale(1.05);
    transition: transform 0.2s;
  }

  .avatar-wrapper {
    width: 38px;
    height: 38px;
    overflow: hidden;
    border-radius: 50%;
    border: 2px solid #0d6efd;
    transition: transform 0.3s, box-shadow 0.3s;
  }

    .avatar-wrapper:hover {
      transform: scale(1.1);
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
    }

  .avatar-img {
    width: 100%;
    height: 100%;
    object-fit: cover;
  }

  .action-btn {
    font-weight: 600;
    transition: transform 0.2s, box-shadow 0.2s;
  }

    .action-btn:hover {
      transform: translateY(-2px);
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
    }
</style>
