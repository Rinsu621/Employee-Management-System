import { createRouter, createWebHistory } from 'vue-router';
import Login from '../components/Auth/Login.vue';  // updated path
import Dashboard from '../components/Dashboard.vue'
import UserList from '../components/UserList.vue'
const routes = [
  { path: '/', redirect: '/login' },
  { path: '/login', component: Login },
  { path: '/dashboard', component: Dashboard },
  { path: '/userlist', component: UserList }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;
