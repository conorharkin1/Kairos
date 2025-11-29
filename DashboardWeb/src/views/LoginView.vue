<template>
    <div v-if="isLoading" class="loading-container loader"></div>
    <div v-else class="content">
        <div>
            <h2>Welcome back</h2>
        </div>
        <div class="wrapper">
            <form @submit="loginUser">
                <h1>Login</h1>
                <div class="input-box">
                    <input type="email" v-model="email" required placeholder="Email" />
                </div>
                <div class="input-box">
                    <input type="password" v-model="password" required placeholder="Password" />
                </div>
                <button type="submit" class="btn">Login</button>
                <div class="register-link">
                    <p>
                        <a @click="goToRegister">Don't have an account? Register</a>
                    </p>
                </div>
            </form>
        </div>
    </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { login } from '@/services/authService'

const email = ref('')
const password = ref('')
const router = useRouter()
const isLoading = ref(false)

const loginUser = async (event) => {
    event.preventDefault()
    isLoading.value = true

    try {
        const user = await login({
            email: email.value,
            password: password.value,
        })
        router.push('/dashboard')
    } catch (error) {
        alert('Login failed: ' + error.message)
    } finally {
        isLoading.value = false
    }
}

const goToRegister = () => {
    router.push('/register')
}
</script>
<style scoped lang="scss">
.content {
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 100vh;
    background: $primary-dark-background;
}

.content h2 {
    padding-right: 2rem;
    color: $primary-text-light;
    font-size: 30px;
}

.wrapper {
    width: 420px;
    color: $primary-text-light;
    border-radius: 10px;
    padding: 30px 40px;
    display: flex;
    justify-content: center;
}

.wrapper h1 {
    font-size: 36px;
    text-align: center;
}

.wrapper .input-box {
    width: 100%;
    height: 50px;
    margin: 30px 0;
    display: flex;
    justify-content: center;
    align-items: center;
}

.input-box input {
    width: 100%;
    height: 80%;
    background: transparent;
    border: 2px solid $primary-red-colour;
    border-radius: 20px;
    outline: none;
    color: $primary-text-light;
    padding-left: 1rem;
}

.wrapper .btn {
    width: 100%;
    height: 45px;
    background: $primary-red-colour;
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

.loading-container {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-grow: 1;
}
.loader {
  width: fit-content;
  font-weight: bold;
  font-family: sans-serif;
  padding-bottom: 8px;
  background: linear-gradient(currentColor 0 0) 0 100%/0% 3px no-repeat;
  animation: l2 2s linear infinite;
  color: $primary-red-colour;
}
.loader:before {
  content:"Loading..."
}
@keyframes l2 {to{background-size: 100% 3px}}
</style>
