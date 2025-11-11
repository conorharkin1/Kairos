import axios from 'axios'

const apiClient = axios.create({
    baseURL: import.meta.env.VITE_SERVER_BASE_URL || 'http://localhost:5500/api/',
    headers: {
        'Content-Type': 'application/json',
    },
})

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
