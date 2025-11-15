import api from './api.js'

export async function register(userData) {
    return api.post('auth/register', userData)
        .then(res => {
            const { token } = res.data
            if (token) {
                localStorage.setItem('authToken', token)
            }
            return res.data
        })
        .catch(err => { 
            throw err
        })
}

export async function login(credentials) {
    return api.post('auth/login', credentials)
        .then(res => {
            const { token } = res.data
            if (token) {
                localStorage.setItem('authToken', token)
            }
            return res.data
        })
        .catch(err => { 
            throw err
        })
}

export function logout() {
    localStorage.removeItem('authToken')
}

export function getToken() {
    return localStorage.getItem('authToken')
}

export function isAuthenticated() {
    return !!getToken()
}

