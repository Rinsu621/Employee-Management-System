<template>
  <div>
    <Navbar />
    <div class="container mt-4">
      <h2 class="mb-4">Role List</h2>
      <table class="table table-bordered table-hover">
        <thead class="table-dark">
          <tr>
            <th>S.No</th>
            <th>Role Name</th>
          </tr>
        </thead>
        <tbody>
          <tr v-if="roles.length === 0">
            <td colspan="3" class="text-center">No Roles Found</td>
          </tr>
          <tr v-for="(role, index) in roles" :key="role.id">
            <td>{{ index + 1 }}</td>
            <td>{{ role}}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
  import Navbar from "./Navbar.vue";
  import { getRoles } from "../services/employeeService";
  import { logout } from '../services/authService'
const roles = ref([]);

async function fetchRoles() {
  try {
    const res = await getRoles();
    roles.value = Array.isArray(res.data) ? res.data : [];
  } catch (err) {
    console.error("Error fetching roles:", err);
    roles.value = [];
  }
}

onMounted(() => {
  fetchRoles();
});
</script>
