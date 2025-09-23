<template>
  <div class="container mt-5">
    <div class="row justify-content-center">
      <div class="col-md-4">
        <h2 class="text-center mb-4">Login</h2>
        <form @submit.prevent="login">
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
          <button type="submit" class="btn btn-primary w-100">Login</button>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
  import { login } from '../../services/authService.js';


  export default {
    name: "Login",
    data() {
      return {
        email: "",
        password: "",
        error: ""
      };
    },
    methods: {
      async login() {
        try {
          const result = await login(this.email, this.password);
          alert('Login successful! Token stored.');
          this.$router.push('/dashboard'); // redirect after login
        } catch (err) {
          this.error = err.message || 'Login failed';
          alert(this.error);
        }
      }
    }
  };
</script>

<style>
  /* Optional: small style tweaks */
  body {
    background-color: #f8f9fa;
  }
</style>
