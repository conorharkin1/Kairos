import router from '@/router'
import axios from 'axios'
import { ref } from 'vue'

export const isLoading = ref(false)

const apiClient = axios.create({
    baseURL: import.meta.env.VITE_SERVER_BASE_URL || 'http://localhost:5500/api/',
    headers: {
        'Content-Type': 'application/json',
    },
})

// Add request interceptor to include JWT token
apiClient.interceptors.request.use(
    config => {
        const token = localStorage.getItem('authToken')
        if (token) {
            config.headers.Authorization = `Bearer ${token}`
        }
        isLoading.value = true
        return config
    },
    error => {
        isLoading.value = false
        return Promise.reject(error)
    }
)

// Add response interceptor to handle 401 errors
apiClient.interceptors.response.use(
    response => {
        isLoading.value = false
        return response
    },
    error => {
        isLoading.value = false
        if (error.response?.status === 401) {
            localStorage.removeItem('authToken')
            router.push('/login')
        }
        return Promise.reject(error)
    }
)

export default {
    async get(url, config = {}) {
        return await apiClient.get(url, config)
    },

    async delete(url, config = {}) {
        return await apiClient.delete(url, config)
    },

    async post(url, body = {}, config = {}) {
        return await apiClient.post(url, body, config)
    },

    async put(url, body = {}, config = {}) {
        return await apiClient.put(url, body, config)
    },
}
