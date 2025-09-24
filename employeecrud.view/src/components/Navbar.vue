<template>
  <nav class="navbar navbar-expand-lg navbar-light bg-light mb-4">
    <div class="container-fluid">
      <router-link class="navbar-brand" to="/dashboard">
        Employee CRUD
      </router-link>

      <div class="d-flex ms-auto">
        <span class="nav-link me-3">Hello, {{ userName }}</span>
        <button class="btn btn-outline-primary btn-sm me-2" @click="changePassword">
          Change Password
        </button>
        <button class="btn btn-outline-danger btn-sm" @click="logout">
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
const userRole = ref("")

onMounted(() => {
  const token = localStorage.getItem("token")
  if (token) {
    try {
      const decoded = jwt_decode.default(token)
      userName.value = localStorage.getItem("name")
      userRole.value =
        decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || ""
    } catch (err) {
      console.error("Invalid token", err)
      userName.value = ""
      userRole.value = ""
    }
  }
})

function logout() {
  localStorage.removeItem("token")
  localStorage.removeItem("refreshToken")
  localStorage.removeItem("name")
  router.push("/login")
}

function changePassword() {
  router.push("/change-password")
}
</script>

<style>
  .navbar-nav .nav-link {
    color: #000;
  }
</style>
