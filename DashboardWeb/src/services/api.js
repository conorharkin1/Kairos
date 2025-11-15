import axios from 'axios'

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
        return config
    },
    error => Promise.reject(error)
)

// Add response interceptor to handle 401 errors
apiClient.interceptors.response.use(
    response => response,
    error => {
        if (error.response?.status === 401) {
            localStorage.removeItem('authToken')
            window.location.href = '/login'
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
