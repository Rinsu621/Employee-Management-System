import { createRouter, createWebHistory } from 'vue-router';
import Login from '../components/Auth/Login.vue';  // updated path
import Dashboard from '../components/Dashboard.vue'
import UserList from '../components/UserList.vue'
import RoleList from '../components/RoleList.vue'
import EmployeeList from '../components/EmployeeList.vue'
import Profile from '../components/Profile.vue'
const routes = [
  { path: '/', redirect: '/login' },
  { path: '/login', component: Login },
  { path: '/dashboard', component: Dashboard },
  { path: '/userlist', component: UserList },
  { path: '/role-list', component: RoleList },
  { path: '/employee-list', component: EmployeeList },
  { path: '/profile', component: Profile }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;
