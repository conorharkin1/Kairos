import { createRouter, createWebHistory } from 'vue-router'
import Dashboard from '../views/Dashboard.vue'
import Calendar from '../views/Calendar.vue'

const routes = [
  {
    path: '/',
    redirect: '/dashboard',
  },
  {
    path: '/dashboard',
    component: Dashboard,
  },
  {
    path: '/calendar',
    component: Calendar,
  },
]
const router = createRouter({
  history: createWebHistory(),
  routes,
})

export default router
