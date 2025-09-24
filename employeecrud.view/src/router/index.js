import { createRouter, createWebHistory } from 'vue-router';
import Login from '../components/Auth/Login.vue';  // updated path
import Dashboard from '../components/Dashboard.vue'
const routes = [
  { path: '/', redirect: '/login' },
  { path: '/login', component: Login },
  { path: '/dashboard', component: Dashboard }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;
