import api from './api.js'
import router from '@/router'

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
    router.push('/login')
}

export async function deleteAccount() {
    return api.delete('auth/delete')
        .then(res => {
            localStorage.removeItem('authToken')
            router.push('/login')
            return res.data
        })
        .catch(err => {
            throw err
        })
}

export function getToken() {
    return localStorage.getItem('authToken')
}

export function isAuthenticated() {
    return !!getToken()
}

