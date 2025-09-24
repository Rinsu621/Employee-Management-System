<template>
  <div>
    <Navbar />

    <div class="container">
      <h2>Welcome to Dashboard</h2>

      <div class="mt-4">
        <button v-if="userRole === 'Admin'" class="btn btn-primary me-2 mb-2" @click="goToUserList">
          User List
        </button>

        <button v-if="userRole === 'Admin'" class="btn btn-primary me-2 mb-2" @click="goToRoleList">
          Role List
        </button>

        <button v-if="userRole === 'Admin' || userRole === 'Manager'" class="btn btn-primary me-2 mb-2" @click="goToEmployeeList">
          Employee List
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
  import Navbar from "../components/Navbar.vue"
  import { ref, onMounted } from "vue"
  import * as jwt_decode from "jwt-decode"
  import { useRouter } from "vue-router"

  const router = useRouter()
  const userRole = ref("")

  onMounted(() => {
    const token = localStorage.getItem("token")
    if (token) {
      try {
        const decoded = jwt_decode.default(token)
        userRole.value =
          decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || ""
      } catch (err) {
        userRole.value = ""
      }
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
