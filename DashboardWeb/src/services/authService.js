import api from './api.js'

export async function register(userData) {
    const response = await api.post('auth/register', userData)

    if (!response.ok) {
        console.log('Registration failed:', await response.text())
    }

    return await response.json()
}

export async function login(credentials) {
    const response = await api.post('auth/login', credentials)

    if (!response.ok) {
        console.log('Login failed:', await response.text())
    }

    return await response.json()
}
