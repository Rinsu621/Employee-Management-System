import { createRouter, createWebHistory } from 'vue-router';
import Login from '../components/Auth/Login.vue';  // updated path

const routes = [
  { path: '/', redirect: '/login' },
  { path: '/login', component: Login },
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;
