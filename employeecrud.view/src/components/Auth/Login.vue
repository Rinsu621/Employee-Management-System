<template>
  <div class="container mt-5">
    <div class="row justify-content-center">
      <div class="col-md-4">
        <h2 class="text-center mb-4">Login</h2>
        <form @submit.prevent="handleLogin">
          <div class="mb-3">
            <label for="email" class="form-label">Email</label>
            <input type="email"
                   id="email"
                   v-model="email"
                   class="form-control"
                   placeholder="Enter your email"
                   required />
          </div>

          <div class="mb-3">
            <label for="password" class="form-label">Password</label>
            <input type="password"
                   id="password"
                   v-model="password"
                   class="form-control"
                   placeholder="Enter your password"
                   required />
          </div>

          <div class="mb-3">
            <button type="submit" class="btn btn-primary w-100">Login</button>
          </div>

          <div v-if="error" class="alert alert-danger">
            {{ error }}
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
  import { ref } from 'vue';
  import { useRouter } from 'vue-router';
  import { login } from '../../services/authService.js';

  const router = useRouter();
  const email = ref('');
  const password = ref('');
  const error = ref('');

  async function handleLogin() {
    error.value = '';
    try {
      const result = await login(email.value, password.value);
      router.push('/dashboard');
    } catch (err) {
      if (err.response && err.response.data && err.response.data.error) {
        error.value = err.response.data.error; 
      } else {
        error.value = 'Login failed';
      }
    }
  }
</script>

<style>
  body {
    background-color: #f8f9fa;
  }
</style>
