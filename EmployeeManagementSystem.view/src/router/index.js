import { createRouter, createWebHistory } from 'vue-router';
import { getToken, isTokenValid, getUserRole, logout } from '../services/authService.js';

import Login from '../components/Auth/Login.vue';  // updated path
import Dashboard from '../components/Dashboard.vue'
import UserList from '../components/UserList.vue'
import RoleList from '../components/RoleList.vue'
import EmployeeList from '../components/EmployeeList.vue'
import Profile from '../components/Profile.vue'
import AddSalary from '../components/Salary.vue'
const routes = [
  { path: '/', redirect: '/login' },
  { path: '/login', component: Login },
  { path: '/dashboard', component: Dashboard, meta: { requiresAuth: true } },
  { path: '/userlist', component: UserList, meta: { requiresAuth: true, roles: ["Admin", "Manager"] } },
  { path: '/role-list', component: RoleList, meta: { requiresAuth: true, roles: ["Admin", "Manager"] } },
  { path: '/employee-list', component: EmployeeList, meta: { requiresAuth: true } },
  { path: '/profile', component: Profile, meta: { requiresAuth: true } },
   { path: '/salary/add', component: AddSalary, meta: { requiresAuth: true, roles: ["Admin", "Manager"] } }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;

router.beforeEach((to, from, next) => {
  if (!to.meta?.requiresAuth) {
    next();
  } else if (!getToken()) {
    logout(); 
  } else if (to.meta.roles) {
    const role = getUserRole();
    if (!role || !to.meta.roles.includes(role)) {
      next('/dashboard'); 
    } else {
      next();
    }
  } else {
    next();
  }
});


