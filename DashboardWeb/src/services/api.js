import axios from 'axios'

const apiClient = axios.create({
    baseURL: import.meta.env.VITE_SERVER_BASE_URL || 'http://localhost:5500/api/',
    headers: {
        'Content-Type': 'application/json',
    },
})

export default {
    async get(url, config = {}) {
        const { data } = await apiClient.get(url, config)
        return data
    },

    async delete(url, config = {}) {
        const { data } = await apiClient.delete(url, config)
        return data
    },

    async post(url, body = {}, config = {}) {
        const { data } = await apiClient.post(url, body, config)
        return data
    },

    async put(url, body = {}, config = {}) {
        const { data } = await apiClient.put(url, body, config)
        return data
    },
}
