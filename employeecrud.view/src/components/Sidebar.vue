<template>
  <div :class="['sidebar', { 'toggled': isToggled }]">
    <ul class="sidebar-nav">
      <li class="sidebar-brand">
        <div class="brand-title d-flex align-items-center justify-content-between">
          <router-link v-if="!isToggled"
                       to="/dashboard"
                       class="fw-bold text-white ms-2 text-decoration-none">
            Employee CRUD
          </router-link>

          <a href="#" @click.prevent="toggleMenu" class="toggle-btn">
            <i class="fa fa-bars" style="font-size:20px; color:white;"></i>
          </a>
        </div>
      </li>

      <li v-for="(item, index) in menuItems" :key="index">
        <router-link :to="item.link" class="nav-link">
          <i :class="item.icon"></i>
          <span v-if="!isToggled" style="margin-left:10px;">{{ item.name }}</span>
        </router-link>
      </li>
    </ul>
  </div>
</template>

<script>
  import { useRouter } from "vue-router"
  export default {
    name: "Sidebar",
    data() {
      return {
        isToggled: false,
        menuItems: [
          { name: "User List", link: "/userlist", icon: "fa fa-users" },
          { name: "Role List", link: "/role-list", icon: "fa fa-shield" },
          { name: "Employee List", link: "/employee-list", icon: "fa fa-id-badge" },
          { name: "Add Salary", link: "/salary/add", icon: "fa fa-money-bill" }
        ],
      };
    },
    methods: {
      toggleMenu() {
        this.isToggled = !this.isToggled;
      },
    },
  };
</script>

<style scoped>
  .sidebar {
    width: 220px;
    background: linear-gradient(135deg, #0d6efd, #6f42c1);
    min-height: 100vh;
    transition: width 0.3s ease;
    color: white;
    box-shadow: 2px 0 10px rgba(0, 0, 0, 0.1);
    position: sticky;
    top: 0;
  }

    .sidebar.toggled {
      width: 70px;
    }

  .sidebar-nav {
    list-style: none;
    padding: 0;
    margin: 0;
  }

  .sidebar-brand {
    padding: 15px;
    border-bottom: 1px solid rgba(255, 255, 255, 0.2);
  }

  .nav-link {
    display: flex;
    align-items: center;
    padding: 10px 15px;
    color: rgba(255, 255, 255, 0.85);
    text-decoration: none;
    border-radius: 8px;
    margin: 4px 10px;
    transition: background 0.3s, color 0.3s;
  }

    .nav-link:hover,
    .router-link-active {
      background: rgba(255, 255, 255, 0.15);
      color: white;
    }
</style>
