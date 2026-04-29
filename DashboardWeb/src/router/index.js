import { createRouter, createWebHistory } from 'vue-router'
import Dashboard from '@/views/DashboardView.vue'
import LoginView from '@/views/LoginView.vue'
import RegisterView from '@/views/RegisterView.vue'

// ATTRIBUTION
// The Below code is adapted from the following source:
// https://router.vuejs.org/guide/

const routes = [
    { path: '/', redirect: '/login' },
    { path: '/login', component: LoginView, meta: { requiresAuth: false }},
    { path: '/register', component: RegisterView, meta: { requiresAuth: false }},
    { path: '/dashboard', component: Dashboard, meta: { requiresAuth: true } },
]

const router = createRouter({
    history: createWebHistory(),
    routes,
})

router.beforeEach((to, from, next) => {
    const isLoggedIn = !!localStorage.getItem('authToken')

    if (to.meta.requiresAuth && !isLoggedIn) {
        return next('/login')
    }

    next()
})

export default router
