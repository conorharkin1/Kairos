<template>
    <body>
        <div>
            <h2>Organise your thoughts</h2>
        </div>
        <div class="wrapper">
            <form @submit="registerUser">
                <h1>Create a new account</h1>
                <div class="input-box">
                    <input type="email" v-model="email" required placeholder="Email" />
                </div>
                <div class="input-box">
                    <input type="password" v-model="password" required placeholder="Password" />
                </div>
                <div class="input-box">
                    <input
                        type="password"
                        v-model="confirmPassword"
                        required
                        placeholder="Confirm password"
                    />
                </div>
                <button class="btn" type="submit">Register</button>
            </form>
        </div>
    </body>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { register } from '@/services/authService'

const email = ref('')
const password = ref('')
const confirmPassword = ref('')
const router = useRouter()

const registerUser = async (event) => {
    event.preventDefault()

    if (password.value !== confirmPassword.value) {
        alert('Passwords do not match!')
        return
    }

    try {
        await register({
            email: email.value,
            password: password.value,
        })
        router.push('/login')
    } catch (error) {
        alert('Registration failed: ' + error.message)
    }
}
</script>

<style scoped lang="scss">
body {
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 100vh;
    background: $primary-background-dark;
}

body h2 {
    padding-right: 2rem;
    color: $primary-text-light;
}

.wrapper {
    width: 420px;
    background: $primary-background-dark;
    color: $primary-text-light;
    border-radius: 10px;
    padding: 30px 40px;
    display: flex;
    justify-content: center;
    align-items: center;
}

.wrapper h1 {
    font-size: 36px;
    text-align: center;
}

.wrapper .input-box {
    width: 100%;
    height: 50px;
    margin: 30px 0;
}

.input-box input {
    width: 80%;
    height: 80%;
    background: transparent;
    border: 2px solid $primary-red-outline;
    border-radius: 20px;
    outline: none;
    color: $primary-text-light;
    padding-left: 1rem;
}

.wrapper .btn {
    width: 100%;
    height: 45px;
    background: $primary-red-outline;
    border: none;
    outline: none;
    border-radius: 20px;
    cursor: pointer;
    color: $primary-text-light;
    font-weight: 600;
}

.wrapper .register-link {
    font-size: 14px;
    text-align: center;
    margin: 20px 0 15px;
}

.register-link p a {
    color: $primary-text-light;
    text-decoration: none;
    font-weight: 600;
    font-size: 16px;
}

.register-link p a:hover {
    text-decoration: underline;
}
</style>
