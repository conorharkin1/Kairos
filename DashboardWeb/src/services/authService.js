const serverUrl = import.meta.env.VITE_SERVER_BASE_URL

export async function register(userData) {
    const response = await fetch(`${serverUrl}api/auth/register`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(userData),
    })

    if (!response.ok) {
        console.log('Registration failed:', await response.text())
    }

    return await response.json()
}

export async function login(credentials) {
    const response = await fetch(`${serverUrl}api/auth/login`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(credentials),
    })

    if (!response.ok) {
        console.log('Login failed:', await response.text())
    }

    return await response.json()
}
