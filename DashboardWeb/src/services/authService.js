import api from './api.js'

export async function register(userData) {
    return api.post('auth/register', userData)
        .then(res => {
            return res.data;
        })
        .catch(err => { 
            throw err;
        });
}

export async function login(credentials) {
    return api.post('auth/login', credentials)
        .then(res => {
            return res.data;
        })
        .catch(err => { 
            throw err;
        });
}
