<template>
  <div class="login-page">
    <!-- Background Animation -->
    <div class="animated-bg">
      <div class="circle" v-for="n in 8" :key="'c' + n"></div>
      <div class="square" v-for="n in 6" :key="'s' + n"></div>
    </div>

    <div class="container d-flex justify-content-center align-items-center min-vh-100">
      <div class="card shadow-lg p-4 rounded login-card">
        <h2 class="text-center mb-4 fw-bold">Login</h2>
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
            <button type="submit" class="btn btn-primary w-100 fw-bold shadow">Login</button>
          </div>

          <div v-if="error" class="alert alert-danger text-center">
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
      if (err.response?.data?.error) {
        error.value = err.response.data.error;
      } else {
        error.value = 'Login failed';
      }
    }
  }
</script>

<style scoped>
  /* Background */
  .login-page {
    position: relative;
    overflow: hidden;
    min-height: 100vh;
   
  }

  .animated-bg {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    z-index: 0;
  }

  .circle, .square {
    position: absolute;
    border-radius: 50%;
    animation: float 15s linear infinite;
  }

  .square {
    border-radius: 10%;
    background: rgba(255, 255, 255, 0.1);
    animation-duration: 20s;
  }

  /* Random positions and sizes */
  .circle:nth-child(1) {
    width: 120px;
    height: 120px;
    top: 10%;
    left: 5%;
    animation-delay: 0s;
  }

  .circle:nth-child(2) {
    width: 80px;
    height: 80px;
    top: 50%;
    left: 20%;
    animation-delay: 3s;
  }

  .circle:nth-child(3) {
    width: 60px;
    height: 60px;
    top: 30%;
    left: 70%;
    animation-delay: 5s;
  }

  .circle:nth-child(4) {
    width: 100px;
    height: 100px;
    top: 80%;
    left: 60%;
    animation-delay: 2s;
  }

  .circle:nth-child(5) {
    width: 50px;
    height: 50px;
    top: 60%;
    left: 40%;
    animation-delay: 4s;
  }

  .circle:nth-child(6) {
    width: 90px;
    height: 90px;
    top: 20%;
    left: 80%;
    animation-delay: 6s;
  }

  .circle:nth-child(7) {
    width: 70px;
    height: 70px;
    top: 40%;
    left: 10%;
    animation-delay: 1s;
  }

  .circle:nth-child(8) {
    width: 110px;
    height: 110px;
    top: 70%;
    left: 30%;
    animation-delay: 7s;
  }

  .square:nth-child(1) {
    width: 60px;
    height: 60px;
    top: 15%;
    left: 50%;
    animation-delay: 0s;
  }

  .square:nth-child(2) {
    width: 40px;
    height: 40px;
    top: 70%;
    left: 25%;
    animation-delay: 2s;
  }

  .square:nth-child(3) {
    width: 50px;
    height: 50px;
    top: 40%;
    left: 80%;
    animation-delay: 4s;
  }

  .square:nth-child(4) {
    width: 70px;
    height: 70px;
    top: 60%;
    left: 10%;
    animation-delay: 1s;
  }

  .square:nth-child(5) {
    width: 30px;
    height: 30px;
    top: 85%;
    left: 70%;
    animation-delay: 3s;
  }

  .square:nth-child(6) {
    width: 90px;
    height: 90px;
    top: 20%;
    left: 20%;
    animation-delay: 5s;
  }


  @keyframes float {
    0% {
      transform: translateY(0) rotate(0deg);
    }

    50% {
      transform: translateY(-20px) rotate(180deg);
    }

    100% {
      transform: translateY(0) rotate(360deg);
    }
  }

  /* Card & Form */
  .login-card {
    position: relative;
    z-index: 10;
    background-color: #ffffffcc;
    border-radius: 1rem;
    padding: 2rem;
    width: 100%;
    max-width: 400px;
  }

  input.form-control {
    border-radius: 0.5rem;
  }

  button.btn-primary {
    border-radius: 0.5rem;
    font-weight: 600;
    letter-spacing: 0.5px;
    transition: transform 0.3s, box-shadow 0.3s;
  }

    button.btn-primary:hover {
      transform: translateY(-2px);
      box-shadow: 0px 6px 15px rgba(0, 0, 0, 0.2);
    }
</style>
