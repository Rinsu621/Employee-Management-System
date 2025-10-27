<template>
  <div class="layout d-flex">
    <Sidebar />

    <div class="main-content flex-grow-1">
      <Navbar />
      <div class="content-wrapper p-4">
        <slot />
      </div>
    </div>
  </div>
</template>

<script setup>
  import Sidebar from "../components/Sidebar.vue";
  import Navbar from "../components/Navbar.vue";
</script>

<style scoped>
  .layout {
    min-height: 100vh;
    display: flex;
    background: linear-gradient(135deg, #e0f0ff, #ffffff);
  }

  .main-content {
    flex: 1;
    display: flex;
    flex-direction: column;
  }

  .content-wrapper {
    flex-grow: 1;
    padding: 2rem;
    background: transparent;
  }
</style>
